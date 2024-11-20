using System.Numerics;
using System.Runtime.CompilerServices;

using PlowTracer.Core.DataStructures.Utilities;

namespace PlowTracer.Core.DataStructures.Render.Primitives.Intersection.Materials;

internal class Metal(Vector3 c_albedo, float c_glossiness = 0.05f) : IMaterial
{
    public Vector3 Albedo     { get; } = c_albedo;
    public float   Roughness { get; } = c_glossiness;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ScatterRecord Scatter(ref Ray p_ray, ref Intersection p_intersection)
    {
        var reflectedRay = Vector3.Normalize(Vector3.Reflect(p_ray.Direction, p_intersection.Normal)) + Roughness * MathUtilities.GetRandomUnitVector();
        return new ScatterRecord(new Ray(p_intersection.Point + p_intersection.Normal * 0.00001f, reflectedRay, p_ray.CurrentDepth + 1), Albedo);
    }
}