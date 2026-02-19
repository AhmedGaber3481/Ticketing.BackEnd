using LinkDev.Ticketing.Logging.Enums;
using Microsoft.Extensions.Logging;

namespace LinkDev.Ticketing.Logging.Infra
{
    public class Logger : Application.Interfaces.ILogger
    {
        private ILogger<Logger> _logger;
        private string messageTemplate = "{LogMessage}{ClassName}{MethodName}{CorrelationId}{SourceType}{Id1}{Description}";

        public Logger(ILogger<Logger> logger)
        {
            _logger = logger;
        }

        public void LogInformation(string message, string className, string methodName, Guid correlationId, LoggerSourceType sourceType = LoggerSourceType.None, string id1 = null, string description = null)
        {
            _logger.LogInformation(messageTemplate, message, className, methodName, correlationId.ToString(), sourceType.ToString(), id1, description);
        }
        public void LogError(Exception exp, string message, string className, string methodName, Guid correlationId, LoggerSourceType sourceType = LoggerSourceType.None, string id1 = null, string description = null)
        {
            _logger.LogError(exp, messageTemplate, message, className, methodName, correlationId.ToString(), sourceType.ToString(), id1, description);
        }
        public void LogWarning(string message, string className, string methodName, Guid correlationId, LoggerSourceType sourceType = LoggerSourceType.None, string id1 = null, string description = null)
        {
            _logger.LogWarning(messageTemplate, message, className, methodName, correlationId.ToString(), sourceType.ToString(), id1, description);
        }
    }
}
