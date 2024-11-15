using System;

namespace PlowTracer.Core.DataStructures.Utilities;

internal static class MathUtilities
{
    internal static float DegreesToRadians(float p_degrees)
    {
        return MathF.PI / 180 * p_degrees;
    }

    internal static float RadiansToDegrees(float p_radians)
    {
        return 180 / MathF.PI * p_radians;
    }
}