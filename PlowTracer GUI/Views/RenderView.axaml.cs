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
        var panAndZoomControl = this.GetControl<ZoomBorder>("RenderPanAndZoom");
        
        panAndZoomControl.ResetMatrix();
    }

    private void OnAttached(object? p_sender, VisualTreeAttachmentEventArgs _)
    {
        if ( DataContext is not MainWindowViewModel mainWindowViewModel ) return;

        mainWindowViewModel.ResetRenderPanAndZoom = ResetRenderView;
    }
}