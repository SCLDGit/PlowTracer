using System;
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

public class ProgressiveRenderKernel : IRenderKernel
{
    private Scene? Scene { get; set; }
    
    public async IAsyncEnumerable<RenderResult> RenderAsync(RenderSettings p_settings)
    {
        var renderResult = new RenderResult(p_settings.Width, p_settings.Height);
        
        var camera       = new ThinLensCamera(p_settings);

        Scene = StaticScenes.ThreeSpheresScene();

        var renderBuckets = RenderBucketUtilities.GetRenderBuckets(p_settings);
        
        var accumulationBuffer = new Vector3[p_settings.Width * p_settings.Height];
        
        for (var sampleCount = 0; sampleCount < p_settings.Samples; sampleCount++)
        {
            // Process all buckets for this sample
            var       threadCount     = Environment.ProcessorCount;
            using var renderSemaphore = new SemaphoreSlim(threadCount);
            var       tasks           = new List<Task>();
            
            foreach (var bucket in renderBuckets)
            {
                await renderSemaphore.WaitAsync();
                
                tasks.Add(Task.Run(() =>
                                   {
                                       try
                                       {
                                           ProcessBucketSample(bucket, renderResult, accumulationBuffer, camera, p_settings, sampleCount, sampleCount + 1);
                                       }
                                       finally
                                       {
                                           // ReSharper disable once AccessToDisposedClosure
                                           // Semaphore is always disposed after all tasks have completed. - Comment by Matt Heimlich on 03/12/2025 @ 12:09:35
                                           renderSemaphore.Release();
                                       }
                                   }));
            }
            
            // Wait for all buckets to complete this sample
            await Task.WhenAll(tasks);
            
            // Yield the updated result every 10 samples
            if ((sampleCount + 1) % 10 == 0 ) yield return renderResult;
        }

        yield return renderResult;
    }
    
    private void ProcessBucketSample(
        RenderBucket p_bucket,
        RenderResult p_renderResult,
        Vector3[] p_accumulationBuffer,
        ICamera p_camera,
        RenderSettings p_settings,
        int p_sampleIndex,
        int p_completedSamples)
    {
        const byte alpha = 0xFF;
        
        var sampleScale = 1.0f / p_completedSamples;

        // Process all pixels in this bucket for the current sample
        for (var row = p_bucket.StartY; row < p_bucket.StartY + p_bucket.Height; ++row)
        {
            for (var col = p_bucket.StartX; col < p_bucket.StartX + p_bucket.Width; ++col)
            {
                var pixelIndex = row * p_settings.Width + col;
                
                // Generate ray with randomization for this sample
                var ray = p_camera.GetRay(col, row, true);
                
                // Get color for this ray and add to accumulation buffer
                p_accumulationBuffer[pixelIndex] += p_settings.Tracer.GetPixelColor(ref ray, Scene!);
                
                // Calculate average color from all samples so far
                var pixelColor = p_accumulationBuffer[pixelIndex] * sampleScale;
                
                // Apply color correction
                var correctedPixelColor = ColorUtilities.LinearToSrgb(pixelColor);

                // Convert to bytes and write to result buffer
                var formattedRed = (int)MathF.Round(255 * Math.Clamp(correctedPixelColor.X, 0.0f, 0.999f),
                                               MidpointRounding.AwayFromZero);
                var formattedGreen = (int)MathF.Round(255 * Math.Clamp(correctedPixelColor.Y, 0.0f, 0.999f),
                                                 MidpointRounding.AwayFromZero);
                var formattedBlue = (int)MathF.Round(255 * Math.Clamp(correctedPixelColor.Z, 0.0f, 0.999f),
                                                MidpointRounding.AwayFromZero);

                var resultDataIndex = (row * p_settings.Width + col) * 4;
                p_renderResult.Data[resultDataIndex++] = (byte)formattedRed;
                p_renderResult.Data[resultDataIndex++] = (byte)formattedGreen;
                p_renderResult.Data[resultDataIndex++] = (byte)formattedBlue;
                p_renderResult.Data[resultDataIndex] = alpha;
            }
        }
    }

    public override string ToString()
    {
        return "Progressive Renderer";
    }
}