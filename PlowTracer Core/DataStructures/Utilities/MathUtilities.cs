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
    internal static Vector2 GetRandomPointInUnitSquare()
    {
        return new Vector2(Random.Shared.NextSingle() - 0.5f, Random.Shared.NextSingle() - 0.5f);
    }
}