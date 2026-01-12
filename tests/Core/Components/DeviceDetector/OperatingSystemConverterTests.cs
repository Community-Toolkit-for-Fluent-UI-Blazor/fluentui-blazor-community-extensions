using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using FluentUI.Blazor.Community.Components;
using Xunit;
using OperatingSystem = FluentUI.Blazor.Community.Components.OperatingSystem;

namespace Components.Tests.Components.DeviceDetector;

public class OperatingSystemConverterTests
{
    private readonly JsonSerializerOptions _options = new()
    {
        Converters = { new OperatingSystemConverter() }
    };

    [Theory]
    [InlineData("Windows", OperatingSystem.Windows)]
    [InlineData("Mac", OperatingSystem.Mac)]
    [InlineData("Linux", OperatingSystem.Linux)]
    [InlineData("iOS", OperatingSystem.iOS)]
    [InlineData("Android", OperatingSystem.Android)]
    [InlineData("NotARealOS", OperatingSystem.Undefined)]
    public void Read_ParsesStringToOperatingSystemEnum(string input, OperatingSystem expected)
    {
        var json = $"\"{input}\"";
        var result = JsonSerializer.Deserialize<OperatingSystem>(json, _options);
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Write_ThrowsNotImplementedException()
    {
        var converter = new OperatingSystemConverter();
        var writer = new Utf8JsonWriter(new System.Buffers.ArrayBufferWriter<byte>());
        Assert.Throws<NotImplementedException>(() => converter.Write(writer, OperatingSystem.Windows, _options));
    }
}
