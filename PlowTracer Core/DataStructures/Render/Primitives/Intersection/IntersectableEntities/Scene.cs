using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace PlowTracer.Core.DataStructures.Render.Primitives.Intersection.IntersectableEntities;

internal class Scene(IEnumerable<IIntersectable> c_sceneEntities) : IIntersectable
{
    private IIntersectable[] Entities { get; } = c_sceneEntities.ToArray();
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IntersectionRecord GetIntersection(ref Ray p_ray)
    {
        var closestInterception = IntersectionRecord.Miss;
        
        foreach ( var entity in Entities )
        {
            var intersection = entity.GetIntersection(ref p_ray);

            if ( !intersection.Intersected ||
                 intersection.Distance > closestInterception.Distance )
            {
                continue;
            }
            
            closestInterception = intersection;
        }
        
        return closestInterception;
    }
}