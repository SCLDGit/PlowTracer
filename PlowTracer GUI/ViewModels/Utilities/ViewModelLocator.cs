using System;

using Microsoft.Extensions.DependencyInjection;

namespace PlowTracer.GUI.ViewModels.Utilities;

internal class ViewModelLocator(IServiceProvider c_serviceProvider)
{
    internal MainWindowViewModel MainWindowViewModel { get; } = c_serviceProvider.GetRequiredService<MainWindowViewModel>();
}