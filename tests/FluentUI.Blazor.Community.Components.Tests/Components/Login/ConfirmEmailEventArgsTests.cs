using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Login;

public class ConfirmEmailEventArgsTests
{
    [Fact]
    public void Ctor_Sets_UserId_And_Code()
    {
        var args = new ConfirmEmailEventArgs("user-id", "code-123");
        Assert.Equal("user-id", args.UserId);
        Assert.Equal("code-123", args.Code);
    }

    [Fact]
    public void IsSuccessful_Default_False_And_CanBe_Set()
    {
        var args = new ConfirmEmailEventArgs("u", "c");
        // default bool is false
        Assert.False(args.IsSuccessful);

        args.IsSuccessful = true;
        Assert.True(args.IsSuccessful);
    }
}
