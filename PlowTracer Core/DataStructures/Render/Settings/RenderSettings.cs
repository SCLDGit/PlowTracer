using System;
using System.Numerics;
using System.Runtime.CompilerServices;

using PlowTracer.Core.Core.Tracers;

[assembly: InternalsVisibleTo("PlowTracer.Core.Tests")]

namespace PlowTracer.Core.DataStructures.Render.Settings;

public readonly record struct RenderSettings(int     Width,
                                             int     Height,
                                             int     Samples,
                                             int     MaxBounces,
                                             int     BucketSize,
                                             Vector3 CameraOrigin,
                                             Vector3 CameraTarget,
                                             Vector3 CameraUp,
                                             float   CameraFieldOfView,
                                             float   CameraFocalLength,
                                             ITracer Tracer)
{
    public RenderSettings() : this(Width: 1366,
                                   Height: 768,
                                   Samples: 16,
                                   MaxBounces: 5,
                                   CameraOrigin: new Vector3(0, 0, 0),
                                   CameraTarget: new Vector3(0, 0, -1),
                                   CameraUp: new Vector3(0, 1, 0),
                                   CameraFieldOfView: 60.0f,
                                   CameraFocalLength: 1.0f,
                                   Tracer: new NormalsTracer(),
                                   BucketSize: 32)
    {
        Validate();
    }
    
    private void Validate()
    {
        if ( Width < 2 )
        {
            throw new ArgumentException("Invalid Render Settings: Width must be at least 2");
        }

        if ( Height < 2 )
        {
            throw new ArgumentException("Invalid Render Settings: Height must be at least 2");
        }

        if ( Samples < 1 )
        {
            throw new ArgumentException("Invalid Render Settings: Samples must be at least 1");
        }

        if ( MaxBounces < 1 )
        {
            throw new ArgumentException("Invalid Render Settings: Max Bounces must be at least 1");
        }

        if ( BucketSize < 2 )
        {
            throw new ArgumentException("Invalid Render Settings: Bucket size must be at least 2");
        }
    }
}