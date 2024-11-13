namespace PlowTracer.Core.DataStructures.Render.Result;

public record RenderResult
{
    public RenderResult(int p_width, int p_height)
    {
        Data = new byte[p_width * p_height * 4];
    }
    
    public byte[] Data { get; }
}