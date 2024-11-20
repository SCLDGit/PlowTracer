using System;
using System.Numerics;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("PlowTracer.Core.Tests")]

namespace PlowTracer.Core.DataStructures.Render.Settings;

public readonly record struct RenderSettings
{
    public RenderSettings(int     p_width,
                          int     p_height,
                          int     p_samples,
                          int     p_maxBounces,
                          Vector3 p_cameraOrigin,
                          Vector3 p_cameraTarget,
                          Vector3 p_cameraUp,
                          float   p_cameraFieldOfView,
                          float   p_cameraFocalLength)
    {
        if ( p_width < 2 || p_height < 2 )
        {
            throw new ArgumentException("Width and height must be at least 2");
        }

        if ( p_maxBounces < 1 )
        {
            throw new ArgumentException("Max bounces must be at least 1");
        }

        Width      = p_width;
        Height     = p_height;
        Samples    = p_samples;
        MaxBounces = p_maxBounces;

        CameraOrigin      = p_cameraOrigin;
        CameraTarget      = p_cameraTarget;
        CameraUp          = p_cameraUp;
        CameraFieldOfView = p_cameraFieldOfView;
        CameraFocalLength = p_cameraFocalLength;
    }

    internal int Width      { get; }
    internal int Height     { get; }
    internal int Samples    { get; }
    internal int MaxBounces { get; }

    internal Vector3 CameraOrigin      { get; }
    internal Vector3 CameraTarget      { get; }
    internal Vector3 CameraUp          { get; }
    internal float   CameraFieldOfView { get; }
    internal float   CameraFocalLength { get; }
}