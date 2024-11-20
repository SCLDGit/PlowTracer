using System.Numerics;

namespace PlowTracer.Core.DataStructures.Render.Primitives.Intersection.Materials;

internal class NullMaterial : IMaterial
{
    internal static NullMaterial Instance = new();
    
    public Vector3      Albedo                                                  { get; } = Vector3.One;
    public ScatterRecord Scatter(ref Ray p_ray, ref Intersection p_intersection)
    {
        throw new System.NotImplementedException();
    }
}