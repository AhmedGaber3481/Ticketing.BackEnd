using LinkDev.Ticketing.Logging.Enums;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LinkDev.Ticketing.Logging.Infra
{
    public class Logger : Logging.Application.Interfaces.ILogger
    {
        private string messageTemplate = "{LogMessage} {ClassName} {MethodName} {CorrelationId} {SourceType} {Id1} {Description} \n\n";

        public void LogInformation(string message, string className, string methodName, Guid correlationId, LoggerSourceType sourceType = LoggerSourceType.None, string? id1 = null, string? description = null)
        {
            Serilog.Log.Information(messageTemplate, message, className, methodName, correlationId.ToString(), sourceType.ToString(), id1, description);
        }
        public void LogError(string message, string className, string methodName, Guid correlationId, LoggerSourceType sourceType = LoggerSourceType.PortalUser, string? id1 = null, string? description = null)
        {
            Serilog.Log.Error(messageTemplate, message, className, methodName, correlationId.ToString(), sourceType.ToString(), id1, description);
        }
        public void LogWarning(string message, string className, string methodName, Guid correlationId, LoggerSourceType sourceType = LoggerSourceType.None, string? id1 = null, string? description = null)
        {
            Serilog.Log.Warning(messageTemplate, message, className, methodName, correlationId.ToString(), sourceType.ToString(), id1, description);
        }
        public void LogInformation(object requestBody, string className, string methodName, Guid correlationId, LoggerSourceType sourceType = LoggerSourceType.PortalUser, string? id1 = null, string? description = null)
        {
            string message = JsonConvert.SerializeObject(requestBody);
            Serilog.Log.Information(messageTemplate, message, className, methodName, correlationId.ToString(), sourceType.ToString(), id1, description);
        }

        //public void Write(int level, string message, string className, string methodName, Guid correlationId, LoggerSourceType sourceType)
        //{
        //    Serilog.Log.Write((Serilog.Events.LogEventLevel)level, messageTemplate, message, className, methodName, correlationId.ToString(), sourceType.ToString(), null, null);
        //}

        public void LogDebug(string message, string className, string methodName, Guid correlationId, LoggerSourceType sourceType = LoggerSourceType.None, string? id1 = null, string? description = null)
        {
            Serilog.Log.Debug(messageTemplate, message, className, methodName, correlationId.ToString(), sourceType.ToString(), id1, description);
        }

        public void LogError(Exception exp, string message, string className, string methodName, Guid correlationId, LoggerSourceType sourceType = LoggerSourceType.None, string? id1 = null, string? description = null)
        {
            Serilog.Log.Error(exp, messageTemplate, "Exception", className, methodName, correlationId.ToString(), sourceType.ToString(), id1, description);
        }
    }

}
