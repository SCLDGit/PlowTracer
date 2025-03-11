using System;
using System.Numerics;
using System.Threading.Tasks;

using PlowTracer.Core.Core.Tracers;
using PlowTracer.Core.DataStructures.Render.Primitives.Camera;
using PlowTracer.Core.DataStructures.Render.Primitives.Intersection.IntersectableEntities;
using PlowTracer.Core.DataStructures.Render.Primitives.Intersection.IntersectableEntities.Scenes;
using PlowTracer.Core.DataStructures.Render.Primitives.Intersection.IntersectableEntities.Shapes;
using PlowTracer.Core.DataStructures.Render.Primitives.Intersection.Materials;
using PlowTracer.Core.DataStructures.Render.Result;
using PlowTracer.Core.DataStructures.Render.Settings;
using PlowTracer.Core.DataStructures.Utilities;

namespace PlowTracer.Core.Core.Kernels;

public class MaterialTestKernel : IRenderKernel
{
    public async Task<RenderResult> RenderAsync(RenderSettings p_settings)
    {
        var tracer = new MaterialTracer(p_settings.MaxBounces);

        var renderResult = new RenderResult(p_settings.Width, p_settings.Height);

        var camera = new ThinLensCamera(p_settings);

        var scene = StaticScenes.ThreeSpheresScene();
        
        const byte alpha  = 0xFF;

        var resultDataIndex = 0; // Image format is a flat array, so start at 0 index and count up for each inserted value. - Comment by Matt Heimlich on 11/14/2024 @ 16:55:27

        var sampleScale = 1.0f / p_settings.Samples;
        
        for ( var row = 0; row < p_settings.Height; ++row )
        {
            for ( var column = 0; column < p_settings.Width; ++column )
            {
                var pixelColor = Vector3.Zero;
                
                for ( var sample = 0; sample < p_settings.Samples; ++sample )
                {
                    var ray = camera.GetRay(column, row, p_settings.Samples > 1);
                    pixelColor += tracer.GetPixelColor(ref ray, scene);
                }

                pixelColor *= sampleScale;

                var correctedPixelColor = ColorUtilities.LinearToSrgb(pixelColor);

                var formattedRed   = (int)MathF.Round(255 * Math.Clamp(correctedPixelColor.X, 0.0f, 0.999f), MidpointRounding.AwayFromZero);
                var formattedGreen = (int)MathF.Round(255 * Math.Clamp(correctedPixelColor.Y, 0.0f, 0.999f), MidpointRounding.AwayFromZero);
                var formattedBlue  = (int)MathF.Round(255 * Math.Clamp(correctedPixelColor.Z, 0.0f, 0.999f), MidpointRounding.AwayFromZero);

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
        return "Material Test"; 
    }
}