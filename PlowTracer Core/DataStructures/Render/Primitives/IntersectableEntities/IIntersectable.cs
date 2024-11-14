namespace PlowTracer.Core.DataStructures.Render.Primitives.IntersectableEntities;

internal interface IIntersectable
{
    public IntersectionRecord GetIntersection(ref Ray p_ray);
}