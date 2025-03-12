using System.Collections.Generic;
using System.Threading.Tasks;

using PlowTracer.Core.DataStructures.Render.Result;
using PlowTracer.Core.DataStructures.Render.Settings;

namespace PlowTracer.Core.Core.Kernels;

public interface IRenderKernel
{
    public IAsyncEnumerable<RenderResult> RenderAsync(RenderSettings p_settings);
}