using System.ComponentModel;

namespace FluentUI.Blazor.Community.Components.Tests.Components;

public class ByteSizeTypeConverterTests
{
    [Fact]
    public void CanConvertFromString()
    {
        var converter = TypeDescriptor.GetConverter(typeof(ByteSize));
        Assert.True(converter.CanConvertFrom(typeof(string)));
        var value = converter.ConvertFrom("1 KB");
        Assert.IsType<ByteSize>(value);
        Assert.Equal(ByteSize.FromBytes(1000), value);
    }

    [Fact]
    public void CanConvertToString()
    {
        var converter = TypeDescriptor.GetConverter(typeof(ByteSize));
        var str = converter.ConvertTo(ByteSize.FromBytes(2048), typeof(string)) as string;
        Assert.IsType<string>(str);
        Assert.True(str!.Contains("2.05 KB") || str.Contains("2,05 KB") || str.Contains("2 KiB"));
    }

    [Fact]
    public void InvalidString_Throws()
    {
        var converter = TypeDescriptor.GetConverter(typeof(ByteSize));
        Assert.ThrowsAny<System.Exception>(() => converter.ConvertFrom("not a size"));
    }
}
