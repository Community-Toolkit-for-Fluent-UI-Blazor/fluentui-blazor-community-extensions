using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentUI.Blazor.Community.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Tests;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Audio;

public class MinimalPlayerTests : TestBase
{
    public MinimalPlayerTests()
    {
        JSInterop.Mode = Bunit.JSRuntimeMode.Loose;
        Services.AddSingleton(UnitTestLibraryConfiguration);
        Services.AddFluentUIComponents();
        Services.AddFluentCxUIComponents();
    }

    [Fact]
    public void MinimalPlayer_Rendered_HasUniqueId()
    {
        // Act
        var cut = RenderComponent<MinimalPlayer>();

        // Assert
        var instance = cut.Instance;
        Assert.StartsWith("minimal-player-", instance.Id);
        Assert.False(string.IsNullOrWhiteSpace(instance.Id));
    }

    [Fact]
    public async Task MinimalPlayer_OnPlayPause_EventCallbackIsTriggered()
    {
        // Arrange
        var callbackInvoked = false;
        var cut = RenderComponent<MinimalPlayer>(parameters => parameters
            .Add(p => p.OnPlayPause, EventCallback.Factory.Create<bool>(this, (playing) => { callbackInvoked = true; }))
        );

        // Act
        await cut.Instance.OnPlayPause.InvokeAsync(true);

        // Assert
        Assert.True(callbackInvoked);
    }
}
