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
public class AudioPlaylistTests : TestBase
{
    public AudioPlaylistTests()
    {
        JSInterop.Mode = Bunit.JSRuntimeMode.Loose;
        Services.AddSingleton(UnitTestLibraryConfiguration);
        Services.AddFluentUIComponents();
        Services.AddFluentCxUIComponents();
    }

    [Fact]
    public void AudioPlaylist_Rendered_HasUniqueId()
    {
        // Act
        var cut = RenderComponent<AudioPlaylist>();

        // Assert
        var instance = cut.Instance;
        Assert.False(string.IsNullOrWhiteSpace(instance.Id));
    }

    [Fact]
    public void AudioPlaylist_Playlist_ParameterIsSet()
    {
        // Arrange
        var track1 = new AudioTrackItem { Title = "Track 1", Artist = "Artist 1", Source = "track1.mp3", Cover = "cover1.png" };
        var track2 = new AudioTrackItem { Title = "Track 2", Artist = "Artist 2", Source = "track2.mp3", Cover = "cover2.png" };
        var playlist = new List<AudioTrackItem> { track1, track2 };

        // Act
        var cut = RenderComponent<AudioPlaylist>(parameters => parameters
            .Add(p => p.Playlist, playlist)
        );

        // Assert
        Assert.Equal(playlist, cut.Instance.Playlist);
    }

    [Fact]
    public void AudioPlaylist_CurrentTrack_ParameterIsSet()
    {
        // Arrange
        var track = new AudioTrackItem { Title = "Current", Artist = "Artist", Source = "current.mp3", Cover = "cover.png" };

        // Act
        var cut = RenderComponent<AudioPlaylist>(parameters => parameters
            .Add(p => p.CurrentTrack, track)
        );

        // Assert
        Assert.Equal(track, cut.Instance.CurrentTrack);
    }

    [Fact]
    public async Task AudioPlaylist_OnTrackSelected_EventCallbackIsTriggered()
    {
        // Arrange
        var track = new AudioTrackItem { Title = "Selected", Artist = "Artist", Source = "selected.mp3", Cover = "cover.png" };
        bool callbackInvoked = false;
        var cut = RenderComponent<AudioPlaylist>(parameters => parameters
            .Add(p => p.OnTrackSelected, EventCallback.Factory.Create<AudioTrackItem>(this, (selectedTrack) => { callbackInvoked = true; }))
        );

        // Act
        await cut.Instance.OnTrackSelected.InvokeAsync(track);

        // Assert
        Assert.True(callbackInvoked);
    }
}
