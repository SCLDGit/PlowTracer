using System;
using System.Numerics;
using System.Runtime.CompilerServices;

using PlowTracer.Core.DataStructures.Render.Primitives.IntersectableEntities;
using PlowTracer.Core.DataStructures.Render.Settings;
using PlowTracer.Core.DataStructures.Utilities;

namespace PlowTracer.Core.DataStructures.Render.Primitives.Camera;

internal readonly record struct ThinLensCamera : ICamera
{
    internal ThinLensCamera(RenderSettings p_renderSettings, Scene p_scene)
    {
        Scene = p_scene;
        
        Origin = p_renderSettings.CameraOrigin;
        
        var aspectRatio = p_renderSettings.Width / (float)p_renderSettings.Height;
        var fieldOfView = p_renderSettings.CameraFieldOfView;
        var focalLength = p_renderSettings.CameraFocalLength;
        
        var theta = MathUtilities.DegreesToRadians(fieldOfView);
        var h     = MathF.Tan(theta / 2);

        var viewportHeight = 2              * h * focalLength;
        var viewportWidth  = viewportHeight * aspectRatio;
        
        var viewportU = new Vector3(viewportWidth, 0, 0);
        var viewportV = new Vector3(0, -viewportHeight, 0);
        
        PixelOffsetU = viewportU / p_renderSettings.Width;
        PixelOffsetV = viewportV / p_renderSettings.Height;
        
        var viewportOrigin = Origin - new Vector3(0, 0, focalLength) - viewportU / 2 - viewportV / 2;
        PixelOrigin = viewportOrigin + 0.5f * (PixelOffsetU + PixelOffsetV);
    }
    
    public Scene   Scene  { get; }

    public Vector3 Origin       { get; }
    public Vector3 PixelOffsetU { get; }
    public Vector3 PixelOffsetV { get; }
    public Vector3 PixelOrigin  { get; }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Ray GetRay(int p_x, int p_y, bool p_multiSampled = true)
    {
        var offset      = p_multiSampled ? MathUtilities.GetRandomPointInUnitSquare() : Vector2.Zero;
        var pixelTarget = PixelOrigin + (p_x + offset.X) * PixelOffsetU + (p_y + offset.Y) * PixelOffsetV;

        return new Ray(Origin, pixelTarget - Origin);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector3 GetPixelColor(ref Ray p_ray)
    {
        var intersection = Scene.GetIntersection(ref p_ray);
        
        if ( intersection.Intersected )
        {
            return 0.5f * ( intersection.Normal + Vector3.One );
        }

        var unitDirection = Vector3.Normalize(p_ray.Direction);
        var a             = 0.5f * ( unitDirection.Y + 1.0f );
        return ( 1.0f - a ) * Vector3.One + a * new Vector3(0.5f, 0.7f, 1.0f);
    }
}