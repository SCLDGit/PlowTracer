using Avalonia;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace PlowTracer.GUI.Models.Converters;

public static class BoxShadowConverters
{
    public static readonly IValueConverter CardThicknessConverter = new FuncValueConverter<bool, Thickness>(p_in => p_in ? Thickness.Parse("4") : Thickness.Parse("0"));
    public static readonly IValueConverter BoxShadowConverter = new FuncValueConverter<bool, BoxShadows>(p_in => p_in ? BoxShadows.Parse("0 0 4 2 #80000000") : BoxShadows.Parse("0 0 0 0 #FF000000"));
}