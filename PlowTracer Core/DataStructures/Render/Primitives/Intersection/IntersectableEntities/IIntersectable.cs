namespace PlowTracer.Core.DataStructures.Render.Primitives.Intersection.IntersectableEntities;

internal interface IIntersectable
{
    public Intersection GetIntersection(ref Ray p_ray);
}