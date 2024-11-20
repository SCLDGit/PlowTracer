using System.Threading.Tasks;

using PlowTracer.Core.DataStructures.Render.Result;
using PlowTracer.Core.DataStructures.Render.Settings;

namespace PlowTracer.Core.Core.Kernels;

public interface IRenderKernel
{
    public Task<RenderResult> Render(RenderSettings p_settings);
}