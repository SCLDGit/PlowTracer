using Avalonia;
using Avalonia.Controls;

using Material.Icons;

namespace PlowTracer.GUI.Views.Controls.Layout;

public partial class IconLabel : UserControl
{
    public IconLabel()
    {
        InitializeComponent();
    }

    public static readonly StyledProperty<MaterialIconKind> IconKindProperty = AvaloniaProperty.Register<IconLabel, MaterialIconKind>(
                                                                                         "IconKind");
    public MaterialIconKind IconKind
    {
        get => GetValue(IconKindProperty);
        set => SetValue(IconKindProperty, value);
    }

    public static readonly StyledProperty<string> TextProperty = AvaloniaProperty.Register<IconLabel, string>(
                                                                                         "Text");
    public string Text
    {
        get => GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }
}