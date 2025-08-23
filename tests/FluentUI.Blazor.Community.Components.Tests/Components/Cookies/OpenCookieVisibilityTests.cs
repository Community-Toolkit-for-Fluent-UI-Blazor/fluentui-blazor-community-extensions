using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Cookies;

public class OpenCookieVisibilityTests
{
    [Fact]
    public void Enum_Should_Have_Expected_Values()
    {
        var values = Enum.GetValues<OpenCookieVisibility>();
        Assert.Contains(OpenCookieVisibility.Always, values);
        Assert.Contains(OpenCookieVisibility.Never, values);
        Assert.Contains(OpenCookieVisibility.WhenFirstHidden, values);
        Assert.Equal(3, values.Length);
    }

    [Theory]
    [InlineData(0, OpenCookieVisibility.Always)]
    [InlineData(1, OpenCookieVisibility.Never)]
    [InlineData(2, OpenCookieVisibility.WhenFirstHidden)]
    public void Enum_Values_Should_Match_Int(int expected, OpenCookieVisibility value)
    {
        Assert.Equal(expected, (int)value);
    }

    [Theory]
    [InlineData("Always", OpenCookieVisibility.Always)]
    [InlineData("Never", OpenCookieVisibility.Never)]
    [InlineData("WhenFirstHidden", OpenCookieVisibility.WhenFirstHidden)]
    public void Enum_Parse_Should_Work(string name, OpenCookieVisibility expected)
    {
        var parsed = (OpenCookieVisibility)Enum.Parse(typeof(OpenCookieVisibility), name);
        Assert.Equal(expected, parsed);
    }
}
