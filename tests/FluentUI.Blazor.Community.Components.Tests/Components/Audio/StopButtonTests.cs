using Bunit;
using FluentUI.Blazor.Community.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Tests;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Audio;

public class StopButtonTests : TestBase
{
    public StopButtonTests()
    {
        JSInterop.Mode = Bunit.JSRuntimeMode.Loose;
        Services.AddSingleton(UnitTestLibraryConfiguration);
        Services.AddFluentUIComponents();
        Services.AddFluentCxUIComponents();
    }

    [Fact]
    public void StopButton_OnStopAsync_InvokesCallback()
    {
        var invoked = false;
        var cut = RenderComponent<StopButton>(parameters => parameters
            .Add(p => p.OnStop, EventCallback.Factory.Create(this, () => invoked = true))
        );

        cut.InvokeAsync(() => cut.Instance.GetType().GetMethod("OnStopAsync", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .Invoke(cut.Instance, null));

        Assert.True(invoked);
    }
}
