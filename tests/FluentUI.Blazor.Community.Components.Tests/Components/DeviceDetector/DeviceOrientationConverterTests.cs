using System.Text.Json;
using FluentUI.Blazor.Community.Components;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Components.DeviceDetector;

public class DeviceOrientationConverterTests
{
    private readonly JsonSerializerOptions _options = new()
    {
        Converters = { new DeviceOrientationConverter() }
    };

    [Theory]
    [InlineData("Portrait", DeviceOrientation.Portrait)]
    [InlineData("Landscape", DeviceOrientation.Landscape)]
    [InlineData("NotARealOrientation", DeviceOrientation.Unknown)]
    public void Read_ParsesStringToDeviceOrientationEnum(string input, DeviceOrientation expected)
    {
        var json = $"\"{input}\"";
        var result = JsonSerializer.Deserialize<DeviceOrientation>(json, _options);
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Write_ThrowsNotImplementedException()
    {
        var converter = new DeviceOrientationConverter();
        var writer = new Utf8JsonWriter(new System.Buffers.ArrayBufferWriter<byte>());
        Assert.Throws<NotImplementedException>(() => converter.Write(writer, DeviceOrientation.Portrait, _options));
    }
}
