using Bunit;
using FluentUI.Blazor.Community.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Tests;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Audio;

public class RepeatButtonTests : TestBase
{
    public RepeatButtonTests()
    {
        JSInterop.Mode = Bunit.JSRuntimeMode.Loose;
        Services.AddSingleton(UnitTestLibraryConfiguration);
        Services.AddFluentUIComponents();
        Services.AddFluentCxUIComponents();
    }

    [Fact]
    public void RepeatButton_OnChangeRepeatModeAsync_CyclesModeAndInvokesCallback()
    {
        AudioRepeatMode? mode = null;
        var cut = RenderComponent<RepeatButton>(parameters => parameters
            .Add(p => p.OnRepeatModeChanged, EventCallback.Factory.Create<AudioRepeatMode>(this, m => mode = m))
        );

        var method = cut.Instance.GetType().GetMethod("OnChangeRepeatModeAsync", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        cut.InvokeAsync(() => method.Invoke(cut.Instance, null));
        Assert.NotNull(mode);

        var initialMode = mode.Value;
        cut.InvokeAsync(() => method.Invoke(cut.Instance, null));
        Assert.NotEqual(initialMode, mode.Value);
    }
}
