using Bunit;
using FluentUI.Blazor.Community.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Tests;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Audio;

public class PreviousButtonTests : TestBase
{
    public PreviousButtonTests()
    {
        JSInterop.Mode = Bunit.JSRuntimeMode.Loose;
        Services.AddSingleton(UnitTestLibraryConfiguration);
        Services.AddFluentUIComponents();
        Services.AddFluentCxUIComponents();
    }

    [Fact]
    public void PreviousButton_OnPreviousAsync_InvokesCallback()
    {
        bool invoked = false;
        var cut = RenderComponent<PreviousButton>(parameters => parameters
            .Add(p => p.OnPrevious, EventCallback.Factory.Create(this, () => invoked = true))
        );

        cut.InvokeAsync(() => cut.Instance.GetType().GetMethod("OnPreviousAsync", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .Invoke(cut.Instance, null));

        Assert.True(invoked);
    }
}
