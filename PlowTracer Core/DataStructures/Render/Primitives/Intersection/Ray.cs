using System.Numerics;
using System.Runtime.CompilerServices;

using PlowTracer.Core.DataStructures.Math.Primitives;

namespace PlowTracer.Core.DataStructures.Render.Primitives.Intersection;

public readonly struct Ray(Vector3 c_origin, Vector3 c_direction, int c_currentDepth = 0, float c_minimumIntersectionDistance = 0.0f, float c_maximumIntersectionDistance = float.PositiveInfinity)
{
    internal Vector3         Origin            { get; } = c_origin;
    internal Vector3         Direction         { get; } = c_direction;
    internal int             CurrentDepth      { get; } = c_currentDepth;
    internal Interval<float> IntersectionRange { get; } = new (c_minimumIntersectionDistance, c_maximumIntersectionDistance);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal Vector3 GetPointAt(float p_distance)
    {
        return Origin + Direction * p_distance;
    }
}