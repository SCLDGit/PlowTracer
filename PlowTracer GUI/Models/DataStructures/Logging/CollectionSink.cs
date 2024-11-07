using System.IO;
using System.Text.RegularExpressions;

using Avalonia.Collections;

using PlowTracer.GUI.Models.DataStructures.Logging.LogMessages;

using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting.Display;

namespace PlowTracer.GUI.Models.DataStructures.Logging;

internal partial class CollectionSink : ILogEventSink
{
    private readonly MessageTemplateTextFormatter m_textFormatter = new ("{Timestamp:HH:mm:ss} - {Message}{Exception}");

    private static AvaloniaList<IConsoleLogMessage> Events { get; set; } = [];

    public void Emit(LogEvent p_logEvent)
    {
        var renderer = new StringWriter();
        m_textFormatter.Format(p_logEvent, renderer);

        var renderedMessage = renderer.ToString();

        renderedMessage = MyRegex().Replace(renderedMessage, "");
        renderedMessage = renderedMessage.Replace(@"""", "");

        IConsoleLogMessage message = p_logEvent.Properties["Type"].ToString().Trim('"') switch
                      {
                          "ACTIVITY" => new ActivityLogMessage(p_logEvent.Level, renderedMessage),
                          _          => new ActivityLogMessage(p_logEvent.Level, renderedMessage)
                      };

        Events.Insert(0, message);
        
        // Only store the last 100 log events. - Comment by M9 on 07/09/2024 @ 16:25:11
        if ( Events.Count > 100 )
        {
            Events.RemoveAt(Events.Count - 1);
        }
    }

    public static void SetCollection(AvaloniaList<IConsoleLogMessage> p_sink)
    {
        Events = p_sink;
    }

    [GeneratedRegex(@"(\[[A-Za-z]+\])")]
    private static partial Regex MyRegex();
}