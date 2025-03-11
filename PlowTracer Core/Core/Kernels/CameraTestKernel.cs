using System;
using System.Numerics;
using System.Threading.Tasks;

using PlowTracer.Core.Core.Tracers;
using PlowTracer.Core.DataStructures.Render.Primitives.Camera;
using PlowTracer.Core.DataStructures.Render.Primitives.Intersection.IntersectableEntities;
using PlowTracer.Core.DataStructures.Render.Primitives.Intersection.IntersectableEntities.Shapes;
using PlowTracer.Core.DataStructures.Render.Primitives.Intersection.Materials;
using PlowTracer.Core.DataStructures.Render.Result;
using PlowTracer.Core.DataStructures.Render.Settings;

namespace PlowTracer.Core.Core.Kernels;

public class CameraTestKernel : IRenderKernel
{
    public async Task<RenderResult> RenderAsync(RenderSettings p_settings)
    {
        var scene = new Scene([
                                  new Sphere(new Vector3(0.0f, 0.0f, -1.0f), 0.5f, new LambertianDiffuse(Vector3.Zero)),
                                  new Sphere(new Vector3(0.0f, -100.5f, -1.0f), 100.0f, new LambertianDiffuse(Vector3.Zero))
                              ]);

        var tracer = new NormalsTracer();

        var renderResult = new RenderResult(p_settings.Width, p_settings.Height);

        var camera = new ThinLensCamera(p_settings);
        
        const byte alpha  = 0xFF;

        var resultDataIndex = 0; // Image format is a flat array, so start at 0 index and count up for each inserted value. - Comment by Matt Heimlich on 11/14/2024 @ 16:55:27

        for ( var row = 0; row < p_settings.Height; ++row )
        {
            for ( var column = 0; column < p_settings.Width; ++column )
            {
                var ray = camera.GetRay(column, row);
                var pixelColor = tracer.GetPixelColor(ref ray, scene);

                var formattedRed   = (int)MathF.Round(255 * Math.Clamp(pixelColor.X, 0.0f, 0.999f), MidpointRounding.AwayFromZero);
                var formattedGreen = (int)MathF.Round(255 * Math.Clamp(pixelColor.Y, 0.0f, 0.999f), MidpointRounding.AwayFromZero);
                var formattedBlue  = (int)MathF.Round(255 * Math.Clamp(pixelColor.Z, 0.0f, 0.999f), MidpointRounding.AwayFromZero);

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