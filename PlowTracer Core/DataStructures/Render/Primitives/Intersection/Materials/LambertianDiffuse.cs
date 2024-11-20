using System.Numerics;
using System.Runtime.CompilerServices;

using PlowTracer.Core.DataStructures.Utilities;

namespace PlowTracer.Core.DataStructures.Render.Primitives.Intersection.Materials;

internal class LambertianDiffuse(Vector3 c_albedo) : IMaterial
{
    public Vector3 Albedo { get; } = c_albedo;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ScatterRecord Scatter(ref Ray p_ray, ref Intersection p_intersection)
    {
        var scatterDirection = p_intersection.Normal + MathUtilities.GetRandomUnitVector();
        return new ScatterRecord(new Ray(p_intersection.Point + p_intersection.Normal * 0.00001f, scatterDirection, p_ray.CurrentDepth + 1), Albedo);
    }
}