namespace PlowTracer.Core.DataStructures.Render.Primitives.Intersection.IntersectableEntities;

public interface IIntersectable
{
    public Intersection GetIntersection(ref Ray p_ray);
}