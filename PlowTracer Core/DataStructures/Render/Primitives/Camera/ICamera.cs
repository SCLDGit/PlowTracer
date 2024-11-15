using System.Numerics;

using PlowTracer.Core.DataStructures.Render.Primitives.IntersectableEntities;

namespace PlowTracer.Core.DataStructures.Render.Primitives.Camera;

internal interface ICamera
{
    Scene   Scene  { get; }
    
    Vector3 Origin      { get; }
    int     Width       { get; }
    int     Height      { get; }
    float   AspectRatio { get; }
    float   FieldOfView { get; }
    float   FocalLength { get; }
    
    float ViewportHeight { get; }
    float ViewportWidth   { get; }
    
    Vector3 ViewportU { get; }
    Vector3 ViewportV { get; }
    
    Vector3 PixelOffsetU { get; }
    Vector3 PixelOffsetV { get; }
    
    Vector3 ViewportOrigin { get; }
    Vector3 PixelOrigin { get; }

    Ray GetRay(int p_x, int p_y);

    Vector3 GetPixelColor(ref Ray p_ray);
}