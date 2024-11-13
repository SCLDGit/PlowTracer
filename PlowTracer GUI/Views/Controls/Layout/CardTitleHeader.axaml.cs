using System.Windows.Input;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;

using Material.Icons;

namespace PlowTracer.GUI.Views.Controls.Layout;

public partial class CardTitleHeader : UserControl
{
    public CardTitleHeader()
    {
        InitializeComponent();
    }

    public static readonly StyledProperty<MaterialIconKind> IconKindProperty = AvaloniaProperty.Register<CardTitleHeader, MaterialIconKind>(
                                                                                         "IconKind");
    public MaterialIconKind IconKind
    {
        get => GetValue(IconKindProperty);
        set => SetValue(IconKindProperty, value);
    }

    public static readonly StyledProperty<string> TitleProperty = AvaloniaProperty.Register<CardTitleHeader, string>(
                                                                                         "Title");
    public string Title
    {
        get => GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }
    
    public static readonly StyledProperty<bool> ShowFunctionButtonProperty = AvaloniaProperty.Register<CardTitleHeader, bool>(
                                                                                         "ShowFunctionButton");
    public bool ShowFunctionButton
    {
        get => GetValue(ShowFunctionButtonProperty);
        set => SetValue(ShowFunctionButtonProperty, value);
    }

    public static readonly StyledProperty<ICommand> FunctionButtonCommandProperty = AvaloniaProperty.Register<CardTitleHeader, ICommand>(
                                                                                         "FunctionButtonCommand");
    public ICommand FunctionButtonCommand
    {
        get => GetValue(FunctionButtonCommandProperty);
        set => SetValue(FunctionButtonCommandProperty, value);
    }

    public static readonly StyledProperty<FlyoutBase> FunctionButtonFlyoutProperty = AvaloniaProperty.Register<CardTitleHeader, FlyoutBase>(
                                                                                         "FunctionButtonFlyout");
    public FlyoutBase FunctionButtonFlyout
    {
        get => GetValue(FunctionButtonFlyoutProperty);
        set => SetValue(FunctionButtonFlyoutProperty, value);
    }
}