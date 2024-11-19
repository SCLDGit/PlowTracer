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
}