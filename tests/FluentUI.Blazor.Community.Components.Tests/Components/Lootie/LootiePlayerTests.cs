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

namespace FluentUI.Blazor.Community.Components.Tests.Components.Lootie;

public class LootiePlayerTests : TestBase
{
    public LootiePlayerTests()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
        Services.AddSingleton(UnitTestLibraryConfiguration);
    }

    [Fact]
    public void Renders_LootieContainer_With_ChildContent()
    {
        // Arrange & Act
        var cut = RenderComponent<FluentCxLootiePlayer>(parameters => parameters
            .AddChildContent("<span>Test Content</span>")
        );

        // Assert
        cut.Markup.Contains("lootie-container");
        cut.Markup.Contains("<span>Test Content</span>");
    }

    [Fact]
    public async Task PlayAsync_InvokesJsInterop()
    {
        var mockModule = JSInterop.SetupModule("./_content/FluentUI.Blazor.Community.Components/Components/Lootie/FluentCxLootiePlayer.razor.js");
        mockModule.SetupVoid("fluentcxLootiePlayer.play", It.IsAny<string>()).SetVoidResult();
        mockModule.SetupVoid("fluentcxLootiePlayer.load",
            It.IsAny<string>(),
            It.IsAny<DotNetObjectReference<FluentCxLootiePlayer>>(),
            It.IsAny<string>(),
            It.IsAny<bool>(),
            It.IsAny<bool>(),
            It.IsAny<double>(),
            It.IsAny<LootieRenderer>()).SetVoidResult();

        var player = RenderComponent<FluentCxLootiePlayer>();

        await player.Instance.PlayAsync();

        mockModule.VerifyInvoke("fluentcxLootiePlayer.load");
        mockModule.VerifyInvoke("fluentcxLootiePlayer.play");
    }

    [Fact]
    public async Task PauseAsync_InvokesJsInterop()
    {
        var mockModule = JSInterop.SetupModule("./_content/FluentUI.Blazor.Community.Components/Components/Lootie/FluentCxLootiePlayer.razor.js");
        mockModule.SetupVoid("fluentcxLootiePlayer.pause", It.IsAny<string>()).SetVoidResult();
        mockModule.SetupVoid("fluentcxLootiePlayer.load",
            It.IsAny<string>(),
            It.IsAny<DotNetObjectReference<FluentCxLootiePlayer>>(),
            It.IsAny<string>(),
            It.IsAny<bool>(),
            It.IsAny<bool>(),
            It.IsAny<double>(),
            It.IsAny<LootieRenderer>()).SetVoidResult();

        var player = RenderComponent<FluentCxLootiePlayer>();

        await player.Instance.PauseAsync();

        mockModule.VerifyInvoke("fluentcxLootiePlayer.load");
        mockModule.VerifyInvoke("fluentcxLootiePlayer.pause");
    }

    [Fact]
    public async Task StopAsync_InvokesJsInterop()
    {
        var mockModule = JSInterop.SetupModule("./_content/FluentUI.Blazor.Community.Components/Components/Lootie/FluentCxLootiePlayer.razor.js");
        mockModule.SetupVoid("fluentcxLootiePlayer.stop", It.IsAny<string>()).SetVoidResult();
        mockModule.SetupVoid("fluentcxLootiePlayer.load",
            It.IsAny<string>(),
            It.IsAny<DotNetObjectReference<FluentCxLootiePlayer>>(),
            It.IsAny<string>(),
            It.IsAny<bool>(),
            It.IsAny<bool>(),
            It.IsAny<double>(),
            It.IsAny<LootieRenderer>()).SetVoidResult();

        var player = RenderComponent<FluentCxLootiePlayer>();

        await player.Instance.StopAsync();

        mockModule.VerifyInvoke("fluentcxLootiePlayer.load");
        mockModule.VerifyInvoke("fluentcxLootiePlayer.stop");
    }

    [Fact]
    public async Task SetSpeedAsync_InvokesJsInterop()
    {
        var mockModule = JSInterop.SetupModule("./_content/FluentUI.Blazor.Community.Components/Components/Lootie/FluentCxLootiePlayer.razor.js");
        mockModule.SetupVoid("fluentcxLootiePlayer.setSpeed", It.IsAny<string>(), It.IsAny<double>()).SetVoidResult();
        mockModule.SetupVoid("fluentcxLootiePlayer.load",
            It.IsAny<string>(),
            It.IsAny<DotNetObjectReference<FluentCxLootiePlayer>>(),
            It.IsAny<string>(),
            It.IsAny<bool>(),
            It.IsAny<bool>(),
            It.IsAny<double>(),
            It.IsAny<LootieRenderer>()).SetVoidResult();

        var player = RenderComponent<FluentCxLootiePlayer>();

        await player.Instance.SetSpeedAsync(2.5);

        mockModule.VerifyInvoke("fluentcxLootiePlayer.load");
        mockModule.VerifyInvoke("fluentcxLootiePlayer.setSpeed");
    }

    [Fact]
    public async Task SetDirectionAsync_InvokesJsInterop()
    {
        var mockModule = JSInterop.SetupModule("./_content/FluentUI.Blazor.Community.Components/Components/Lootie/FluentCxLootiePlayer.razor.js");
        mockModule.SetupVoid("fluentcxLootiePlayer.setDirection", It.IsAny<string>(), It.IsAny<LootieDirection>()).SetVoidResult();
        mockModule.SetupVoid("fluentcxLootiePlayer.load",
            It.IsAny<string>(),
            It.IsAny<DotNetObjectReference<FluentCxLootiePlayer>>(),
            It.IsAny<string>(),
            It.IsAny<bool>(),
            It.IsAny<bool>(),
            It.IsAny<double>(),
            It.IsAny<LootieRenderer>()).SetVoidResult();

        var player = RenderComponent<FluentCxLootiePlayer>();

        await player.Instance.SetDirectionAsync(LootieDirection.Backward);

        mockModule.VerifyInvoke("fluentcxLootiePlayer.load");
        mockModule.VerifyInvoke("fluentcxLootiePlayer.setDirection");
    }

    [Fact]
    public async Task PlaySegments_InvokesJsInterop()
    {
        var mockModule = JSInterop.SetupModule("./_content/FluentUI.Blazor.Community.Components/Components/Lootie/FluentCxLootiePlayer.razor.js");
        mockModule.SetupVoid("fluentcxLootiePlayer.playSegments",
            It.IsAny<string>(),
            It.IsAny<int>(),
            It.IsAny<int>(),
            It.IsAny<bool>()).SetVoidResult();

        mockModule.SetupVoid("fluentcxLootiePlayer.load",
            It.IsAny<string>(),
            It.IsAny<DotNetObjectReference<FluentCxLootiePlayer>>(),
            It.IsAny<string>(),
            It.IsAny<bool>(),
            It.IsAny<bool>(),
            It.IsAny<double>(),
            It.IsAny<LootieRenderer>()).SetVoidResult();

        var player = RenderComponent<FluentCxLootiePlayer>();

        await player.Instance.PlaySegments(2, 6, true);

        mockModule.VerifyInvoke("fluentcxLootiePlayer.load");
        mockModule.VerifyInvoke("fluentcxLootiePlayer.playSegments");
    }

    [Fact]
    public async Task ToggleLoopAsync_UpdatesLoopAndTriggersStateHasChanged()
    {
        var player = RenderComponent<FluentCxLootiePlayer>();

        await player.Instance.ToggleLoopAsync(false);

        Assert.True((bool)player.Instance.GetType().GetField("_hasPropertyChanged", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            ?.GetValue(player.Instance)!);

        Assert.False(player.Instance.Loop);
    }

    [Fact]
    public async Task SetWidth()
    {
        var player = RenderComponent<FluentCxLootiePlayer>(p => p.Add(p => p.Width, "50px"));

        Assert.Contains("width: 50px", player.Markup);
    }

    [Fact]
    public async Task SetHeight()
    {
        var player = RenderComponent<FluentCxLootiePlayer>(p => p.Add(p => p.Height, "80px"));

        Assert.Contains("height: 80px", player.Markup);
    }

    [Fact]
    public async Task DisposeAsync_DisposesModule()
    {
        var mockModule = JSInterop.SetupModule("./_content/FluentUI.Blazor.Community.Components/Components/Lootie/FluentCxLootiePlayer.razor.js");
        mockModule.SetupVoid("fluentcxLootiePlayer.dispose",
            It.IsAny<string>()).SetVoidResult();

        var player = RenderComponent<FluentCxLootiePlayer>();

        await player.Instance.DisposeAsync();

        mockModule.VerifyInvoke("fluentcxLootiePlayer.dispose");
    }

    [Fact]
    public async Task NotifyCompleteAsync_InvokesCallback()
    {
        var called = false;

        var player = RenderComponent<FluentCxLootiePlayer>(p =>
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

        var player = RenderComponent<FluentCxLootiePlayer>(p =>
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

        var player = RenderComponent<FluentCxLootiePlayer>(p =>
        {
            p.Add(a => a.OnEnterFrame, EventCallback.Factory.Create(this, () => called = true));
        });

        await player.Instance.NotifyEnterFrameAsync();

        Assert.True(called);
    }
}
