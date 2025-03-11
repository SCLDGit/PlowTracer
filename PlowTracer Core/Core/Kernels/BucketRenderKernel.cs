using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;

using PlowTracer.Core.DataStructures.Render.Buckets;
using PlowTracer.Core.DataStructures.Render.Primitives.Camera;
using PlowTracer.Core.DataStructures.Render.Primitives.Intersection.IntersectableEntities;
using PlowTracer.Core.DataStructures.Render.Primitives.Intersection.IntersectableEntities.Scenes;
using PlowTracer.Core.DataStructures.Render.Result;
using PlowTracer.Core.DataStructures.Render.Settings;
using PlowTracer.Core.DataStructures.Utilities;

namespace PlowTracer.Core.Core.Kernels;

public class BucketRenderKernel : IRenderKernel
{
    private Scene? Scene { get; set; }
    
    public async Task<RenderResult> RenderAsync(RenderSettings p_settings)
    {
        var renderResult = new RenderResult(p_settings.Width, p_settings.Height);
        
        var camera       = new ThinLensCamera(p_settings);

        Scene = StaticScenes.ThreeSpheresScene();

        var renderBuckets = RenderBucketUtilities.GetRenderBuckets(p_settings);

        var threadCount = Environment.ProcessorCount;

        var renderSemaphore = new SemaphoreSlim(threadCount);

        var bucketQueue = new ConcurrentQueue<RenderBucket>(renderBuckets);

        var renderTasks = new List<Task>();

        while ( bucketQueue.TryDequeue(out var bucket) )
        {
            await renderSemaphore.WaitAsync();
            
            renderTasks.Add(Task.Run(() =>
                                     {
                                         try
                                         {
                                             // Process this bucket completely (all samples)
                                             ProcessBucket(bucket, renderResult, camera, p_settings);
                                         }
                                         finally
                                         {
                                             renderSemaphore.Release();
                                         }
                                     }));
        }

        await Task.WhenAll(renderTasks);

        return renderResult;
    }

    private void ProcessBucket(RenderBucket p_bucket, RenderResult p_renderResult, ICamera p_camera, RenderSettings p_settings)
    {
        const byte alpha       = 0xFF;
        
        var        sampleScale = 1.0f / p_settings.Samples;
        
        for (var row = p_bucket.StartY; row < p_bucket.StartY + p_bucket.Height; ++row)
        {
            for (var col = p_bucket.StartX; col < p_bucket.StartX + p_bucket.Width; ++col)
            {
                var pixelColor = Vector3.Zero;
                
                // Process all samples for this pixel
                for (var sample = 0; sample < p_settings.Samples; ++sample)
                {
                    var ray = p_camera.GetRay(col, row, p_settings.Samples > 1);
                    pixelColor += p_settings.Tracer.GetPixelColor(ref ray, Scene!);
                }
                
                pixelColor *= sampleScale;
                var correctedPixelColor = ColorUtilities.LinearToSrgb(pixelColor);
                
                var formattedRed = (int)MathF.Round(255 * Math.Clamp(correctedPixelColor.X, 0.0f, 0.999f), 
                                                    MidpointRounding.AwayFromZero);
                var formattedGreen = (int)MathF.Round(255 * Math.Clamp(correctedPixelColor.Y, 0.0f, 0.999f), 
                                                      MidpointRounding.AwayFromZero);
                var formattedBlue = (int)MathF.Round(255 * Math.Clamp(correctedPixelColor.Z, 0.0f, 0.999f), 
                                                     MidpointRounding.AwayFromZero);
                
                var resultDataIndex = ((row * p_settings.Width) + col) * 4;
                p_renderResult.Data[resultDataIndex++] = (byte)formattedRed;
                p_renderResult.Data[resultDataIndex++] = (byte)formattedGreen;
                p_renderResult.Data[resultDataIndex++] = (byte)formattedBlue;
                p_renderResult.Data[resultDataIndex]   = alpha;
            }
        }
    }
    
    public override string ToString()
    {
        return "Bucket Renderer";
    }
}