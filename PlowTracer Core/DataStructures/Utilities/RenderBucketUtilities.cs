using System.Collections.Generic;

using PlowTracer.Core.DataStructures.Render.Buckets;
using PlowTracer.Core.DataStructures.Render.Settings;

namespace PlowTracer.Core.DataStructures.Utilities;

internal static class RenderBucketUtilities
{
    internal static RenderBucket[] GetRenderBuckets(RenderSettings p_renderSettings)
    {
        var bucketsList = new List<RenderBucket>();
        
        for (var y = 0; y < p_renderSettings.Height; y += p_renderSettings.BucketSize)
        {
            for (var x = 0; x < p_renderSettings.Width; x += p_renderSettings.BucketSize)
            {
                var bucket = new RenderBucket
                             {
                                 StartX = x,
                                 StartY = y,
                                 Width  = System.Math.Min(p_renderSettings.BucketSize, p_renderSettings.Width  - x),
                                 Height = System.Math.Min(p_renderSettings.BucketSize, p_renderSettings.Height - y)
                             };
                
                bucketsList.Add(bucket);
            }
        }
        
        return bucketsList.ToArray();
    }
}