using System.Numerics;
using System.Runtime.CompilerServices;

using PlowTracer.Core.DataStructures.Render.Primitives.Intersection;
using PlowTracer.Core.DataStructures.Render.Primitives.Intersection.IntersectableEntities;
using PlowTracer.Core.DataStructures.Utilities;

namespace PlowTracer.Core.Core.Tracers;

internal class LambertianTracer(int c_maxBounces) : ITracer
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
            var direction  = intersection.Normal + MathUtilities.GetRandomUnitVectorOnHemisphere(intersection.Normal);
            var bouncedRay = new Ray(intersection.Point + intersection.Normal * 0.0001f, direction, p_ray.CurrentDepth + 1);
            return 0.5f * GetPixelColor(ref bouncedRay, p_scene);
        }

        var unitDirection = Vector3.Normalize(p_ray.Direction);
        var a             = 0.5f * ( unitDirection.Y + 1.0f );
        return ( 1.0f - a ) * Vector3.One + a * new Vector3(0.5f, 0.7f, 1.0f);
    }
}