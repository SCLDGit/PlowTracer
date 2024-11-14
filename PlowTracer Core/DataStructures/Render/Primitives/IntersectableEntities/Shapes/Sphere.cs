using System;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace PlowTracer.Core.DataStructures.Render.Primitives.IntersectableEntities.Shapes;

internal class Sphere(Vector3 c_center, float c_radius) : IIntersectable
{
    internal Vector3 Center { get; } = c_center;
    internal float   Radius { get; } = c_radius >= 0 ? c_radius : throw new ArgumentOutOfRangeException(nameof(Radius), "Radius must be at least zero.");
    
    private float RadiusSquared => Radius * Radius;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IntersectionRecord GetIntersection(ref Ray p_ray)
    {
        var originToCenter = Center - p_ray.Origin;
        
        var a = p_ray.Direction.LengthSquared();
        var h = Vector3.Dot(p_ray.Direction, originToCenter);
        var c = originToCenter.LengthSquared() - RadiusSquared;
        
        var discriminant = h * h - a * c;

        if ( discriminant < 0 ) return IntersectionRecord.Miss;
        
        var squareRootOfDiscriminant = MathF.Sqrt(discriminant);
        
        var intersectionDistance = (h - squareRootOfDiscriminant) / a;

        if ( !p_ray.IntersectionRange.ContainsExclusive(intersectionDistance) )
        {
            intersectionDistance = (h + squareRootOfDiscriminant) / a;

            if ( !p_ray.IntersectionRange.ContainsExclusive(intersectionDistance) )
            {
                return IntersectionRecord.Miss;
            }
        }
        
        var intersectionPoint = p_ray.GetPointAt(intersectionDistance);
        var intersectionNormal = (intersectionPoint - Center) / Radius;
        
        var intersectionRecord = new IntersectionRecord(intersectionPoint, intersectionDistance, true);
        
        intersectionRecord = intersectionRecord.WithFaceNormal(p_ray, intersectionNormal);
        
        return intersectionRecord;
    }
}