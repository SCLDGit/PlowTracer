using System;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

using Avalonia;
using Avalonia.Collections;
using Avalonia.Media.Imaging;
using Avalonia.Metadata;
using Avalonia.Platform;

using Microsoft.Extensions.Logging;

using PlowTracer.Core.Core.Kernels;
using PlowTracer.Core.Core.Tracers;
using PlowTracer.Core.DataStructures.Render.Settings;
using PlowTracer.GUI.Models.DataStructures.Logging;
using PlowTracer.GUI.Models.DataStructures.Logging.LogMessages;
using PlowTracer.GUI.Models.Enumerations.Logging;
using PlowTracer.GUI.Models.Extensions.Logging;

using ReactiveUI;
using ReactiveUI.Fody.Helpers;

using Vector = Avalonia.Vector;

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

    [Reactive] public bool RenderIsRunning { get; set; }

    public AvaloniaList<IRenderKernel> RenderKernels { get; } =
        [
            new ColorOutputTestKernel(),
            new RayTestKernel(),
            new SphereTestKernel(),
            new SurfaceNormalTestKernel(),
            new CameraTestKernel(),
            new MultisampleTestKernel(),
            new MaterialTestKernel(),
            new BucketRenderKernel()
        ];

    [Reactive] public IRenderKernel SelectedRenderKernel { get; set; }
    
    [Reactive] public int RenderWidth   { get; set; } = 1366;
    [Reactive] public int RenderHeight  { get; set; } = 768;
    
    [Reactive] public int RenderSamples { get; set; } = 1;

    [Reactive] public int MaxLightBounces { get; set; } = 15;
    
    [Reactive] public float CameraXPosition   { get; set; }
    [Reactive] public float CameraYPosition   { get; set; }
    [Reactive] public float CameraZPosition   { get; set; }
    
    [Reactive] public float CameraTargetXPosition { get; set; }
    [Reactive] public float CameraTargetYPosition { get; set; }
    [Reactive] public float CameraTargetZPosition { get; set; } = -1.0f;
    
    [Reactive] public float CameraUpX { get; set; }
    [Reactive] public float CameraUpY { get; set; } = 1.0f;
    [Reactive] public float CameraUpZ { get; set; }
    
    [Reactive] public float CameraFieldOfView { get; set; } = 90.0f;
    [Reactive] public float CameraFocalLength { get; set; } = 1.0f;
    
    [Reactive] public required Bitmap OutputImage       { get; set; }
    
    public Action? ResetRenderPanAndZoom { get; set; }
    public Action? RefreshRenderTarget  { get; set; }

    private void ResetOutputImage()
    {
        OutputImage = new WriteableBitmap(new PixelSize(RenderWidth, RenderHeight), new Vector(96, 96), PixelFormat.Rgba8888, AlphaFormat.Premul);
    }

    [DependsOn(nameof(RenderIsRunning))]
    // ReSharper disable once UnusedMember.Global
    public bool CanClickRender(object? p_parameter)
    {
        return !RenderIsRunning;
    }
    
    public async Task ClickRender()
    {
        m_logger.LogDebug(LogMessageType.ACTIVITY, "Clicked Render");
        
        ResetRenderPanAndZoom?.Invoke();
        
        var stopwatch = Stopwatch.StartNew();
        
        ResetOutputImage();
        
        RenderIsRunning = true;
        
        try
        {
            if ( OutputImage is not WriteableBitmap outputImage ) return;

            await foreach ( var resultUpdate in SelectedRenderKernel.RenderAsync(new RenderSettings(RenderWidth, RenderHeight, RenderSamples, MaxLightBounces, 32,
                                                                                                    new Vector3(CameraXPosition, CameraYPosition, CameraZPosition),
                                                                                                    new Vector3(CameraTargetXPosition, CameraTargetYPosition, CameraTargetZPosition),
                                                                                                    new Vector3(CameraUpX, CameraUpY, CameraUpZ), CameraFieldOfView, CameraFocalLength,
                                                                                                    new MaterialTracer(MaxLightBounces))) )
            {
                using ( var lockedBitmap = outputImage.Lock() )
                {
                    Marshal.Copy(resultUpdate.Data, 0, new IntPtr(lockedBitmap.Address.ToInt64()), resultUpdate.DataSize);
                }

                RefreshRenderTarget?.Invoke();
            }

            stopwatch.Stop();

            m_logger.LogInformation(LogMessageType.ACTIVITY, $"Rendered in {stopwatch.ElapsedMilliseconds}ms");
        }
        finally
        {
            RenderIsRunning = false;
        }
    }
}