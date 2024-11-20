using System;
using System.Numerics;
using System.Runtime.CompilerServices;

using PlowTracer.Core.DataStructures.Render.Primitives.Intersection;
using PlowTracer.Core.DataStructures.Render.Primitives.Intersection.IntersectableEntities;
using PlowTracer.Core.DataStructures.Render.Settings;
using PlowTracer.Core.DataStructures.Utilities;

namespace PlowTracer.Core.DataStructures.Render.Primitives.Camera;

internal readonly record struct ThinLensCamera : ICamera
{
    internal ThinLensCamera(RenderSettings p_renderSettings, Scene p_scene)
    {
        Scene = p_scene;
        
        Origin = p_renderSettings.CameraOrigin;
        Target = p_renderSettings.CameraTarget;
        
        var aspectRatio = p_renderSettings.Width / (float)p_renderSettings.Height;
        var fieldOfView = p_renderSettings.CameraFieldOfView;
        var focalLength = p_renderSettings.CameraFocalLength;
        
        var theta = MathUtilities.DegreesToRadians(fieldOfView);
        var h     = MathF.Tan(theta / 2);

        var viewportHeight = 2              * h * focalLength;
        var viewportWidth  = viewportHeight * aspectRatio;
        
        var w = Vector3.Normalize(Origin - Target);
        var u = Vector3.Normalize(Vector3.Cross(p_renderSettings.CameraUp, w));
        var v = Vector3.Cross(w, u);

        var viewportU = viewportWidth * u;
        var viewportV = viewportHeight * -v;
        
        PixelOffsetU = viewportU / p_renderSettings.Width;
        PixelOffsetV = viewportV / p_renderSettings.Height;
        
        var viewportOrigin = Origin - focalLength * w - viewportU / 2 - viewportV / 2;
        PixelOrigin = viewportOrigin + 0.5f * (PixelOffsetU + PixelOffsetV);
    }
    
    public Scene   Scene  { get; }

    public Vector3 Origin       { get; }
    public Vector3 Target       { get; }
    
    // private Vector3 U { get; }
    // private Vector3 V { get; }
    // public  Vector3 W { get; }
    
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
}