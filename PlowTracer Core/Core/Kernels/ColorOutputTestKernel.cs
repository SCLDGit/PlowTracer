using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

using PlowTracer.Core.DataStructures.Render.Result;
using PlowTracer.Core.DataStructures.Render.Settings;

[assembly:InternalsVisibleTo("PlowTracer.Core.Tests")]

namespace PlowTracer.Core.Core.Kernels;

public class ColorOutputTestKernel : IRenderKernel
{
    public async IAsyncEnumerable<RenderResult> RenderAsync(RenderSettings p_settings)
    {
        var renderResult = new RenderResult(p_settings.Width, p_settings.Height);

        const byte blue  = 0x00;
        const byte alpha = 0xFF;
        
        var  index = 0; // Start at beginning of array
        
        for ( var row = 0; row < p_settings.Height; ++row )
        {
            for (var column = 0; column < p_settings.Width; ++column)
            {
                var red   = (float)column / (p_settings.Width  - 1);
                var green = (float)row    / (p_settings.Height - 1);

                var formattedRed   = (int)MathF.Round(255 * red, MidpointRounding.AwayFromZero);
                var formattedGreen = (int)MathF.Round(255 * green, MidpointRounding.AwayFromZero);

                renderResult.Data[index++] = (byte)formattedRed;
                renderResult.Data[index++] = (byte)formattedGreen;
                renderResult.Data[index++] = blue;
                renderResult.Data[index++] = alpha;
            }
        }
        
        yield return renderResult;
        
        await Task.CompletedTask;
    }

    public override string ToString()
    {
        return "Color Output Test";
    }
}