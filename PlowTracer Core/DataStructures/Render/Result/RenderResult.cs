using System;
using System.Buffers;

namespace PlowTracer.Core.DataStructures.Render.Result;

public sealed record RenderResult : IDisposable
{
    public RenderResult(int p_width, int p_height)
    {
        DataSize = p_width * p_height * 4;
        Data = ArrayPool<byte>.Shared.Rent(DataSize);
        Array.Clear(Data, 0, DataSize);
    }
    
    public byte[] Data     { get; }
    public int    DataSize { get; }
    
    public void Dispose()
    {
        ArrayPool<byte>.Shared.Return(Data, clearArray: true);
    }
}