using System.Numerics;

using PlowTracer.Core.DataStructures.Render.Primitives.Intersection;
using PlowTracer.Core.DataStructures.Render.Primitives.Intersection.IntersectableEntities;

namespace PlowTracer.Core.Core.Tracers;

internal interface ITracer
{
    public Vector3 GetPixelColor(ref Ray p_ray, Scene p_scene);
}