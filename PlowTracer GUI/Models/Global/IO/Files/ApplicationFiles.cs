using System.IO;

using PlowTracer.GUI.Models.Global.IO.Directories;

namespace PlowTracer.GUI.Models.Global.IO.Files;

internal static class ApplicationFiles
{
    internal static string AggregateLogFile => Path.Combine(ApplicationDirectories.LogsDirectory, "Aggregate Logs", "aggregate.log");
    internal static string ActivityLogFile  => Path.Combine(ApplicationDirectories.LogsDirectory, "Activity Logs", "activity.log");
    internal static string ErrorLogFile     => Path.Combine(ApplicationDirectories.LogsDirectory, "Error Logs", "error.log");
    internal static string DebugLogFile     => Path.Combine(ApplicationDirectories.LogsDirectory, "Debug Logs", "debug.log");
}