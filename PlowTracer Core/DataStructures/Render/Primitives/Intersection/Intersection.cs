using System.Numerics;
using System.Runtime.CompilerServices;

using PlowTracer.Core.DataStructures.Render.Primitives.Intersection.Materials;

namespace PlowTracer.Core.DataStructures.Render.Primitives.Intersection;

internal record struct Intersection(Vector3 Point, IMaterial Material, float Distance, bool Intersected = false)
{
    internal static Intersection Miss = new(Vector3.Zero, NullMaterial.Instance, float.PositiveInfinity);
    
    internal Vector3   Normal        { get; private set; }
    internal bool      IsFrontFacing { get; private set; }
    

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal void SetNormal(Ray p_ray, Vector3 p_normal)
    {
        IsFrontFacing = Vector3.Dot(p_ray.Direction, p_normal) < 0;
        Normal        = IsFrontFacing ? p_normal : -p_normal;
    }
}