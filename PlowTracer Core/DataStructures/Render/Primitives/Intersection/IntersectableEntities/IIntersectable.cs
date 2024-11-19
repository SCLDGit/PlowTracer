namespace PlowTracer.Core.DataStructures.Render.Primitives.Intersection.IntersectableEntities;

internal interface IIntersectable
{
    public IntersectionRecord GetIntersection(ref Ray p_ray);
}