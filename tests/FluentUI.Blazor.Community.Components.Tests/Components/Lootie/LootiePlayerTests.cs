using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FluentUI.AspNetCore.Components.Tests;
using Microsoft.JSInterop;
using Moq;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Lottie;

public class LottiePlayerTests : TestBase
{
    public LottiePlayerTests()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
        Services.AddSingleton(UnitTestLibraryConfiguration);
    }

    [Fact]
    public void Renders_LottieContainer_With_ChildContent()
    {
        // Arrange & Act
        var cut = RenderComponent<FluentCxLottiePlayer>(parameters => parameters
            .AddChildContent("<span>Test Content</span>")
        );

        // Assert
        cut.Markup.Contains("Lottie-container");
        cut.Markup.Contains("<span>Test Content</span>");
    }

    [Fact]
    public async Task PlayAsync_InvokesJsInterop()
    {
        var mockModule = JSInterop.SetupModule("./_content/FluentUI.Blazor.Community.Components/Components/Lottie/FluentCxLottiePlayer.razor.js");
        mockModule.SetupVoid("fluentcxLottiePlayer.play", It.IsAny<string>()).SetVoidResult();
        mockModule.SetupVoid("fluentcxLottiePlayer.load",
            It.IsAny<string>(),
            It.IsAny<DotNetObjectReference<FluentCxLottiePlayer>>(),
            It.IsAny<string>(),
            It.IsAny<bool>(),
            It.IsAny<bool>(),
            It.IsAny<double>(),
            It.IsAny<LottieRenderer>()).SetVoidResult();

        var player = RenderComponent<FluentCxLottiePlayer>();

        await player.Instance.PlayAsync();

        mockModule.VerifyInvoke("fluentcxLottiePlayer.load");
        mockModule.VerifyInvoke("fluentcxLottiePlayer.play");
    }

    [Fact]
    public async Task PauseAsync_InvokesJsInterop()
    {
        var mockModule = JSInterop.SetupModule("./_content/FluentUI.Blazor.Community.Components/Components/Lottie/FluentCxLottiePlayer.razor.js");
        mockModule.SetupVoid("fluentcxLottiePlayer.pause", It.IsAny<string>()).SetVoidResult();
        mockModule.SetupVoid("fluentcxLottiePlayer.load",
            It.IsAny<string>(),
            It.IsAny<DotNetObjectReference<FluentCxLottiePlayer>>(),
            It.IsAny<string>(),
            It.IsAny<bool>(),
            It.IsAny<bool>(),
            It.IsAny<double>(),
            It.IsAny<LottieRenderer>()).SetVoidResult();

        var player = RenderComponent<FluentCxLottiePlayer>();

        await player.Instance.PauseAsync();

        mockModule.VerifyInvoke("fluentcxLottiePlayer.load");
        mockModule.VerifyInvoke("fluentcxLottiePlayer.pause");
    }

    [Fact]
    public async Task StopAsync_InvokesJsInterop()
    {
        var mockModule = JSInterop.SetupModule("./_content/FluentUI.Blazor.Community.Components/Components/Lottie/FluentCxLottiePlayer.razor.js");
        mockModule.SetupVoid("fluentcxLottiePlayer.stop", It.IsAny<string>()).SetVoidResult();
        mockModule.SetupVoid("fluentcxLottiePlayer.load",
            It.IsAny<string>(),
            It.IsAny<DotNetObjectReference<FluentCxLottiePlayer>>(),
            It.IsAny<string>(),
            It.IsAny<bool>(),
            It.IsAny<bool>(),
            It.IsAny<double>(),
            It.IsAny<LottieRenderer>()).SetVoidResult();

        var player = RenderComponent<FluentCxLottiePlayer>();

        await player.Instance.StopAsync();

        mockModule.VerifyInvoke("fluentcxLottiePlayer.load");
        mockModule.VerifyInvoke("fluentcxLottiePlayer.stop");
    }

    [Fact]
    public async Task SetSpeedAsync_InvokesJsInterop()
    {
        var mockModule = JSInterop.SetupModule("./_content/FluentUI.Blazor.Community.Components/Components/Lottie/FluentCxLottiePlayer.razor.js");
        mockModule.SetupVoid("fluentcxLottiePlayer.setSpeed", It.IsAny<string>(), It.IsAny<double>()).SetVoidResult();
        mockModule.SetupVoid("fluentcxLottiePlayer.load",
            It.IsAny<string>(),
            It.IsAny<DotNetObjectReference<FluentCxLottiePlayer>>(),
            It.IsAny<string>(),
            It.IsAny<bool>(),
            It.IsAny<bool>(),
            It.IsAny<double>(),
            It.IsAny<LottieRenderer>()).SetVoidResult();

        var player = RenderComponent<FluentCxLottiePlayer>();

        await player.Instance.SetSpeedAsync(2.5);

        mockModule.VerifyInvoke("fluentcxLottiePlayer.load");
        mockModule.VerifyInvoke("fluentcxLottiePlayer.setSpeed");
    }

    [Fact]
    public async Task SetDirectionAsync_InvokesJsInterop()
    {
        var mockModule = JSInterop.SetupModule("./_content/FluentUI.Blazor.Community.Components/Components/Lottie/FluentCxLottiePlayer.razor.js");
        mockModule.SetupVoid("fluentcxLottiePlayer.setDirection", It.IsAny<string>(), It.IsAny<LottieDirection>()).SetVoidResult();
        mockModule.SetupVoid("fluentcxLottiePlayer.load",
            It.IsAny<string>(),
            It.IsAny<DotNetObjectReference<FluentCxLottiePlayer>>(),
            It.IsAny<string>(),
            It.IsAny<bool>(),
            It.IsAny<bool>(),
            It.IsAny<double>(),
            It.IsAny<LottieRenderer>()).SetVoidResult();

        var player = RenderComponent<FluentCxLottiePlayer>();

        await player.Instance.SetDirectionAsync(LottieDirection.Backward);

        mockModule.VerifyInvoke("fluentcxLottiePlayer.load");
        mockModule.VerifyInvoke("fluentcxLottiePlayer.setDirection");
    }

    [Fact]
    public async Task PlaySegments_InvokesJsInterop()
    {
        var mockModule = JSInterop.SetupModule("./_content/FluentUI.Blazor.Community.Components/Components/Lottie/FluentCxLottiePlayer.razor.js");
        mockModule.SetupVoid("fluentcxLottiePlayer.playSegments",
            It.IsAny<string>(),
            It.IsAny<int>(),
            It.IsAny<int>(),
            It.IsAny<bool>()).SetVoidResult();

        mockModule.SetupVoid("fluentcxLottiePlayer.load",
            It.IsAny<string>(),
            It.IsAny<DotNetObjectReference<FluentCxLottiePlayer>>(),
            It.IsAny<string>(),
            It.IsAny<bool>(),
            It.IsAny<bool>(),
            It.IsAny<double>(),
            It.IsAny<LottieRenderer>()).SetVoidResult();

        var player = RenderComponent<FluentCxLottiePlayer>();

        await player.Instance.PlaySegments(2, 6, true);

        mockModule.VerifyInvoke("fluentcxLottiePlayer.load");
        mockModule.VerifyInvoke("fluentcxLottiePlayer.playSegments");
    }

    [Fact]
    public async Task ToggleLoopAsync_UpdatesLoopAndTriggersStateHasChanged()
    {
        var player = RenderComponent<FluentCxLottiePlayer>();

        await player.Instance.ToggleLoopAsync(false);

        Assert.True((bool)player.Instance.GetType().GetField("_hasPropertyChanged", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            ?.GetValue(player.Instance)!);

        Assert.False(player.Instance.Loop);
    }

    [Fact]
    public async Task SetWidth()
    {
        var player = RenderComponent<FluentCxLottiePlayer>(p => p.Add(p => p.Width, "50px"));

        Assert.Contains("width: 50px", player.Markup);
    }

    [Fact]
    public async Task SetHeight()
    {
        var player = RenderComponent<FluentCxLottiePlayer>(p => p.Add(p => p.Height, "80px"));

        Assert.Contains("height: 80px", player.Markup);
    }

    [Fact]
    public async Task DisposeAsync_DisposesModule()
    {
        var mockModule = JSInterop.SetupModule("./_content/FluentUI.Blazor.Community.Components/Components/Lottie/FluentCxLottiePlayer.razor.js");
        mockModule.SetupVoid("fluentcxLottiePlayer.dispose",
            It.IsAny<string>()).SetVoidResult();

        var player = RenderComponent<FluentCxLottiePlayer>();

        await player.Instance.DisposeAsync();

        mockModule.VerifyInvoke("fluentcxLottiePlayer.dispose");
    }

    [Fact]
    public async Task NotifyCompleteAsync_InvokesCallback()
    {
        var called = false;

        var player = RenderComponent<FluentCxLottiePlayer>(p =>
        {
            p.Add(a => a.OnComplete, EventCallback.Factory.Create(this, () => called = true));
        });

        await player.Instance.NotifyCompleteAsync();

        Assert.True(called);
    }

    [Fact]
    public async Task NotifyLoopAsync_InvokesCallback()
    {
        var called = false;

        var player = RenderComponent<FluentCxLottiePlayer>(p =>
        {
            p.Add(a => a.OnLoop, EventCallback.Factory.Create(this, () => called = true));
        });

        await player.Instance.NotifyLoopAsync();

        Assert.True(called);
    }

    [Fact]
    public async Task NotifyEnterFrameAsync_InvokesCallback()
    {
        var called = false;

        var player = RenderComponent<FluentCxLottiePlayer>(p =>
        {
            p.Add(a => a.OnEnterFrame, EventCallback.Factory.Create(this, () => called = true));
        });

        await player.Instance.NotifyEnterFrameAsync();

        Assert.True(called);
    }
}
