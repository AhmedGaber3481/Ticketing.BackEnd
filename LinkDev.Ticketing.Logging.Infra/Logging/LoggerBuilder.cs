using LinkDev.Ticketing.Logging.Enums;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;
using System.Data;
using System.Text;

namespace LinkDev.Ticketing.Logging.Infra
{
    public class LoggerBuilder
    {
        private LoggerConfiguration loggerConfiguration = new LoggerConfiguration();

        public LoggerType LoggerType { get; set; } = LoggerType.File;
        public string? ConnectionString { get; set; }
        public bool AutoCreateSqlTable { get; set; }
        public LogEventLevel MinimumLogLevel { get; set; } = LogEventLevel.Information;
        public string? LogPath { get; set; }
        public string? LogFile { get; set; }

        public ILogger CreateLogger()
        {
            return loggerConfiguration.CreateLogger();
        }

        public LoggerBuilder Initialize(IConfigurationSection section)
        {
            if (section == null)
            {
                throw new ArgumentNullException(nameof(section));
            }

            string? logTo = section.GetValue<string>("WriteTo");
            if (logTo != null)
            {
                if (Enum.TryParse(logTo, out LoggerType type))
                {
                    LoggerType = type;
                }
            }

            string? minimumLogLevel = section.GetValue<string>("MinimumLogLevel");
            if (minimumLogLevel != null)
            {
                if (Enum.TryParse(minimumLogLevel, out LogEventLevel logEventLevel))
                {
                    MinimumLogLevel = logEventLevel;
                }
            }

            SetMinimumLevel(MinimumLogLevel);
            
            return this;
        }

        public LoggerBuilder ConfigureLoggingSink(IConfigurationSection section)
        {
            if (section == null)
            {
                throw new ArgumentNullException(nameof(section));
            }

            if (LoggerType == LoggerType.Database)
            {
                ConnectionString = section.GetValue<string>("Args:ConnectionString");
                AutoCreateSqlTable = section.GetValue<bool>("Args:AutoCreateSqlTable");

                WriteToDatabase();
            }
            else
            {
                LogPath = section.GetValue<string>("Args:LogPath");
                LogFile = section.GetValue<string>("Args:LogFile");

                WriteToFile();
            }
            return this;
        }

        public LoggerBuilder WriteToDatabase()
        {
            ColumnOptions columnOptions = new ColumnOptions()
            {
                Store = [StandardColumn.Level, StandardColumn.TimeStamp, StandardColumn.Exception],
                AdditionalColumns = [
                      new SqlColumn("LogMessage", SqlDbType.NVarChar)
                    , new SqlColumn("ClassName", SqlDbType.NVarChar)
                    , new SqlColumn("MethodName", SqlDbType.NVarChar)
                    , new SqlColumn("SourceType", SqlDbType.NVarChar)
                    , new SqlColumn("CorrelationId", SqlDbType.VarChar , true, 100)
                    , new SqlColumn("Id1", SqlDbType.VarChar, true, 100)
                    , new SqlColumn("Description", SqlDbType.NVarChar)]
            };

            MSSqlServerSinkOptions serverSinkOptions = new MSSqlServerSinkOptions()
            {
                AutoCreateSqlTable = AutoCreateSqlTable,
                TableName = "Logs"
            };

            loggerConfiguration = loggerConfiguration
                .Enrich.FromLogContext()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("System", LogEventLevel.Warning)
                .WriteTo
                .MSSqlServer(ConnectionString, sinkOptions: serverSinkOptions, columnOptions: columnOptions);

            return this;
        }

        public LoggerBuilder WriteToFile()
        {
            StringBuilder path = new StringBuilder();
            path.AppendFormat("{0}\\{1}", LogPath, LogFile);

            loggerConfiguration = loggerConfiguration
                .WriteTo
                .File(path.ToString(), fileSizeLimitBytes: 52428800, rollingInterval: RollingInterval.Day, rollOnFileSizeLimit: true);

            return this;
        }

        public LoggerBuilder SetMinimumLevel(LogEventLevel MinimumLogLevel)
        {
            switch (MinimumLogLevel)
            {
                case LogEventLevel.Information:
                    loggerConfiguration = loggerConfiguration.MinimumLevel.Information();
                    break;
                case LogEventLevel.Warning:
                    loggerConfiguration = loggerConfiguration.MinimumLevel.Warning();
                    break;
                case LogEventLevel.Error:
                    loggerConfiguration = loggerConfiguration.MinimumLevel.Error();
                    break;
                case LogEventLevel.Fatal:
                    loggerConfiguration = loggerConfiguration.MinimumLevel.Fatal();
                    break;
                default:
                    loggerConfiguration = loggerConfiguration.MinimumLevel.Information();
                    break;
            }

            return this;
        }
    }
}
