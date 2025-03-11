using System.Numerics;
using System.Runtime.CompilerServices;

using PlowTracer.Core.DataStructures.Render.Primitives.Intersection;
using PlowTracer.Core.DataStructures.Render.Primitives.Intersection.IntersectableEntities;

namespace PlowTracer.Core.Core.Tracers;

public class MaterialTracer(int c_maxBounces) : ITracer
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector3 GetPixelColor(ref Ray p_ray, Scene p_scene)
    {
        if ( p_ray.CurrentDepth > c_maxBounces )
        {
            return Vector3.Zero;
        }
        
        var intersection = p_scene.GetIntersection(ref p_ray);
        
        if ( intersection.Intersected )
        {
            var scatter = intersection.Material.Scatter(ref p_ray, ref intersection);
            return scatter.Attenuation * GetPixelColor(ref scatter.OutRay, p_scene);
        }

        var unitDirection = Vector3.Normalize(p_ray.Direction);
        var a             = 0.5f * ( unitDirection.Y + 1.0f );
        return ( 1.0f - a ) * Vector3.One + a * new Vector3(0.5f, 0.7f, 1.0f);
    }
}