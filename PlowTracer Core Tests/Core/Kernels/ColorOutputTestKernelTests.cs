using System;
using System.Numerics;
using System.Threading.Tasks;

using FluentAssertions;

using PlowTracer.Core.Core.Kernels;
using PlowTracer.Core.DataStructures.Render.Settings;

namespace PlowTracer.Core.Tests.Core.Kernels;

public class ColorOutputTestKernelTests
{
    [Theory]
    [InlineData(2, 2)]
    [InlineData(4, 4)]
    [InlineData(8, 8)]
    [InlineData(16, 16)]
    [InlineData(32, 32)]
    [InlineData(64, 64)]
    [InlineData(128, 128)]
    [InlineData(256, 256)]
    public void TEST_CreateRenderSettings(int p_width, int p_height)
    {
        var renderSettings = new RenderSettings(p_width, p_height, 1, Vector3.Zero, 90, 1);
        
        renderSettings.Should().NotBeNull();
        renderSettings.Width.Should().Be(p_width);
        renderSettings.Height.Should().Be(p_height);
    }

    [Fact]
    public void TEST_CreateRenderSettings_InvalidDimensions()
    {
        var createRenderSettingsAction = () =>
                                         {
                                             _ = new RenderSettings(1, 1, 1, Vector3.Zero, 90, 1);
                                         };

        createRenderSettingsAction.Should().Throw<ArgumentException>();
    }
    
    [Theory]
    [InlineData(2, 2)]
    [InlineData(4, 4)]
    [InlineData(8, 8)]
    [InlineData(16, 16)]
    [InlineData(32, 32)]
    [InlineData(64, 64)]
    [InlineData(128, 128)]
    [InlineData(256, 256)]
    public async Task TEST_Render(int p_width, int p_height)
    {
        var renderSettings = new RenderSettings(p_width, p_height, 1, Vector3.Zero, 90, 1);

        var kernel = new ColorOutputTestKernel();

        var result = await kernel.Render(renderSettings);

        result.Data[0].Should().Be(0x00);
        result.Data[1].Should().Be(0x00);
        result.Data[2].Should().Be(0x00);
        result.Data[3].Should().Be(0xFF);
        
        result.Data[^1].Should().Be(0xFF);
        result.Data[^2].Should().Be(0x00);
        result.Data[^3].Should().Be(0xFF);
        result.Data[^4].Should().Be(0xFF);
    }
}