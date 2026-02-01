using System;
using FluentUI.Blazor.Community.Components;
using Xunit;

namespace Components.Tests.Components.CookieConsent;

public class OpenCookieVisibilityTests
{
    [Fact]
    public void OpenCookieVisibility_DefinesExpectedValues()
    {
        Assert.Equal(0, (int)OpenCookieVisibility.Always);
        Assert.Equal(1, (int)OpenCookieVisibility.Never);
        Assert.Equal(2, (int)OpenCookieVisibility.WhenFirstHidden);
    }

    [Fact]
    public void OpenCookieVisibility_ContainsExpectedMembers()
    {
        var values = Enum.GetValues<OpenCookieVisibility>();

        Assert.Equal(3, values.Length);
        Assert.Contains(OpenCookieVisibility.Always, values);
        Assert.Contains(OpenCookieVisibility.Never, values);
        Assert.Contains(OpenCookieVisibility.WhenFirstHidden, values);
    }
}
