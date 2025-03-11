using System.Numerics;

using PlowTracer.Core.DataStructures.Render.Primitives.Intersection;
using PlowTracer.Core.DataStructures.Render.Primitives.Intersection.IntersectableEntities;

namespace PlowTracer.Core.Core.Tracers;

public interface ITracer
{
    public Vector3 GetPixelColor(ref Ray p_ray, Scene p_scene);
}