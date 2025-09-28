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
public class AudioPlaylistItemTests : TestBase
{
    public AudioPlaylistItemTests()
    {
        JSInterop.Mode = Bunit.JSRuntimeMode.Loose;
        Services.AddSingleton(UnitTestLibraryConfiguration);
        Services.AddFluentUIComponents();
        Services.AddFluentCxUIComponents();
    }

    [Fact]
    public void AudioPlaylistItem_Rendered_HasUniqueId()
    {
        // Act
        var cut = RenderComponent<AudioPlaylistItem>();

        // Assert
        var instance = cut.Instance;
        Assert.StartsWith("audio-playlist-item-", instance.Id);
        Assert.False(string.IsNullOrWhiteSpace(instance.Id));
    }

    [Fact]
    public void AudioPlaylistItem_Track_ParameterIsSet()
    {
        // Arrange
        var track = new AudioTrackItem { Title = "Track", Artist = "Artist", Source = "track.mp3", Cover = "cover.png" };

        // Act
        var cut = RenderComponent<AudioPlaylistItem>(parameters => parameters
            .Add(p => p.Track, track)
        );

        // Assert
        Assert.Equal(track, cut.Instance.Track);
    }

    [Fact]
    public void AudioPlaylistItem_IsSelected_ParameterIsSet()
    {
        // Act
        var cut = RenderComponent<AudioPlaylistItem>(parameters => parameters
            .Add(p => p.IsSelected, true)
        );

        // Assert
        Assert.True(cut.Instance.IsSelected);
    }

    [Fact]
    public void AudioPlaylistItem_TrackIcon_ReturnsPollIcon_WhenSelected()
    {
        // Act
        var cut = RenderComponent<AudioPlaylistItem>(parameters => parameters
            .Add(p => p.IsSelected, true)
        );

        // Assert
        var iconField = cut.Instance.GetType().GetProperty("TrackIcon", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var icon = iconField?.GetValue(cut.Instance);
        Assert.NotNull(icon);
        Assert.Equal("Poll", icon?.GetType().Name);
    }

    [Fact]
    public void AudioPlaylistItem_TrackIcon_ReturnsPlayIcon_WhenNotSelected()
    {
        // Act
        var cut = RenderComponent<AudioPlaylistItem>(parameters => parameters
            .Add(p => p.IsSelected, false)
        );

        // Assert
        var iconField = cut.Instance.GetType().GetProperty("TrackIcon", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var icon = iconField?.GetValue(cut.Instance);
        Assert.NotNull(icon);
        Assert.Equal("Play", icon?.GetType().Name);
    }

    [Fact]
    public async Task AudioPlaylistItem_OnHandleClickAsync_InvokesCallback_WhenTrackAndDelegateSet()
    {
        // Arrange
        var track = new AudioTrackItem { Title = "Track", Artist = "Artist", Source = "track.mp3", Cover = "cover.png" };
        bool callbackInvoked = false;
        var cut = RenderComponent<AudioPlaylistItem>(parameters => parameters
            .Add(p => p.Track, track)
            .Add(p => p.OnSelected, EventCallback.Factory.Create<AudioTrackItem>(this, (selectedTrack) => { callbackInvoked = true; }))
        );

        // Act
        var method = cut.Instance.GetType().GetMethod("OnHandleClickAsync", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        await (Task)method!.Invoke(cut.Instance, null);

        // Assert
        Assert.True(callbackInvoked);
    }

    [Fact]
    public async Task AudioPlaylistItem_OnHandleClickAsync_DoesNotInvokeCallback_WhenTrackIsNull()
    {
        // Arrange
        bool callbackInvoked = false;
        var cut = RenderComponent<AudioPlaylistItem>(parameters => parameters
            .Add(p => p.OnSelected, EventCallback.Factory.Create<AudioTrackItem>(this, (selectedTrack) => { callbackInvoked = true; }))
        );

        // Act
        var method = cut.Instance.GetType().GetMethod("OnHandleClickAsync", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        await (Task)method!.Invoke(cut.Instance, null);

        // Assert
        Assert.False(callbackInvoked);
    }

    [Fact]
    public async Task AudioPlaylistItem_OnHandleClickAsync_DoesNotInvokeCallback_WhenDelegateNotSet()
    {
        // Arrange
        var track = new AudioTrackItem { Title = "Track", Artist = "Artist", Source = "track.mp3", Cover = "cover.png" };
        var cut = RenderComponent<AudioPlaylistItem>(parameters => parameters
            .Add(p => p.Track, track)
        );

        // Act
        var method = cut.Instance.GetType().GetMethod("OnHandleClickAsync", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        await (Task)method!.Invoke(cut.Instance, null);

        // Assert
        // No exception, no callback
    }
}
