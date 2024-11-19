using System.Numerics;

using PlowTracer.Core.DataStructures.Render.Primitives.IntersectableEntities;

namespace PlowTracer.Core.DataStructures.Render.Primitives.Camera;

internal interface ICamera
{
    Scene   Scene  { get; }
    
    Vector3 Origin      { get; }
    
    Vector3 PixelOffsetU { get; }
    Vector3 PixelOffsetV { get; }
    Vector3 PixelOrigin { get; }

    Ray GetRay(int p_x, int p_y, bool p_multiSampled);

    Vector3 GetPixelColor(ref Ray p_ray);
}