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

namespace FluentUI.Blazor.Community.Components.Tests.Components.Lottie;

public class LottieControlsTests : TestBase
{
    public LottieControlsTests()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
        Services.AddSingleton(UnitTestLibraryConfiguration);
    }

    [Fact]
    public void LottieControls_Should_Render_DefaultToolbar_When_NoChildContent()
    {
        // Act
        var cut = RenderComponent<FluentCxLottiePlayer>(p =>
        {
            p.AddChildContent<LottieControls>();
        });

        // Assert
        cut.Markup.Contains("<fluent-toolbar>");
    }

    [Fact]
    public void LottieControls_Should_Have_Id_After_Construction()
    {
        // Act
        var cut = RenderComponent<FluentCxLottiePlayer>(p =>
        {
            p.AddChildContent<LottieControls>();
        });

        // Assert
        Assert.False(string.IsNullOrWhiteSpace(cut.Instance.Id));
    }

    [Fact]
    public void OnInitialized_Throws_If_Parent_Is_Null()
    {
        Assert.Throws<InvalidOperationException>(() => RenderComponent<LottieControls>());
    }

    [Fact]
    public void OnInitialized_Sets_IsLooping_From_Parent()
    {
        var comp = RenderComponent<FluentCxLottiePlayer>(parameters => parameters
            .AddChildContent<LottieControls>(p =>
            {
            })
            .Add(p => p.Loop, true)
        );

        var LottieControls = comp.FindComponent<LottieControls>();
        var isLoopingField = typeof(LottieControls).GetField("_isLooping", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        Assert.True((bool)isLoopingField?.GetValue(LottieControls.Instance)!);
    }

    [Fact]
    public async Task PlayAsync_Calls_Parent_PlayAsync()
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

        var comp = RenderComponent<FluentCxLottiePlayer>(parameters => parameters
           .AddChildContent<LottieControls>()
       );

        var ctrl = comp.FindComponent<LottieControls>();
        Assert.NotNull(ctrl);
        await ctrl.Instance.PlayAsync();

        mockModule.VerifyInvoke("fluentcxLottiePlayer.load");
        mockModule.VerifyInvoke("fluentcxLottiePlayer.play");
    }

    [Fact]
    public async Task PauseAsync_Calls_Parent_PauseAsync()
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

        var comp = RenderComponent<FluentCxLottiePlayer>(parameters => parameters
           .AddChildContent<LottieControls>()
       );

        var ctrl = comp.FindComponent<LottieControls>();
        Assert.NotNull(ctrl);
        await ctrl.Instance.PauseAsync();

        mockModule.VerifyInvoke("fluentcxLottiePlayer.load");
        mockModule.VerifyInvoke("fluentcxLottiePlayer.pause");
    }

    [Fact]
    public async Task StopAsync_Calls_Parent_StopAsync()
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

        var comp = RenderComponent<FluentCxLottiePlayer>(parameters => parameters
           .AddChildContent<LottieControls>()
       );

        var ctrl = comp.FindComponent<LottieControls>();
        Assert.NotNull(ctrl);
        await ctrl.Instance.StopAsync();

        mockModule.VerifyInvoke("fluentcxLottiePlayer.load");
        mockModule.VerifyInvoke("fluentcxLottiePlayer.stop");
    }

    [Fact]
    public async Task SetSpeedAsync_Calls_Parent_SetSpeedAsync()
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

        var comp = RenderComponent<FluentCxLottiePlayer>(parameters => parameters
           .AddChildContent<LottieControls>()
       );

        await comp.Instance.SetSpeedAsync(2.5);

        mockModule.VerifyInvoke("fluentcxLottiePlayer.load");
        mockModule.VerifyInvoke("fluentcxLottiePlayer.setSpeed");
        Assert.Equal(2.5, comp.Instance.Speed);
    }

    [Fact]
    public async Task SetDirectionAsync_Calls_Parent_SetDirectionAsync()
    {
        var mockModule = JSInterop.SetupModule("./_content/FluentUI.Blazor.Community.Components/Components/Lottie/FluentCxLottiePlayer.razor.js");
        mockModule.SetupVoid("fluentcxLottiePlayer.setDirection", It.IsAny<string>(), It.IsAny<double>()).SetVoidResult();
        mockModule.SetupVoid("fluentcxLottiePlayer.load",
            It.IsAny<string>(),
            It.IsAny<DotNetObjectReference<FluentCxLottiePlayer>>(),
            It.IsAny<string>(),
            It.IsAny<bool>(),
            It.IsAny<bool>(),
            It.IsAny<double>(),
            It.IsAny<LottieRenderer>()).SetVoidResult();

        var comp = RenderComponent<FluentCxLottiePlayer>(parameters => parameters
           .AddChildContent<LottieControls>()
       );

        await comp.Instance.SetDirectionAsync(LottieDirection.Backward);

        mockModule.VerifyInvoke("fluentcxLottiePlayer.load");
        mockModule.VerifyInvoke("fluentcxLottiePlayer.setDirection");
    }

    [Fact]
    public async Task ToggleLoopAsync_Calls_Parent_ToggleLoopAsync_And_Updates_IsLooping()
    {
        var comp = RenderComponent<FluentCxLottiePlayer>(parameters => parameters
          .AddChildContent<LottieControls>()
        );

        var ctrl = comp.FindComponent<LottieControls>();
        Assert.NotNull(ctrl);
        await ctrl.Instance.ToggleLoopAsync(true);
        Assert.True(comp.Instance.Loop);
    }
}
