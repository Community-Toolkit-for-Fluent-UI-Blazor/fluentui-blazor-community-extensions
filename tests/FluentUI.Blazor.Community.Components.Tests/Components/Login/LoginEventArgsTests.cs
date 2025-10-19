using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Login;

public class LoginEventArgsTests
{
    [Fact]
    public void Ctor_Sets_Properties()
    {
        var args = new LoginEventArgs("user", "pwd", true);
        Assert.Equal("user", args.Email);
        Assert.Equal("pwd", args.Password);
        Assert.True(args.RememberMe);
    }

    [Fact]
    public void Default_FailReason_None_And_IsSuccessful_True()
    {
        var args = new LoginEventArgs(null, null, false);
        Assert.Equal(LoginFailReason.None, args.FailReason);
        Assert.True(args.IsSuccessful);
    }

    [Fact]
    public void IsSuccessful_False_When_FailReason_Set()
    {
        var args = new LoginEventArgs("u", "p", false);
        var alt = Enum.GetValues<LoginFailReason>().FirstOrDefault(v => v != LoginFailReason.None);
        args.FailReason = alt;
        Assert.Equal(alt, args.FailReason);
        Assert.False(args.IsSuccessful);
    }

    [Fact]
    public void FailReason_Setter_Allows_Changing()
    {
        var args = new LoginEventArgs("u", "p", false);
        args.FailReason = LoginFailReason.InvalidCredentials;
        Assert.Equal(LoginFailReason.InvalidCredentials, args.FailReason);

        args.FailReason = LoginFailReason.AccountLocked;
        Assert.Equal(LoginFailReason.AccountLocked, args.FailReason);
    }
}
