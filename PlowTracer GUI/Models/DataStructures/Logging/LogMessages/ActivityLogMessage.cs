using ReactiveUI;

using Serilog.Events;

namespace PlowTracer.GUI.Models.DataStructures.Logging.LogMessages;

public class ActivityLogMessage(LogEventLevel p_logEventLevel, string p_message) : ReactiveObject, IConsoleLogMessage
{
    public LogEventLevel LogLevel { get; } = p_logEventLevel;
    public string        Message  { get; } = p_message;
}