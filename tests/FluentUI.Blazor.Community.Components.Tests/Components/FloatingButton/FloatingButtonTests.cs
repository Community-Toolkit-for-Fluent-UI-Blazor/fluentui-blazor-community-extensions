using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bunit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Tests;

namespace FluentUI.Blazor.Community.Components.Tests.Components.FloatingButton;

public class FloatingButtonTests : TestBase
{
    public FloatingButtonTests()
    {
        JSInterop.Mode = Bunit.JSRuntimeMode.Loose;
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
    [InlineData(FloatingPosition.MiddleCenter, "top: 50%", "left: 50%", "transform: translateX(-50%)")]
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
}
