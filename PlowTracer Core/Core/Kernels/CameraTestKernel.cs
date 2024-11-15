using System;
using System.Numerics;
using System.Threading.Tasks;

using PlowTracer.Core.DataStructures.Render.Primitives.Camera;
using PlowTracer.Core.DataStructures.Render.Primitives.IntersectableEntities;
using PlowTracer.Core.DataStructures.Render.Primitives.IntersectableEntities.Shapes;
using PlowTracer.Core.DataStructures.Render.Result;
using PlowTracer.Core.DataStructures.Render.Settings;

namespace PlowTracer.Core.Core.Kernels;

public class CameraTestKernel : IRenderKernel
{
    public async Task<RenderResult> Render(RenderSettings p_settings)
    {
        var scene = new Scene([
                                  new Sphere(new Vector3(0.0f, 0.0f, -1.0f), 0.5f),
                                  new Sphere(new Vector3(0.0f, -100.5f, -1.0f), 100.0f)
                              ]);

        var renderResult = new RenderResult(p_settings.Width, p_settings.Height);

        var camera = new ThinLensCamera(p_settings, scene);
        
        const byte alpha  = 0xFF;

        var resultDataIndex = 0; // Image format is a flat array, so start at 0 index and count up for each inserted value. - Comment by Matt Heimlich on 11/14/2024 @ 16:55:27

        for ( var row = 0; row < p_settings.Height; ++row )
        {
            for ( var column = 0; column < p_settings.Width; ++column )
            {
                var ray = camera.GetRay(column, row);
                var pixelColor = camera.GetPixelColor(ref ray);

                var formattedRed   = (int)MathF.Round(255 * pixelColor.X, MidpointRounding.AwayFromZero);
                var formattedGreen = (int)MathF.Round(255 * pixelColor.Y, MidpointRounding.AwayFromZero);
                var formattedBlue  = (int)MathF.Round(255 * pixelColor.Z, MidpointRounding.AwayFromZero);

                renderResult.Data[resultDataIndex++] = (byte)formattedRed;
                renderResult.Data[resultDataIndex++] = (byte)formattedGreen;
                renderResult.Data[resultDataIndex++] = (byte)formattedBlue;
                renderResult.Data[resultDataIndex++] = alpha;
            }
        }

        return await Task.FromResult(renderResult);
    }

    public override string ToString()
    {
        return "Camera Test";
    }
}