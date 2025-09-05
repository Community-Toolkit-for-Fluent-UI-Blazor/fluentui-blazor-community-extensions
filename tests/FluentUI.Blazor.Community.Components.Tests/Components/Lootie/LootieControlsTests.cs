using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bunit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FluentUI.AspNetCore.Components.Tests;
using Microsoft.JSInterop;
using Moq;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Lootie;

public class LootieControlsTests : TestBase
{
    public LootieControlsTests()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
        Services.AddSingleton(UnitTestLibraryConfiguration);
    }

    [Fact]
    public void LootieControls_Should_Render_DefaultToolbar_When_NoChildContent()
    {
        // Act
        var cut = RenderComponent<FluentCxLootiePlayer>(p =>
        {
            p.AddChildContent<LootieControls>();
        });

        // Assert
        cut.Markup.Contains("<fluent-toolbar>");
    }

    [Fact]
    public void LootieControls_Should_Have_Id_After_Construction()
    {
        // Act
        var cut = RenderComponent<FluentCxLootiePlayer>(p =>
        {
            p.AddChildContent<LootieControls>();
        });

        // Assert
        Assert.False(string.IsNullOrWhiteSpace(cut.Instance.Id));
    }

    [Fact]
    public void OnInitialized_Throws_If_Parent_Is_Null()
    {
        Assert.Throws<InvalidOperationException>(() => RenderComponent<LootieControls>());
    }

    [Fact]
    public void OnInitialized_Sets_IsLooping_From_Parent()
    {
        var comp = RenderComponent<FluentCxLootiePlayer>(parameters => parameters
            .AddChildContent<LootieControls>(p =>
            {
            })
            .Add(p => p.Loop, true)
        );

        var lootieControls = comp.FindComponent<LootieControls>();
        var isLoopingField = typeof(LootieControls).GetField("_isLooping", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        Assert.True((bool)isLoopingField?.GetValue(lootieControls.Instance)!);
    }

    [Fact]
    public async Task PlayAsync_Calls_Parent_PlayAsync()
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

        var comp = RenderComponent<FluentCxLootiePlayer>(parameters => parameters
           .AddChildContent<LootieControls>()
       );

        var ctrl = comp.FindComponent<LootieControls>();
        Assert.NotNull(ctrl);
        await ctrl.Instance.PlayAsync();

        mockModule.VerifyInvoke("fluentcxLootiePlayer.load");
        mockModule.VerifyInvoke("fluentcxLootiePlayer.play");
    }

    [Fact]
    public async Task PauseAsync_Calls_Parent_PauseAsync()
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

        var comp = RenderComponent<FluentCxLootiePlayer>(parameters => parameters
           .AddChildContent<LootieControls>()
       );

        var ctrl = comp.FindComponent<LootieControls>();
        Assert.NotNull(ctrl);
        await ctrl.Instance.PauseAsync();

        mockModule.VerifyInvoke("fluentcxLootiePlayer.load");
        mockModule.VerifyInvoke("fluentcxLootiePlayer.pause");
    }

    [Fact]
    public async Task StopAsync_Calls_Parent_StopAsync()
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

        var comp = RenderComponent<FluentCxLootiePlayer>(parameters => parameters
           .AddChildContent<LootieControls>()
       );

        var ctrl = comp.FindComponent<LootieControls>();
        Assert.NotNull(ctrl);
        await ctrl.Instance.StopAsync();

        mockModule.VerifyInvoke("fluentcxLootiePlayer.load");
        mockModule.VerifyInvoke("fluentcxLootiePlayer.stop");
    }

    [Fact]
    public async Task SetSpeedAsync_Calls_Parent_SetSpeedAsync()
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

        var comp = RenderComponent<FluentCxLootiePlayer>(parameters => parameters
           .AddChildContent<LootieControls>()
       );

        await comp.Instance.SetSpeedAsync(2.5);

        mockModule.VerifyInvoke("fluentcxLootiePlayer.load");
        mockModule.VerifyInvoke("fluentcxLootiePlayer.setSpeed");
        Assert.Equal(2.5, comp.Instance.Speed);
    }

    [Fact]
    public async Task SetDirectionAsync_Calls_Parent_SetDirectionAsync()
    {
        var mockModule = JSInterop.SetupModule("./_content/FluentUI.Blazor.Community.Components/Components/Lootie/FluentCxLootiePlayer.razor.js");
        mockModule.SetupVoid("fluentcxLootiePlayer.setDirection", It.IsAny<string>(), It.IsAny<double>()).SetVoidResult();
        mockModule.SetupVoid("fluentcxLootiePlayer.load",
            It.IsAny<string>(),
            It.IsAny<DotNetObjectReference<FluentCxLootiePlayer>>(),
            It.IsAny<string>(),
            It.IsAny<bool>(),
            It.IsAny<bool>(),
            It.IsAny<double>(),
            It.IsAny<LootieRenderer>()).SetVoidResult();

        var comp = RenderComponent<FluentCxLootiePlayer>(parameters => parameters
           .AddChildContent<LootieControls>()
       );

        await comp.Instance.SetDirectionAsync(LootieDirection.Backward);

        mockModule.VerifyInvoke("fluentcxLootiePlayer.load");
        mockModule.VerifyInvoke("fluentcxLootiePlayer.setDirection");
    }

    [Fact]
    public async Task ToggleLoopAsync_Calls_Parent_ToggleLoopAsync_And_Updates_IsLooping()
    {
        var comp = RenderComponent<FluentCxLootiePlayer>(parameters => parameters
          .AddChildContent<LootieControls>()
        );

        var ctrl = comp.FindComponent<LootieControls>();
        Assert.NotNull(ctrl);
        await ctrl.Instance.ToggleLoopAsync(true);
        Assert.True(comp.Instance.Loop);
    }
}
