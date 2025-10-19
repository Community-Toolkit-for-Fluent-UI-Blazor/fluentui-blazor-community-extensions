using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Login;

public class ExternalProviderProcessingEventArgsTests
{
    [Fact]
    public void Default_IsSuccessful_True_When_NoEmail_And_FailReason_None()
    {
        var args = new ExternalProviderProcessingEventArgs();
        // Default FailReason is enum default (None) and Email is null -> IsSuccessful true per implementation
        Assert.Equal(ExternalProviderProcessingFailReason.None, args.FailReason);
        Assert.True(string.IsNullOrEmpty(args.Email));
        Assert.True(args.IsSuccessful);
    }

    [Fact]
    public void IsSuccessful_False_When_Email_Present()
    {
        var args = new ExternalProviderProcessingEventArgs
        {
            Email = "user@example.com",
            FailReason = ExternalProviderProcessingFailReason.None
        };

        Assert.False(args.IsSuccessful);
    }

    [Fact]
    public void IsSuccessful_False_When_FailReason_Is_Not_None()
    {
        var args = new ExternalProviderProcessingEventArgs
        {
            Email = null,
            FailReason = ExternalProviderProcessingFailReason.LoginInfoUnavailable
        };

        Assert.False(args.IsSuccessful);
    }

    [Fact]
    public void FailReason_Setter_Allows_Change()
    {
        var args = new ExternalProviderProcessingEventArgs();
        args.FailReason = ExternalProviderProcessingFailReason.LoginInfoUnavailable;
        Assert.Equal(ExternalProviderProcessingFailReason.LoginInfoUnavailable, args.FailReason);

        args.FailReason = ExternalProviderProcessingFailReason.LockedOut;
        Assert.Equal(ExternalProviderProcessingFailReason.LockedOut, args.FailReason);
    }
}
