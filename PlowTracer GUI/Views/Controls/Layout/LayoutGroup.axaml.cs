using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;

namespace PlowTracer.GUI.Views.Controls.Layout;

public partial class LayoutGroup : ItemsControl
{
    public LayoutGroup()
    {
        InitializeComponent();

        Orientation = Orientation.Vertical;
        ItemsSpacing = 4;
    }

    public static readonly StyledProperty<Orientation> OrientationProperty = AvaloniaProperty.Register<LayoutGroup, Orientation>(
                                                                                         "Orientation");
    public Orientation Orientation
    {
        get => GetValue(OrientationProperty);
        set => SetValue(OrientationProperty, value);
    }
    
    public static readonly StyledProperty<double> ItemsSpacingProperty = AvaloniaProperty.Register<LayoutGroup, double>(
                                                                                         "ItemsSpacing");
    public double ItemsSpacing
    {
        get => GetValue(ItemsSpacingProperty);
        set => SetValue(ItemsSpacingProperty, value);
    }
}