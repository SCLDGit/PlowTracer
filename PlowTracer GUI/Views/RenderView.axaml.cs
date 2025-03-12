using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.PanAndZoom;

using Microsoft.Extensions.Logging;

using PlowTracer.GUI.ViewModels;

namespace PlowTracer.GUI.Views;

public partial class RenderView : UserControl
{
    public RenderView()
    {
        InitializeComponent();
    }
    

    public void ResetRenderView()
    {
        RenderPanAndZoom.ResetMatrix();
    }
    
    public void RefreshRenderTarget()
    {
        RenderTarget.InvalidateVisual();
    }

    private void OnAttached(object? p_sender, VisualTreeAttachmentEventArgs _)
    {
        if ( DataContext is not MainWindowViewModel mainWindowViewModel ) return;

        mainWindowViewModel.ResetRenderPanAndZoom = ResetRenderView;
        mainWindowViewModel.RefreshRenderTarget  = RefreshRenderTarget;
    }
}