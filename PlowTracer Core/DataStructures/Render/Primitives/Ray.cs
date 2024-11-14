using System.Numerics;

namespace PlowTracer.Core.DataStructures.Render.Primitives;

public readonly struct Ray(Vector3 c_origin, Vector3 c_direction)
{
    public Vector3 Origin    { get; } = c_origin;
    public Vector3 Direction { get; } = c_direction;

    public Vector3 GetPointAt(float p_distance)
    {
        return Origin + Direction * p_distance;
    }
}