using System.Text.Json;
using FluentUI.Blazor.Community.Components;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Components.DeviceDetector;

public class BrowserConverterTests
{
    private readonly JsonSerializerOptions _options = new()
    {
        Converters = { new BrowserConverter() }
    };

    [Theory]
    [InlineData("Chrome", Browser.Chrome)]
    [InlineData("Firefox", Browser.Firefox)]
    [InlineData("Edge", Browser.Edge)]
    [InlineData("Safari", Browser.Safari)]
    [InlineData("Opera", Browser.Opera)]
    [InlineData("Undefined", Browser.Undefined)]
    [InlineData("NotARealBrowser", Browser.Undefined)]
    public void Read_ParsesStringToBrowserEnum(string input, Browser expected)
    {
        var json = $"\"{input}\"";
        var result = JsonSerializer.Deserialize<Browser>(json, _options);
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Write_ThrowsNotImplementedException()
    {
        var converter = new BrowserConverter();
        var writer = new Utf8JsonWriter(new System.Buffers.ArrayBufferWriter<byte>());
        Assert.Throws<NotImplementedException>(() => converter.Write(writer, Browser.Chrome, _options));
    }
}
