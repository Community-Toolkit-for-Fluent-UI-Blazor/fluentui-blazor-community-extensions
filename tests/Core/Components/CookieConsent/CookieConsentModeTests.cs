using System;
using FluentUI.Blazor.Community.Components;
using Xunit;

namespace Components.Tests.Components.CookieConsent;

public class CookieConsentModeTests
{
    [Fact]
    public void CookieConsentMode_DefinesExpectedValues()
    {
        Assert.Equal(0, (int)CookieConsentMode.AcceptOnly);
        Assert.Equal(1, (int)CookieConsentMode.AcceptDeny);
        Assert.Equal(2, (int)CookieConsentMode.AcceptDenyManage);
    }

    [Fact]
    public void CookieConsentMode_ContainsExpectedMembers()
    {
        var values = Enum.GetValues<CookieConsentMode>();

        Assert.Equal(3, values.Length);
        Assert.Contains(CookieConsentMode.AcceptOnly, values);
        Assert.Contains(CookieConsentMode.AcceptDeny, values);
        Assert.Contains(CookieConsentMode.AcceptDenyManage, values);
    }
}
