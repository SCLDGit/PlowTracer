using System.Linq;

using Avalonia.Data.Converters;

namespace PlowTracer.GUI.Models.Converters;

internal static class PanAndZoomConverters
{
    public static readonly IMultiValueConverter ZoomToOffset = new FuncMultiValueConverter<double, double>(p_values =>
                                                                                                         {
                                                                                                             var values = p_values.ToArray();

                                                                                                             return values[0] - values[0] * values[1];
                                                                                                         });
}