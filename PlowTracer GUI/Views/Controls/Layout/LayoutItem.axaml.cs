using Avalonia;
using Avalonia.Controls;

namespace PlowTracer.GUI.Views.Controls.Layout;

public partial class LayoutItem : UserControl
{
    public LayoutItem()
    {
        InitializeComponent();
    }

    public static readonly StyledProperty<object?> LabelProperty = AvaloniaProperty.Register<LayoutItem, object?>("Label");

    public object? Label
    {
        get => GetValue(LabelProperty);
        set => SetValue(LabelProperty, value);
    }
}