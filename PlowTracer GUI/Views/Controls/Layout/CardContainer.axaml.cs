using Avalonia;
using Avalonia.Controls;

namespace PlowTracer.GUI.Views.Controls.Layout;

public partial class CardContainer : UserControl
{
    public CardContainer()
    {
        InitializeComponent();

        ShowShadow = true;
    }
    
    public static readonly StyledProperty<object?> HeaderContentProperty = AvaloniaProperty.Register<CardContainer, object?>(
                                                                                         "HeaderContent");
    public object? HeaderContent
    {
        get => GetValue(HeaderContentProperty);
        set => SetValue(HeaderContentProperty, value);
    }

    public static readonly StyledProperty<bool> ShowShadowProperty = AvaloniaProperty.Register<CardContainer, bool>(
                                                                                         "ShowShadow");
    public bool ShowShadow
    {
        get => GetValue(ShowShadowProperty);
        set => SetValue(ShowShadowProperty, value);
    }
}