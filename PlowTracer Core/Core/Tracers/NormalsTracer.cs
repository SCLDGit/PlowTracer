using System.Numerics;
using System.Runtime.CompilerServices;

using PlowTracer.Core.DataStructures.Render.Primitives.Intersection;
using PlowTracer.Core.DataStructures.Render.Primitives.Intersection.IntersectableEntities;

namespace PlowTracer.Core.Core.Tracers;

public class NormalsTracer : ITracer
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector3 GetPixelColor(ref Ray p_ray, Scene p_scene)
    {
        var intersection = p_scene.GetIntersection(ref p_ray);
        
        if ( intersection.Intersected )
        {
            return 0.5f * ( intersection.Normal + Vector3.One );
        }

        var unitDirection = Vector3.Normalize(p_ray.Direction);
        var a             = 0.5f * ( unitDirection.Y + 1.0f );
        return ( 1.0f - a ) * Vector3.One + a * new Vector3(0.5f, 0.7f, 1.0f);
    }
}