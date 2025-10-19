using System;
using FluentUI.Blazor.Community.Components;
using Microsoft.FluentUI.AspNetCore.Components.Tests;
using Xunit;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Login;

public class LoginLayoutOptionsTests
{
    [Fact]
    public void Default_Values_Are_Correct()
    {
        var opt = new LoginLayoutOptions();

        Assert.Equal(0.0, opt.StartOpacity);
        Assert.Equal(1.0, opt.EndOpacity);
        Assert.Equal(TimeSpan.FromMilliseconds(500), opt.AnimationDuration);
        Assert.Equal(SlideDirection.Left, opt.Direction);
    }

    [Fact]
    public void Can_Create_With_Custom_Values()
    {
        var custom = new LoginLayoutOptions
        {
            StartOpacity = 0.2,
            EndOpacity = 0.8,
            AnimationDuration = TimeSpan.FromSeconds(1),
            Direction = SlideDirection.Right
        };

        Assert.Equal(0.2, custom.StartOpacity);
        Assert.Equal(0.8, custom.EndOpacity);
        Assert.Equal(TimeSpan.FromSeconds(1), custom.AnimationDuration);
        Assert.Equal(SlideDirection.Right, custom.Direction);
    }
}
