using System.Numerics;
using System.Runtime.CompilerServices;

namespace PlowTracer.Core.DataStructures.Render.Primitives;

internal record struct IntersectionRecord(Vector3 Point, float Distance, bool Intersected = false)
{
    internal static IntersectionRecord Miss => new(Vector3.Zero, float.PositiveInfinity);
    
    internal Vector3 Normal        { get; private set; }
    internal bool    IsFrontFacing { get; private set; }
    

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal void SetNormal(Ray p_ray, Vector3 p_normal)
    {
        IsFrontFacing = Vector3.Dot(p_ray.Direction, p_normal) < 0;
        Normal        = IsFrontFacing ? p_normal : -p_normal;
    }
}