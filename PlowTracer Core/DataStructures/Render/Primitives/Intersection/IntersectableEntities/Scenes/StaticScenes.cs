using System.Numerics;

using PlowTracer.Core.DataStructures.Render.Primitives.Intersection.IntersectableEntities.Shapes;
using PlowTracer.Core.DataStructures.Render.Primitives.Intersection.Materials;

namespace PlowTracer.Core.DataStructures.Render.Primitives.Intersection.IntersectableEntities.Scenes;

public static class StaticScenes
{
    public static Scene ThreeSpheresScene()
    {
        var greenLambertian = new LambertianDiffuse(new Vector3(0.8f, 0.8f, 0.0f));
        var blueLambertian  = new LambertianDiffuse(new Vector3(0.1f, 0.2f, 0.5f));
        
        var bronzeMetal = new Metal(new Vector3(0.8f, 0.6f, 0.2f), 0.15f);
        
        var glass  = new Dielectric(new Vector3(0.975f, 0.975f, 0.975f), 1.5f);
        var bubble = new Dielectric(new Vector3(0.975f, 0.975f, 0.975f), 1.0f / 1.5f);
        
        return new Scene([
                             new Sphere(new Vector3(0.0f, 0.0f, -1.2f), 0.5f, blueLambertian),
                             new Sphere(new Vector3(0.0f, -100.5f, -1.0f), 100.0f, greenLambertian),
                             new Sphere(new Vector3(-1.0f, 0.0f, -1.0f), 0.5f, glass),
                             new Sphere(new Vector3(-1.0f, 0.0f, -1.0f), 0.45f, bubble),
                             new Sphere(new Vector3(1.0f, 0.0f, -1.0f), 0.5f, bronzeMetal)
                         ]);
    }
}