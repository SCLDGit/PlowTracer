﻿using System;
using System.Numerics;
using System.Runtime.CompilerServices;

[assembly:InternalsVisibleTo("PlowTracer.Core.Tests")]

namespace PlowTracer.Core.DataStructures.Render.Settings;

public readonly record struct RenderSettings
{
    public RenderSettings(int p_width, int p_height,
                          Vector3 p_cameraOrigin, float p_cameraFieldOfView, float p_cameraFocalLength)
    {
        if ( p_width < 2 || p_height < 2 )
        {
            throw new ArgumentException("Width and height must be at least 2");
        }
        
        Width = p_width;
        Height = p_height;
        
        CameraOrigin = p_cameraOrigin;
        CameraFieldOfView = p_cameraFieldOfView;
        CameraFocalLength = p_cameraFocalLength;
    }
    
    internal int Width { get; }
    internal int Height { get; }
    
    internal Vector3 CameraOrigin      { get; }
    internal float   CameraFieldOfView { get; }
    internal float   CameraFocalLength { get; }
}