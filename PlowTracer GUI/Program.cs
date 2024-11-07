using System;

using Avalonia;
using Avalonia.ReactiveUI;

namespace PlowTracer.GUI;

sealed class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] p_args)
    {
        #if DEBUG
        Environment.SetEnvironmentVariable("DOTNET_ENVIRONMENT", "Development");
        #else
        Environment.SetEnvironmentVariable("DOTNET_ENVIRONMENT", "Production");
        #endif
        
        BuildAvaloniaApp()
            .StartWithClassicDesktopLifetime(p_args);
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<PlowTracerGuiApplication>()
                     .UsePlatformDetect()
                     .WithInterFont()
                     .LogToTrace()
                     .UseReactiveUI();
}