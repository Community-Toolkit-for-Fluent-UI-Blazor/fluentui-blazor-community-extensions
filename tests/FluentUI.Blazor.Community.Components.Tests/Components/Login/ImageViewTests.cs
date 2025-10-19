using System;
using System.Reflection;
using FluentUI.Blazor.Community.Components;
using Microsoft.FluentUI.AspNetCore.Components.Tests;
using Xunit;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Login;

public class ImageViewTests : TestBase
{
    [Fact]
    public void OnInitialized_Throws_When_Parent_Is_Null()
    {
        var image = new ImageView();

        var method = image.GetType().GetMethod("OnInitialized", BindingFlags.Instance | BindingFlags.NonPublic);

        var ex = Assert.Throws<TargetInvocationException>(() => method!.Invoke(image, null));
        Assert.IsType<InvalidOperationException>(ex.InnerException);
    }

    [Fact]
    public void Dispose_DoesNotThrow_When_Parent_Is_Null()
    {
        var image = new ImageView();

        // Should not throw even if Parent is null
        image.Dispose();
    }
}
