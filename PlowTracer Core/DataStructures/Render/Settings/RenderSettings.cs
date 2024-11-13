using System;
using System.Runtime.CompilerServices;

[assembly:InternalsVisibleTo("PlowTracer.Core.Tests")]

namespace PlowTracer.Core.DataStructures.Render.Settings;

public record RenderSettings
{
    public RenderSettings(int p_width, int p_height)
    {
        if ( p_width < 2 || p_height < 2 )
        {
            throw new ArgumentException("Width and height must be at least 2");
        }
        
        Width = p_width;
        Height = p_height;
    }
    
    internal int Width { get; }
    internal int Height { get; }
}