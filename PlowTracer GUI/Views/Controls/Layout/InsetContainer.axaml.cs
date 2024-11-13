using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace PlowTracer.GUI.Views.Controls.Layout;

public partial class InsetContainer : UserControl
{
    public InsetContainer()
    {
        InitializeComponent();
    }

    public static readonly StyledProperty<Thickness> ShadowDepthProperty = AvaloniaProperty.Register<InsetContainer, Thickness>(nameof(ShadowDepth));

    public Thickness ShadowDepth
    {
        get => GetValue(ShadowDepthProperty);
        set => SetValue(ShadowDepthProperty, value);
    }
}