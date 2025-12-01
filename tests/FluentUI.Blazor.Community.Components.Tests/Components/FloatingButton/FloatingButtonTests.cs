using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Tests;
using Microsoft.JSInterop;
using Moq;

namespace FluentUI.Blazor.Community.Components.Tests.Components.FloatingButton;

public class FloatingButtonTests : TestBase
{
    public FloatingButtonTests()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
        Services.AddSingleton(UnitTestLibraryConfiguration);
        Services.AddFluentUIComponents();
    }

    [Fact]
    public void FluentCxFloatingButton_Default()
    {
        var cut = RenderComponent<FluentCxFloatingButton>();

        Assert.NotNull(cut.Instance);
    }

    [Theory]
    [InlineData("target", "absolute")]
    [InlineData("", "fixed")]
    public void FluentCxFloatingButton_CheckPosition_WithTarget(string target, string value)
    {
        var cut = Render(b =>
        {
            b.OpenElement(0, "div");
            b.AddAttribute(1, "style", "height: 400; position: relative");
            b.AddAttribute(2, "id", "target");

            b.OpenComponent<FluentCxFloatingButton>(3);
            b.AddComponentParameter(4, nameof(FluentCxFloatingButton.RelativeContainerId), target);
            b.CloseComponent();

            b.CloseElement();
        });

        cut.InvokeAsync(() => { });

        Assert.Contains($"position: {value}", cut.Markup);
    }

    [Theory]
    [InlineData(FloatingPosition.TopLeft, "top: 16px", "left: 16px", "")]
    [InlineData(FloatingPosition.MiddleLeft, "top: 50%", "transform: translateY(-50%)", "left: 16px")]
    [InlineData(FloatingPosition.BottomLeft, "bottom: 16px", "left: 16px", "")]
    [InlineData(FloatingPosition.TopCenter, "top: 16px", "left: 50%", "transform: translateX(-50%)")]
    [InlineData(FloatingPosition.MiddleCenter, "top: 50%", "left: 50%", "transform: translate(-50%, -50%)")]
    [InlineData(FloatingPosition.BottomCenter, "bottom: 16px", "left: 50%", "transform: translateX(-50%)")]
    [InlineData(FloatingPosition.TopRight, "top: 16px", "right: 16px", "")]
    [InlineData(FloatingPosition.MiddleRight, "top: 50%", "right: 16px", "transform: translateY(-50%)")]
    [InlineData(FloatingPosition.BottomRight, "bottom: 16px", "right: 16px", "")]
    public void FluentCxFloatingButton_AbsolutePosition(
        FloatingPosition position,
        string firstContent,
        string secondContent,
        string thirdContent)
    {
        var cut = Render(b =>
        {
            b.OpenElement(0, "div");
            b.AddAttribute(1, "style", "height: 400; position: relative");
            b.AddAttribute(2, "id", "target");

            b.OpenComponent<FluentCxFloatingButton>(3);
            b.AddComponentParameter(4, nameof(FluentCxFloatingButton.RelativeContainerId), "target");
            b.AddComponentParameter(5, nameof(FluentCxFloatingButton.Position), position);
            b.CloseComponent();

            b.CloseElement();
        });

        Assert.Contains(firstContent, cut.Markup);
        Assert.Contains(secondContent, cut.Markup);

        if (!string.IsNullOrEmpty(thirdContent))
        {
            Assert.Contains(thirdContent, cut.Markup);
        }
    }

    [Fact]
    public void Renders_WhenIsVisibleTrue()
    {
        var cut = RenderComponent<FluentCxFloatingButton>(parameters => parameters
            .Add(p => p.IsVisible, true)
        );

        // Vérifie que le bouton est rendu
        cut.Markup.Contains("fluent-button");
        cut.Markup.Contains("<style>");
    }

    [Fact]
    public void DoesNotRender_WhenIsVisibleFalse()
    {
        var cut = RenderComponent<FluentCxFloatingButton>(parameters => parameters
            .Add(p => p.IsVisible, false)
        );

        Assert.Empty(cut.Markup);
    }

    [Fact]
    public void OnClick_IsCalled()
    {
        var clicked = false;
        var cut = RenderComponent<FluentCxFloatingButton>(parameters => parameters
            .Add(p => p.IsVisible, true)
            .Add(p => p.OnClick, _ => { clicked = true; })
        );

        cut.Find("fluent-button").Click();

        Assert.True(clicked);
    }

    [Fact]
    public void Constructor_InitializesProperties()
    {
        var button = new FluentCxFloatingButton();
        Assert.NotNull(button);
        Assert.NotNull(button.AdditionalAttributes);
        Assert.True(button.AdditionalAttributes.ContainsKey("onmouseenter"));
        Assert.True(button.AdditionalAttributes.ContainsKey("tabindex"));
    }

    [Fact]
    public void OnInitialized_SetsIsFixed_WhenRelativeContainerIdIsNull()
    {
        var cut = RenderComponent<FluentCxFloatingButton>(parameters => parameters
            .Add(p => p.RelativeContainerId, null)
        );

        Assert.True(GetPrivateField<bool>(cut.Instance, "_isFixed"));
    }

    [Fact]
    public void OnInitialized_SetsIsFixed_WhenRelativeContainerIdIsNotNull()
    {
        var mockModule = JSInterop.SetupModule("./_content/FluentUI.Blazor.Community.Components/Components/FloatingButton/FluentCxFloatingButton.razor.js");
        mockModule.Setup<bool>("hasValidTarget", "container").SetResult(true);

        var cut = RenderComponent<FluentCxFloatingButton>(parameters => parameters
            .Add(p => p.RelativeContainerId, "container")
        );

        Assert.False(GetPrivateField<bool>(cut.Instance, "_isFixed"));
    }

    [Fact]
    public async Task OnMouseEnterAsync_InvokesCallback_WhenHasDelegate()
    {
        var called = false;

        var cut = RenderComponent<FluentCxFloatingButton>(parameters => parameters
            .Add(p => p.OnMouseEnter, EventCallback.Factory.Create(this, (MouseEventArgs e) => { called = true; }))
        );

        await InvokePrivateAsync(cut.Instance, "OnMouseEnterAsync", new MouseEventArgs());
        Assert.True(called);
    }

    [Fact]
    public async Task OnMouseEnterAsync_DoesNothing_WhenNoDelegate()
    {
        var button = new FluentCxFloatingButton();
        // Should not throw
        await InvokePrivateAsync(button, "OnMouseEnterAsync", new MouseEventArgs());
    }

    [Fact]
    public async Task GetIsFixedAsync_SetsIsFixed_True_WhenModuleNull()
    {
        var cut = RenderComponent<FluentCxFloatingButton>(parameters => parameters
           .Add(p => p.RelativeContainerId, "container")
       );
        SetPrivateField(cut.Instance, "_module", null);
        await InvokePrivateAsync(cut.Instance, "GetIsFixedAsync");
        Assert.True(GetPrivateField<bool>(cut.Instance, "_isFixed"));
    }

    [Fact]
    public async Task GetIsFixedAsync_SetsIsFixed_False_WhenModuleReturnsTrue()
    {
        var jsModuleMock = new Mock<IJSObjectReference>();
        jsModuleMock.Setup(m => m.InvokeAsync<bool>("hasValidTarget", It.IsAny<object[]>())).ReturnsAsync(true);

        var button = new FluentCxFloatingButton();
        SetPrivateField(button, "_module", jsModuleMock.Object);
        button.RelativeContainerId = "container";
        await InvokePrivateAsync(button, "GetIsFixedAsync");
        Assert.False(GetPrivateField<bool>(button, "_isFixed"));
    }

    [Fact]
    public async Task GetIsFixedAsync_SetsIsFixed_True_WhenModuleReturnsFalse()
    {
        var jsModuleMock = new Mock<IJSObjectReference>();
        jsModuleMock.Setup(m => m.InvokeAsync<bool>("hasValidTarget", It.IsAny<object[]>())).ReturnsAsync(false);

        var button = new FluentCxFloatingButton();
        SetPrivateField(button, "_module", jsModuleMock.Object);
        button.RelativeContainerId = "container";
        await InvokePrivateAsync(button, "GetIsFixedAsync");
        Assert.True(GetPrivateField<bool>(button, "_isFixed"));
    }

    // Helpers for private/protected access
    private static T GetPrivateField<T>(object obj, string fieldName)
    {
        var field = obj.GetType().GetField(fieldName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        return (T)field?.GetValue(obj)!;
    }

    private static void SetPrivateField(object obj, string fieldName, object? value)
    {
        var field = obj.GetType().GetField(fieldName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        field?.SetValue(obj, value);
    }

    private static async Task InvokePrivateAsync(object obj, string methodName, params object[]? parameters)
    {
        var method = obj.GetType().GetMethod(methodName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var result = method?.Invoke(obj, parameters);

        if (result is Task t)
        {
            await t;
        }
    }
}

public static class FluentCxFloatingButtonTestExtensions
{
    public static void InvokeOnInitialized(this FluentCxFloatingButton button)
    {
        var method = typeof(FluentCxFloatingButton).GetMethod("OnInitialized", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        method?.Invoke(button, null);
    }
}
