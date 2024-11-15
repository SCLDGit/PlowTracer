using System;
using System.Numerics;

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
        
        Width = p_renderSettings.Width;
        Height = p_renderSettings.Height;
        
        AspectRatio = Width / (float)Height;
        FieldOfView = p_renderSettings.CameraFieldOfView;
        FocalLength = p_renderSettings.CameraFocalLength;
        
        var theta = MathUtilities.DegreesToRadians(FieldOfView);
        var h     = MathF.Tan(theta / 2);

        ViewportHeight = 2              * h * FocalLength;
        ViewportWidth  = ViewportHeight * AspectRatio;
        
        ViewportU = new Vector3(ViewportWidth, 0, 0);
        ViewportV = new Vector3(0, -ViewportHeight, 0);
        
        PixelOffsetU = ViewportU / Width;
        PixelOffsetV = ViewportV / Height;
        
        ViewportOrigin = Origin - new Vector3(0, 0, FocalLength) - ViewportU / 2 - ViewportV / 2;
        PixelOrigin = ViewportOrigin + 0.5f * (PixelOffsetU + PixelOffsetV);
    }
    
    public Scene   Scene  { get; }
    
    public  Vector3 Origin { get; }
    
    public int Width  { get; }
    public int Height { get; }

    public float AspectRatio { get; }
    public float FieldOfView { get; }
    public float FocalLength { get; }
    
    public float ViewportHeight { get; }
    public float ViewportWidth { get; }
    
    public Vector3 ViewportU { get; }
    public Vector3 ViewportV { get; }
    
    public Vector3 PixelOffsetU { get; }
    public Vector3 PixelOffsetV { get; }
    
    public Vector3 ViewportOrigin { get; }
    public Vector3 PixelOrigin { get; }
    
    public Ray GetRay(int p_x, int p_y)
    {
        var pixelTarget = PixelOrigin + p_x * PixelOffsetU + p_y * PixelOffsetV;

        return new Ray(Origin, pixelTarget - Origin);
    }
    
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