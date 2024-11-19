using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

using PlowTracer.Core.DataStructures.Render.Primitives.Intersection;
using PlowTracer.Core.DataStructures.Render.Primitives.Intersection.IntersectableEntities;
using PlowTracer.Core.DataStructures.Render.Primitives.Intersection.IntersectableEntities.Shapes;
using PlowTracer.Core.DataStructures.Render.Result;
using PlowTracer.Core.DataStructures.Render.Settings;

namespace PlowTracer.Core.Core.Kernels;

public class SurfaceNormalTestKernel : IRenderKernel
{
    public async Task<RenderResult> Render(RenderSettings p_settings)
    {
        var aspectRatio = (float)p_settings.Width / p_settings.Height;

        const float focalLength = 1.0f;

        const float viewportHeight = 2.0f;

        var viewportWidth = viewportHeight * aspectRatio;

        var cameraCenter = Vector3.Zero;

        var viewportU = new Vector3(viewportWidth, 0, 0);
        var viewportV = new Vector3(0, -viewportHeight, 0);

        var pixelDeltaU = viewportU / p_settings.Width;
        var pixelDeltaV = viewportV / p_settings.Height;

        var viewportUpperLeft = cameraCenter      - new Vector3(0, 0, focalLength) - viewportU / 2.0f - viewportV / 2.0f;
        var upperLeftPixel  = viewportUpperLeft + 0.5f                                       * ( pixelDeltaU + pixelDeltaV );

        var scene = new Scene([
                                  new Sphere(new Vector3(0.0f, 0.0f, -1.0f), 0.5f),
                                  new Sphere(new Vector3(0.0f, -100.5f, -1.0f), 100.0f)
                              ]);

        var renderResult = new RenderResult(p_settings.Width, p_settings.Height);

        const byte alpha = 0xFF;

        var resultDataIndex = 0; // Image format is a flat array, so start at 0 index and count up for each inserted value. - Comment by Matt Heimlich on 11/14/2024 @ 16:55:27

        for ( var row = 0; row < p_settings.Height; ++row )
        {
            for ( var column = 0; column < p_settings.Width; ++column )
            {
                var pixelCenter  = upperLeftPixel + column * pixelDeltaU + row * pixelDeltaV;
                var rayDirection = pixelCenter      - cameraCenter;

                var ray = new Ray(cameraCenter, rayDirection);

                var pixelColor = GetPixelColor(ref ray, scene);

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

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static Vector3 GetPixelColor(ref Ray p_ray, Scene p_scene)
    {
        var intersection = p_scene.GetIntersection(ref p_ray);
        
        if ( intersection.Intersected )
        {
            return 0.5f * ( intersection.Normal + Vector3.One );
        }

        var unitDirection = Vector3.Normalize(p_ray.Direction);
        var a             = 0.5f * ( unitDirection.Y + 1.0f );
        return ( 1.0f - a ) * Vector3.One + a * new Vector3(0.5f, 0.7f, 1.0f);
    }

    public override string ToString()
    {
        return "Surface Normal Test";
    }
}