using System;
using System.Numerics;
using System.Runtime.CompilerServices;

using PlowTracer.Core.DataStructures.Utilities;

namespace PlowTracer.Core.DataStructures.Render.Primitives.Intersection.Materials;

internal class Dielectric(Vector3 c_albedo, float c_indexOfRefraction, float c_roughness = 0.001f) : IMaterial
{
    public Vector3 Albedo { get; } = c_albedo;
    public float Roughness { get; } = c_roughness;
    
    public float   IndexOfRefraction { get; } = c_indexOfRefraction;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ScatterRecord Scatter(ref Ray p_ray, ref Intersection p_intersection)
    {
        var indexOfRefraction = p_intersection.IsFrontFacing ? 1.0f / IndexOfRefraction : IndexOfRefraction;
        
        var unitDirection = Vector3.Normalize(p_ray.Direction);
        
        var cosineTheta = MathF.Min(Vector3.Dot(-unitDirection, p_intersection.Normal), 1.0f);
        var sinTheta = MathF.Sqrt(1.0f - cosineTheta * cosineTheta);
        
        var cannotRefract = indexOfRefraction * sinTheta > 1.0f;
        
        var scatteredRay = cannotRefract || GetReflectivity(cosineTheta, indexOfRefraction) > Random.Shared.NextSingle() ? new Ray(p_intersection.Point + p_intersection.Normal * 0.0001f, Vector3.Normalize(Vector3.Reflect(p_ray.Direction, p_intersection.Normal)) + Roughness * MathUtilities.GetRandomUnitVector(), p_ray.CurrentDepth + 1) : new Ray(p_intersection.Point - p_intersection.Normal * 0.0001f, Vector3.Normalize(MathUtilities.GetRefractedVector(unitDirection, p_intersection.Normal, indexOfRefraction)) + Roughness * MathUtilities.GetRandomUnitVector(), p_ray.CurrentDepth + 1);
        
        return new ScatterRecord(scatteredRay, Albedo);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private float GetReflectivity(float p_cosine, float p_indexOfRefraction)
    {
        var r0 = ( 1 - p_indexOfRefraction ) / ( 1 + p_indexOfRefraction );
        r0 *= r0;
        return r0 + ( 1 - r0 ) * MathF.Pow( 1 - p_cosine, 5);
    }
}