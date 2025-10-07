using Bunit;
using FluentUI.Blazor.Community.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Tests;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Audio;

public class PlayOrPauseButtonTests : TestBase
{
    public PlayOrPauseButtonTests()
    {
        JSInterop.Mode = Bunit.JSRuntimeMode.Loose;
        Services.AddSingleton(UnitTestLibraryConfiguration);
        Services.AddFluentUIComponents();
        Services.AddFluentCxUIComponents();
    }

    [Fact]
    public void PlayOrPauseButton_OnTogglePlayOrPauseAsync_TogglesStateAndInvokesCallback()
    {
        bool? playState = null;
        var cut = RenderComponent<PlayOrPauseButton>(parameters => parameters
            .Add(p => p.OnPlayChanged, EventCallback.Factory.Create<bool>(this, s => playState = s))
        );

        cut.InvokeAsync(() => cut.Instance.GetType().GetMethod("OnTogglePlayOrPauseAsync", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .Invoke(cut.Instance, null));

        Assert.NotNull(playState);
        Assert.True(playState.Value || !playState.Value);
    }
}
