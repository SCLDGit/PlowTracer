using System.Numerics;

namespace PlowTracer.Core.DataStructures.Render.Primitives.Intersection;

internal record struct ScatterRecord
{
    internal ScatterRecord(Ray p_outRay, Vector3 p_attenuation)
    {
        OutRay      = p_outRay;
        Attenuation = p_attenuation;
    }

    internal Ray OutRay;
    internal Vector3 Attenuation { get; }
}