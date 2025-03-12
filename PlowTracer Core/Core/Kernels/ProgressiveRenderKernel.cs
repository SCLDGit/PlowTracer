using System.Collections.Generic;

using PlowTracer.Core.DataStructures.Render.Result;
using PlowTracer.Core.DataStructures.Render.Settings;

namespace PlowTracer.Core.Core.Kernels;

public class ProgressiveRenderKernel : IRenderKernel
{
    public IAsyncEnumerable<RenderResult> RenderAsync(RenderSettings p_settings)
    {
        throw new System.NotImplementedException();
    }
}