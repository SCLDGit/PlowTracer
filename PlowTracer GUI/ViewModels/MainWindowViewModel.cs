using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

using Avalonia;
using Avalonia.Collections;
using Avalonia.Media.Imaging;
using Avalonia.Platform;

using Microsoft.Extensions.Logging;

using PlowTracer.Core.Core.Kernels;
using PlowTracer.Core.DataStructures.Render.Settings;
using PlowTracer.GUI.Models.DataStructures.Logging;
using PlowTracer.GUI.Models.DataStructures.Logging.LogMessages;
using PlowTracer.GUI.Models.Enumerations.Logging;
using PlowTracer.GUI.Models.Extensions.Logging;

using ReactiveUI.Fody.Helpers;

namespace PlowTracer.GUI.ViewModels;

internal class MainWindowViewModel : ViewModelBase
{
    private readonly ILogger<MainWindowViewModel> m_logger;
    
    public MainWindowViewModel(ILogger<MainWindowViewModel> p_logger)
    {
        ResetOutputImage();
        
        SelectedRenderKernel = RenderKernels.First();
        
        CollectionSink.SetCollection(LogMessages);
        
        m_logger = p_logger;

        m_logger.LogDebug(LogMessageType.ACTIVITY, "This is a debug message");
        m_logger.LogInformation(LogMessageType.ACTIVITY, "This is an information message");
        m_logger.LogWarning(LogMessageType.ACTIVITY, "This is a warning message");
        m_logger.LogError(LogMessageType.ACTIVITY, "This is an error message");
        m_logger.LogCritical(LogMessageType.ACTIVITY, "This is a critical message");
        
        m_logger.LogDebug(LogMessageType.ACTIVITY, "Creating MainWindowViewModel");
    }
    
    public AvaloniaList<IConsoleLogMessage> LogMessages { get; } = [];

    public AvaloniaList<IRenderKernel> RenderKernels { get; } = [new ColorOutputTestKernel()];

    [Reactive] public IRenderKernel SelectedRenderKernel { get; set; }
    
    [Reactive] public int RenderWidth  { get; set; } = 1366;
    [Reactive] public int RenderHeight { get; set; } = 768;
    
    [Reactive] public required Bitmap OutputImage       { get; set; }
    
    public Action? ResetRenderPanAndZoom { get; set; }

    private void ResetOutputImage()
    {
        OutputImage = new WriteableBitmap(new PixelSize(RenderWidth, RenderHeight), new Vector(96, 96), PixelFormat.Rgba8888, AlphaFormat.Premul);
    }

    public bool CanClickRender(object? p_parameter)
    {
        return true;
    }
    
    public async Task ClickRender()
    {
        m_logger.LogDebug(LogMessageType.ACTIVITY, "Clicked Render");
        
        ResetRenderPanAndZoom?.Invoke();
        
        var stopwatch = Stopwatch.StartNew();
        
        var result = await SelectedRenderKernel.Render(new RenderSettings(RenderWidth, RenderHeight));

        stopwatch.Stop();
        
        ResetOutputImage();
        
        m_logger.LogDebug(LogMessageType.ACTIVITY, $"Rendered in {stopwatch.ElapsedMilliseconds}ms");
        
        if ( OutputImage is not WriteableBitmap outputImage ) return;
        
        using var lockedBitmap = outputImage.Lock();
        
        Marshal.Copy(result.Data, 0, new IntPtr(lockedBitmap.Address.ToInt64()), result.Data.Length);
    }
}