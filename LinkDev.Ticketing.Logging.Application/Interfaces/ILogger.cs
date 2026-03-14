using LinkDev.Ticketing.Logging.Enums;

namespace LinkDev.Ticketing.Logging.Application.Interfaces
{
    public interface ILogger
    {
        void LogInformation(string message, string className, string methodName, Guid correlationId, LoggerSourceType sourceType = LoggerSourceType.None, string? id1 = null, string? description = null);
        void LogError(string message, string className, string methodName, Guid correlationId, LoggerSourceType sourceType = LoggerSourceType.PortalUser, string? id1 = null, string? description = null);
        void LogWarning(string message, string className, string methodName, Guid correlationId, LoggerSourceType sourceType = LoggerSourceType.None, string? id1 = null, string? description = null);
        void LogInformation(object request, string className, string methodName, Guid correlationId, LoggerSourceType sourceType = LoggerSourceType.None, string? id1 = null, string? description = null);
        void LogDebug(string message, string className, string methodName, Guid correlationId, LoggerSourceType sourceType = LoggerSourceType.None, string? id1 = null, string? description = null);
        void LogError(Exception exp, string message, string className, string methodName, Guid correlationId, LoggerSourceType sourceType = LoggerSourceType.None, string? id1 = null, string? description = null);
    }
}
