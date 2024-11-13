using Microsoft.Extensions.Logging;

using PlowTracer.GUI.Models.Enumerations.Logging;

using Serilog.Context;

namespace PlowTracer.GUI.Models.Extensions.Logging;

internal static class LoggingExtensions
{
    internal static void LogInformation(this ILogger p_logger, LogMessageType p_messageType, string p_message)
    {
        using ( LogContext.PushProperty("Type", p_messageType.ToString()) )
        {
            p_logger.LogInformation("{Message}", p_message);
        }
    }
    
    internal static void LogWarning(this ILogger p_logger, LogMessageType p_messageType, string p_message)
    {
        using ( LogContext.PushProperty("Type", p_messageType.ToString()) )
        {
            p_logger.LogWarning("{Message}", p_message);
        }
    }
    
    internal static void LogError(this ILogger p_logger, LogMessageType p_messageType, string p_message)
    {
        using ( LogContext.PushProperty("Type", p_messageType.ToString()) )
        {
            p_logger.LogError("{Message}", p_message);
        }
    }
    
    internal static void LogCritical(this ILogger p_logger, LogMessageType p_messageType, string p_message)
    {
        using ( LogContext.PushProperty("Type", p_messageType.ToString()) )
        {
            p_logger.LogCritical("{Message}", p_message);
        }
    }
    
    internal static void LogTrace(this ILogger p_logger, LogMessageType p_messageType, string p_message)
    {
        using ( LogContext.PushProperty("Type", p_messageType.ToString()) )
        {
            p_logger.LogTrace("{Message}", p_message);
        }
    }
    
    internal static void LogDebug(this ILogger p_logger, LogMessageType p_messageType, string p_message)
    {
        using ( LogContext.PushProperty("Type", p_messageType.ToString()) )
        {
            p_logger.LogDebug("{Message}", p_message);
        }
    }
}