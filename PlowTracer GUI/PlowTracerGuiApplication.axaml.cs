using System;

using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using PlowTracer.GUI.Models.DataStructures.Logging;
using PlowTracer.GUI.Models.Global.IO.Files;
using PlowTracer.GUI.ViewModels;
using PlowTracer.GUI.ViewModels.Utilities;
using PlowTracer.Views;

using Serilog;
using Serilog.Events;

namespace PlowTracer.GUI;

internal class PlowTracerGuiApplication : Application
{
    private static IConfigurationRoot Configuration    { get; } = GetConfiguration();
    private static IServiceProvider   ServiceProvider  { get; } = ConfigureServiceProvider();
    public static  ViewModelLocator   ViewModelLocator { get; } = new(ServiceProvider);

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if ( ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop )
        {
            desktop.MainWindow = new MainWindow();
        }

        base.OnFrameworkInitializationCompleted();
    }

    private static IConfigurationRoot GetConfiguration()
    {
        var configurationBuilder = new ConfigurationBuilder();

        var environment = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "Production";

        configurationBuilder.AddJsonFile(environment.Equals("Development") ? "appsettings.Development.json" : "appsettings.json", true, true);

        return configurationBuilder.Build();
    }

    private static ServiceProvider ConfigureServiceProvider()
    {
        var serviceCollection = new ServiceCollection();

        serviceCollection.AddLogging(ConfigureLogging);

        PrepareServices(serviceCollection);

        var serviceProvider = serviceCollection.BuildServiceProvider();

        return serviceProvider;
    }

    private static void PrepareServices(IServiceCollection p_services)
    {
        p_services.AddSingleton<MainWindowViewModel>();
    }

    private static void ConfigureLogging(ILoggingBuilder p_builder)
    {
        p_builder.ClearProviders();

        var loggerConfiguration = new LoggerConfiguration().ReadFrom.Configuration(Configuration)
                                                           .Enrich.FromLogContext()
                                                       #if !DEBUG
                                                       // Exclude Debug logs in Release configurations. - Comment by Matt Heimlich on 10/25/2024 @ 14:57:17
                                                       .Filter.ByExcluding(p_event => p_event.Level == LogEventLevel.Debug)
                                                       #else
                                                           // Otherwise, write all logs to Debug loggers as well. - Comment by Matt Heimlich on 10/25/2024 @ 14:59:26
                                                           .WriteTo.Debug()
                                                           .WriteTo.Sink(new CollectionSink())
                                                           .WriteTo.Logger(p_configuration => p_configuration
                                                                                              .Filter.ByIncludingOnly(p_event =>
                                                                                                                      {
                                                                                                                          p_event.AddPropertyIfAbsent(new LogEventProperty("Type",
                                                                                                                                                                           new
                                                                                                                                                                               ScalarValue("APPLICATION")));
                                                                                                                          return p_event.Level is LogEventLevel.Debug;
                                                                                                                      })
                                                                                              .WriteTo.File(ApplicationFiles.DebugLogFile,
                                                                                                            outputTemplate:
                                                                                                            "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz}] [{Type:l}] - {Message:l}{NewLine}{Exception}",
                                                                                                            rollingInterval: RollingInterval.Day,
                                                                                                            retainedFileCountLimit: 31,
                                                                                                            fileSizeLimitBytes: 1024 * 1024 * 32,
                                                                                                            rollOnFileSizeLimit: true))
                                                       #endif
                                                           .WriteTo.Logger(p_configuration => p_configuration
                                                                                              .Filter.ByIncludingOnly(p_event =>
                                                                                                                      {
                                                                                                                          p_event.AddPropertyIfAbsent(new LogEventProperty("Type",
                                                                                                                                                                           new
                                                                                                                                                                               ScalarValue("ACTIVITY")));
                                                                                                                          return true;
                                                                                                                      })
                                                                                              .WriteTo.File(ApplicationFiles.AggregateLogFile,
                                                                                                            outputTemplate:
                                                                                                            "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz}] [{Type:l}:{Level:u3}] - {Message:l}{NewLine}{Exception}",
                                                                                                            rollingInterval: RollingInterval.Day,
                                                                                                            retainedFileCountLimit: 31,
                                                                                                            fileSizeLimitBytes: 1024 * 1024 * 32,
                                                                                                            rollOnFileSizeLimit: true))
                                                           .WriteTo.Logger(p_configuration => p_configuration
                                                                                              .Filter.ByIncludingOnly(p_event => p_event.Properties.ContainsKey("Type") &&
                                                                                                                                 p_event.Properties["Type"].ToString().Trim('"')
                                                                                                                                        .Equals("ACTIVITY", StringComparison.CurrentCultureIgnoreCase))
                                                                                              .WriteTo.File(ApplicationFiles.ActivityLogFile,
                                                                                                            outputTemplate:
                                                                                                            "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz}] [{Level:u3}] - {Message:l}{NewLine}{Exception}",
                                                                                                            rollingInterval: RollingInterval.Day,
                                                                                                            retainedFileCountLimit: 31,
                                                                                                            fileSizeLimitBytes: 1024 * 1024 * 32,
                                                                                                            rollOnFileSizeLimit: true))
                                                           .WriteTo.Logger(p_configuration => p_configuration
                                                                                              .Filter.ByIncludingOnly(p_event =>
                                                                                                                      {
                                                                                                                          p_event.AddPropertyIfAbsent(new LogEventProperty("Type",
                                                                                                                                                                           new
                                                                                                                                                                               ScalarValue("APPLICATION")));
                                                                                                                          return p_event.Level is LogEventLevel.Error or
                                                                                                                                                  LogEventLevel.Fatal;
                                                                                                                      })
                                                                                              .WriteTo.File(ApplicationFiles.ErrorLogFile,
                                                                                                            outputTemplate:
                                                                                                            "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz}] [{Type:l}:{Level:u3}] - {Message:l}{NewLine}{Exception}",
                                                                                                            rollingInterval: RollingInterval.Day,
                                                                                                            retainedFileCountLimit: 31,
                                                                                                            fileSizeLimitBytes: 1024 * 1024 * 32,
                                                                                                            rollOnFileSizeLimit: true));

        if ( OperatingSystem.IsWindows() )
        {
            loggerConfiguration = loggerConfiguration.WriteTo.EventLog("PlowTracer", manageEventSource: true);
        }

        Log.Logger = loggerConfiguration.CreateLogger();

        p_builder.AddSerilog(Log.Logger);
    }
}