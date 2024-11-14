﻿using System;
using System.Numerics;
using System.Threading.Tasks;

using PlowTracer.Core.DataStructures.Render.Primitives;
using PlowTracer.Core.DataStructures.Render.Result;
using PlowTracer.Core.DataStructures.Render.Settings;

namespace PlowTracer.Core.Core.Kernels;

public class SurfaceNormalTestKernel : IRenderKernel
{
    public async Task<RenderResult> Render(RenderSettings p_settings)
    {
        var aspectRatio = (float) p_settings.Width / p_settings.Height;

        const float focalLength = 1.0f;
        
        const float viewportHeight = 2.0f;
        
        var         viewportWidth  = viewportHeight * aspectRatio;

        var cameraCenter = Vector3.Zero;

        var viewportU = new Vector3(viewportWidth, 0, 0);
        var viewportV = new Vector3(0, -viewportHeight, 0);
        
        var pixelDeltaU = viewportU / p_settings.Width;
        var pixelDeltaV = viewportV / p_settings.Height;
        
        var viewportUpperLeft = cameraCenter - new Vector3(0, 0, focalLength) - viewportU / 2.0f - viewportV / 2.0f;
        var pixel100Location = viewportUpperLeft + 0.5f * (pixelDeltaU + pixelDeltaV);
        
        var renderResult = new RenderResult(p_settings.Width, p_settings.Height);
        
        const byte alpha = 0xFF;
        
        var  index = 0; // Start at beginning of array
        
        for ( var row = 0; row < p_settings.Height; ++row )
        {
            for (var column = 0; column < p_settings.Width; ++column)
            {
                var pixelCenter = pixel100Location + column * pixelDeltaU + row * pixelDeltaV;
                var rayDirection = pixelCenter - cameraCenter;
                
                var ray = new Ray(cameraCenter, rayDirection);

                var pixelColor = GetPixelColor(ray);

                var formattedRed   = (int)MathF.Round(255 * pixelColor.X, MidpointRounding.AwayFromZero);
                var formattedGreen = (int)MathF.Round(255 * pixelColor.Y, MidpointRounding.AwayFromZero);
                var formattedBlue  = (int)MathF.Round(255 * pixelColor.Z, MidpointRounding.AwayFromZero);
                
                renderResult.Data[index++] = (byte)formattedRed;
                renderResult.Data[index++] = (byte)formattedGreen;
                renderResult.Data[index++] = (byte)formattedBlue;
                renderResult.Data[index++] = alpha;
            }
        }
        
        return await Task.FromResult(renderResult);
    }

    private Vector3 GetPixelColor(Ray p_ray)
    {
        var intersection = GetSphereIntersection(new Vector3(0.0f, 0.0f, -1.0f), 0.5f, p_ray);
        if ( intersection is not null )
        {
            var intersectionNormal = Vector3.Normalize(p_ray.GetPointAt(intersection.Distance) - new Vector3(0.0f, 0.0f, -1.0f));
            return 0.5f * new Vector3(intersectionNormal.X + 1.0f, intersectionNormal.Y + 1.0f, intersectionNormal.Z + 1.0f);
        }
        
        var unitDirection = Vector3.Normalize(p_ray.Direction);
        var a             = 0.5f * ( unitDirection.Y + 1.0f );
        return ( 1.0f - a ) * Vector3.One + a * new Vector3(0.5f, 0.7f, 1.0f);
    }

    private IntersectionRecord? GetSphereIntersection(Vector3 p_center, float p_radius, Ray p_ray)
    {
        var oc = p_center - p_ray.Origin;
        var a = Vector3.Dot(p_ray.Direction, p_ray.Direction);
        var b = -2.0f * Vector3.Dot(p_ray.Direction, oc);
        var c = Vector3.Dot(oc, oc) - p_radius * p_radius;
        var discriminant = b * b - 4 * a * c;

        return discriminant < 0 ? null : new IntersectionRecord((-b - MathF.Sqrt(discriminant)) / (2.0f * a));
    }

    public override string ToString()
    {
        return "Surface Normal Test";
    }
}