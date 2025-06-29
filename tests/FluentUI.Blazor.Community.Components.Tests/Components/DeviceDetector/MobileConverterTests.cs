using System.Text.Json;
using FluentUI.Blazor.Community.Components;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Components.DeviceDetector;

public class MobileConverterTests
{
    private readonly JsonSerializerOptions _options = new()
    {
        Converters = { new MobileConverter() }
    };

    [Theory]
    [InlineData("IPhone", Mobile.IPhone)]
    [InlineData("Android", Mobile.Android)]
    [InlineData("NotMobileDevice", Mobile.NotMobileDevice)]
    [InlineData("NotARealMobile", Mobile.NotMobileDevice)]
    public void Read_ParsesStringToMobileEnum(string input, Mobile expected)
    {
        var json = $"\"{input}\"";
        var result = JsonSerializer.Deserialize<Mobile>(json, _options);
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Write_ThrowsNotImplementedException()
    {
        var converter = new MobileConverter();
        var writer = new Utf8JsonWriter(new System.Buffers.ArrayBufferWriter<byte>());
        Assert.Throws<NotImplementedException>(() => converter.Write(writer, Mobile.IPhone, _options));
    }
}
