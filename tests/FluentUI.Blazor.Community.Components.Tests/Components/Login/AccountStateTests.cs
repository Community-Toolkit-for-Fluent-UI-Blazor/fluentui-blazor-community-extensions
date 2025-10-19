using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Login;

public class AccountStateTests
{
    [Fact]
    public void Default_Values_Are_Correct()
    {
        var state = new AccountState();
        Assert.Equal(string.Empty, state.ReturnUrl);
        Assert.False(state.RememberMe);
        Assert.Null(state.Provider);
    }

    [Fact]
    public void Properties_CanBe_Set_And_Get()
    {
        var state = new AccountState
        {
            ReturnUrl = "/home",
            RememberMe = true,
            Provider = "Google"
        };

        Assert.Equal("/home", state.ReturnUrl);
        Assert.True(state.RememberMe);
        Assert.Equal("Google", state.Provider);
    }
}
