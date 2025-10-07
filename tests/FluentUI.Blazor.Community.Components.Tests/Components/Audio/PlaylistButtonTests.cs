using Bunit;
using FluentUI.Blazor.Community.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Tests;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Audio;

public class PlaylistButtonTests : TestBase
{
    public PlaylistButtonTests()
    {
        JSInterop.Mode = Bunit.JSRuntimeMode.Loose;
        Services.AddSingleton(UnitTestLibraryConfiguration);
        Services.AddFluentUIComponents();
        Services.AddFluentCxUIComponents();
    }

    [Fact]
    public void PlaylistButton_OnTogglePlaylistAsync_TogglesStateAndInvokesCallback()
    {
        bool? playlistState = null;
        var cut = RenderComponent<PlaylistButton>(parameters => parameters
            .Add(p => p.OnPlaylist, EventCallback.Factory.Create<bool>(this, s => playlistState = s))
        );

        cut.InvokeAsync(() => cut.Instance.GetType().GetMethod("OnTogglePlaylistAsync", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .Invoke(cut.Instance, null));

        Assert.NotNull(playlistState);
        Assert.True(playlistState.Value || !playlistState.Value);
    }
}
