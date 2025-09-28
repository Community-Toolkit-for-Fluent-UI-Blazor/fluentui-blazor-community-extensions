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

public class CompactPlayerTests : TestBase
{
    public CompactPlayerTests()
    {
        JSInterop.Mode = Bunit.JSRuntimeMode.Loose;
        Services.AddSingleton(UnitTestLibraryConfiguration);
        Services.AddFluentUIComponents();
        Services.AddFluentCxUIComponents();
    }

    [Fact]
    public void CompactPlayer_Rendered_HasUniqueId()
    {
        // Act
        var cut = RenderComponent<CompactPlayer>();

        // Assert
        var instance = cut.Instance;
        Assert.StartsWith("compact-player-", instance.Id);
        Assert.False(string.IsNullOrWhiteSpace(instance.Id));
    }

    [Fact]
    public void CompactPlayer_CurrentTrack_ParameterIsSet()
    {
        // Arrange
        var track = new AudioTrackItem { Title = "Test", Artist = "Art", Source = "src.mp3", Cover = "cover.png" };

        // Act
        var cut = RenderComponent<CompactPlayer>(parameters => parameters
            .Add(p => p.CurrentTrack, track)
        );

        // Assert
        Assert.Equal(track, cut.Instance.CurrentTrack);
    }

    [Fact]
    public async Task CompactPlayer_OnPlayPause_EventCallbackIsTriggered()
    {
        // Arrange
        var callbackInvoked = false;
        var cut = RenderComponent<CompactPlayer>(parameters => parameters
            .Add(p => p.OnPlayPauseChanged, EventCallback.Factory.Create<bool>(this, (playing) => { callbackInvoked = true; }))
        );

        // Act
        await cut.Instance.OnPlayPauseChanged.InvokeAsync(true);

        // Assert
        Assert.True(callbackInvoked);
    }
}
