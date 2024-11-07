using System;
using System.IO;

namespace PlowTracer.GUI.Models.Global.IO.Directories;

internal static class ApplicationDirectories
{
    #if DEBUG
    private static string ApplicationDataDirectory =>
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "PlowTracer", "GUI", "Debug");
    #else
    private static string ApplicationDataDirectory =>
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "PlowTracer", "Desktop Client");
    #endif

    internal static string LogsDirectory => Path.Combine(ApplicationDataDirectory, "Logs");
}