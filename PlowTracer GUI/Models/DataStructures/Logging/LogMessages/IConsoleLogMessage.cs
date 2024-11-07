using Serilog.Events;

namespace PlowTracer.GUI.Models.DataStructures.Logging.LogMessages;

public interface IConsoleLogMessage
{
    public LogEventLevel LogLevel { get; }
    public string        Message     { get; }
}