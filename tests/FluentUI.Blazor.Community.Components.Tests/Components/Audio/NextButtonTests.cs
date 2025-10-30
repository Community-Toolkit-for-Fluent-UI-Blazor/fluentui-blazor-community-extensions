using Bunit;
using FluentUI.Blazor.Community.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Tests;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Audio;

public class NextButtonTests : TestBase
{
    public NextButtonTests()
    {
        JSInterop.Mode = Bunit.JSRuntimeMode.Loose;
        Services.AddSingleton(UnitTestLibraryConfiguration);
        Services.AddFluentUIComponents();
        Services.AddFluentCxUIComponents();
    }

    [Fact]
    public void NextButton_OnNextAsync_InvokesCallback()
    {
        var invoked = false;
        var cut = RenderComponent<NextButton>(parameters => parameters
            .Add(p => p.OnNext, EventCallback.Factory.Create(this, () => invoked = true))
        );

        cut.InvokeAsync(() => cut.Instance.GetType().GetMethod("OnClickAsync", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .Invoke(cut.Instance, null));

        Assert.True(invoked);
    }
}
