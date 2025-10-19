using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Login;

public class ExternalAuthenticationProviderTests
{
    [Fact]
    public void Ctor_Sets_Name_And_DisplayName()
    {
        var provider = new ExternalAuthenticationProvider("google", "Google");
        Assert.Equal("google", provider.Name);
        Assert.Equal("Google", provider.DisplayName);
    }

    [Fact]
    public void Records_Are_Equality_Comparable()
    {
        var a = new ExternalAuthenticationProvider("p", "P");
        var b = new ExternalAuthenticationProvider("p", "P");
        var c = new ExternalAuthenticationProvider("q", "Q");

        Assert.Equal(a, b);
        Assert.NotEqual(a, c);
    }
}
