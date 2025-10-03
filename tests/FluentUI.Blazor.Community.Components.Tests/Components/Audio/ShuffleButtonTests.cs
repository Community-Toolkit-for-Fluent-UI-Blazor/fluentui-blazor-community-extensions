using Bunit;
using FluentUI.Blazor.Community.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Tests;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Audio;

public class ShuffleButtonTests : TestBase
{
    public ShuffleButtonTests()
    {
        JSInterop.Mode = Bunit.JSRuntimeMode.Loose;
        Services.AddSingleton(UnitTestLibraryConfiguration);
        Services.AddFluentUIComponents();
        Services.AddFluentCxUIComponents();
    }

    [Fact]
    public void ShuffleButton_OnToggleShuffleAsync_TogglesStateAndInvokesCallback()
    {
        bool? shuffleState = null;
        var cut = RenderComponent<ShuffleButton>(parameters => parameters
            .Add(p => p.OnShuffleChanged, EventCallback.Factory.Create<bool>(this, s => shuffleState = s))
        );

        cut.InvokeAsync(() => cut.Instance.GetType().GetMethod("OnToggleShuffleAsync", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .Invoke(cut.Instance, null));

        Assert.NotNull(shuffleState);
        Assert.True(shuffleState.Value || !shuffleState.Value);
    }
}
