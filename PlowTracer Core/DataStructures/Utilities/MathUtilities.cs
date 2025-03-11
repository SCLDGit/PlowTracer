using System;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace PlowTracer.Core.DataStructures.Utilities;

internal static class MathUtilities
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static float DegreesToRadians(float p_degrees)
    {
        return MathF.PI / 180 * p_degrees;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static float RadiansToDegrees(float p_radians)
    {
        return 180 / MathF.PI * p_radians;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static float NextSingle(this Random p_random, float p_minimum, float p_maximum)
    {
        return p_minimum + (p_maximum - p_minimum) * p_random.NextSingle();
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static Vector2 GetRandomPointInUnitSquare()
    {
        return new Vector2(Random.Shared.NextSingle() - 0.5f, Random.Shared.NextSingle() - 0.5f);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static Vector3 GetRandomUnitVector()
    {
        var z = Random.Shared.NextSingle(-1, 1);
        var axialDistance = MathF.Sqrt(1 - z * z);
        var theta = Random.Shared.NextSingle(0, MathF.PI * 2);
        var x = axialDistance * MathF.Cos(theta);
        var y = axialDistance * MathF.Sin(theta);
        return new Vector3(x, y, z);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static Vector2 GetRandomInUnitDisk()
    {
        var z             = Random.Shared.NextSingle(-1, 1);
        var axialDistance = MathF.Sqrt(1 - z * z);
        var theta         = Random.Shared.NextSingle(0, MathF.PI * 2);
        var x             = axialDistance * MathF.Cos(theta);
        var y             = axialDistance * MathF.Sin(theta);
        return new Vector2(x, y);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static Vector3 GetRandomUnitVectorOnHemisphere(Vector3 p_normal)
    {
        var randomUnitSphereVector = GetRandomUnitVector();

        return Vector3.Dot(randomUnitSphereVector, p_normal) > 0.0f ? randomUnitSphereVector : -randomUnitSphereVector;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static Vector3 GetRefractedVector(Vector3 p_direction, Vector3 p_normal, float p_etaiOverEtat)
    {
        var cosineTheta = MathF.Min(Vector3.Dot(-p_direction, p_normal), 1.0f);

        var perpendicularOut = p_etaiOverEtat * ( p_direction + cosineTheta * p_normal );
        var parallelOut      = -MathF.Sqrt(MathF.Abs(1.0f - perpendicularOut.LengthSquared())) * p_normal;

        return perpendicularOut + parallelOut;
    }
}