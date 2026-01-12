using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using FluentUI.Blazor.Community.Components;
using Xunit;

namespace Components.Tests.Components.DeviceDetector;

public class DeviceOrientationConverterTests
{
    private readonly JsonSerializerOptions _options = new()
    {
        Converters = { new DeviceOrientationConverter() }
    };

    [Theory]
    [InlineData("Portrait", DeviceOrientation.Portrait)]
    [InlineData("PortraitReversed", DeviceOrientation.PortraitReversed)]
    [InlineData("Landscape", DeviceOrientation.Landscape)]
    [InlineData("LandscapeReversed", DeviceOrientation.LandscapeReversed)]
    [InlineData("landscape-primary", DeviceOrientation.Unknown)]
    public void Read_ParsesStringToBrowserEnum(string input, DeviceOrientation expected)
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
