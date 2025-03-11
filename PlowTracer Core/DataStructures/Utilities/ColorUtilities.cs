using System;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace PlowTracer.Core.DataStructures.Utilities;

internal static class ColorUtilities
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static float LinearToGamma(float p_linearValue)
    {
        return p_linearValue > 0.0f ? MathF.Sqrt(p_linearValue) : 0.0f;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static Vector3 LinearToGamma(Vector3 p_linearValues)
    {
        return new Vector3(LinearToGamma(p_linearValues.X), LinearToGamma(p_linearValues.Y), LinearToGamma(p_linearValues.Z));
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static float LinearToSrgb(float p_linearValue)
    {
        return p_linearValue <= 0.0031308f ? 12.92f * p_linearValue : 1.055f * MathF.Pow(p_linearValue, 1.0f / 2.4f) - 0.055f;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static Vector3 LinearToSrgb(Vector3 p_linearValues)
    {
        return new Vector3(LinearToSrgb(p_linearValues.X), LinearToSrgb(p_linearValues.Y), LinearToSrgb(p_linearValues.Z));
    }
}