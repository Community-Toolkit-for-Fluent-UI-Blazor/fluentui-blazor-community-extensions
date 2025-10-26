using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Login;

public class RegisterEventArgsTests
{
    [Fact]
    public void Ctor_Sets_Properties()
    {
        var displayName = "User";
        var email = "user@example.com";
        var password = "pwd";
        var url = "/return";

        var args = new RegisterEventArgs(displayName, email, password, url);

        Assert.Equal(displayName, args.DisplayName);
        Assert.Equal(email, args.Email);
        Assert.Equal(password, args.Password);
        Assert.Equal(url, args.ReturnUrl);
    }

    [Fact]
    public void Default_FailReason_Is_Default_And_IsSuccessful_True()
    {
        var args = new RegisterEventArgs("a", "b", "c", "c");
        // Default enum value should be 0 (None)
        Assert.Equal(RegisterFailReason.None, args.FailReason);
        Assert.True(args.IsSuccessful);
    }

    [Fact]
    public void IsSuccessful_False_When_FailReason_Is_Not_None()
    {
        var args = new RegisterEventArgs("a", "b", "c", "c");

        // pick any non-none value
        var alt = Enum.GetValues<RegisterFailReason>().FirstOrDefault(v => v != RegisterFailReason.None);
        args.FailReason = alt;

        Assert.Equal(alt, args.FailReason);
        Assert.False(args.IsSuccessful);
    }

    [Fact]
    public void FailReason_Setter_Allows_Change()
    {
        var args = new RegisterEventArgs("a", "b", "c", "c");
        args.FailReason = RegisterFailReason.EmailAlreadyInUse;
        Assert.Equal(RegisterFailReason.EmailAlreadyInUse, args.FailReason);

        args.FailReason = RegisterFailReason.DisplayNameAlreadyInUse;
        Assert.Equal(RegisterFailReason.DisplayNameAlreadyInUse, args.FailReason);
    }
}
