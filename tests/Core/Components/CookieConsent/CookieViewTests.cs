using System;
using FluentUI.Blazor.Community.Components;
using Xunit;

namespace Components.Tests.Components.CookieConsent;

public class CookieViewTests
{
    [Fact]
    public void CookieView_DefinesExpectedValues()
    {
        Assert.Equal(0, (int)CookieView.Default);
        Assert.Equal(1, (int)CookieView.Small);
    }

    [Fact]
    public void CookieView_ContainsExpectedMembers()
    {
        var values = Enum.GetValues<CookieView>();

        Assert.Equal(2, values.Length);
        Assert.Contains(CookieView.Default, values);
        Assert.Contains(CookieView.Small, values);
    }
}
