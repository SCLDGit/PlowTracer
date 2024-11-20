using System.Numerics;

namespace PlowTracer.Core.DataStructures.Render.Primitives.Intersection.Materials;

internal interface IMaterial
{
    public Vector3 Albedo { get; }
    
    public ScatterRecord Scatter(ref Ray p_ray, ref Intersection p_intersection);
}