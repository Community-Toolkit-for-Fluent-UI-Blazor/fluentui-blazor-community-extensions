using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bunit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Tests;

namespace FluentUI.Blazor.Community.Components.Tests.Components;

public class SleekDialItemViewTests
    : TestBase
{
    public SleekDialItemViewTests()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
        Services.AddSingleton(UnitTestLibraryConfiguration);
        Services.AddFluentUIComponents();
    }

    [Fact]
    public void OnInitialized_ThrowsIfItemNull()
    {
        var parent = new FluentCxSleekDial();
        var ex = Assert.Throws<InvalidOperationException>(() => RenderComponent<SleekDialItemView>(parameters => parameters
            .AddCascadingValue(parent)
        ));
        Assert.Contains("The Item parameter must be set", ex.Message);
    }

    [Fact]
    public void OnInitialized_ThrowsIfParentNull()
    {
        var item = new SleekDialItem();

        var ex = Assert.Throws<InvalidOperationException>(() => RenderComponent<SleekDialItemView>(parameters => parameters
            .Add(p => p.Item, item)
        ));
        Assert.Contains("A parent of type FluentCxSleekDial must be set.", ex.Message);
    }

    [Fact]
    public void OnInitialized_SubscribesToRadialSettingsChanged()
    {
        var parent = new FluentCxSleekDial();
        var item = new SleekDialItem();
        var cut = RenderComponent<SleekDialItemView>(parameters => parameters
            .AddCascadingValue(parent)
            .Add(p => p.Item, item)
        );

        Assert.Contains("<li", cut.Markup);
    }

    [Fact]
    public void Dispose_UnsubscribesFromRadialSettingsChanged()
    {
        var parent = new FluentCxSleekDial();
        var item = new SleekDialItem();
        var cut = RenderComponent<SleekDialItemView>(parameters => parameters
            .AddCascadingValue(parent)
            .Add(p => p.Item, item)
        );

        cut.Instance.Dispose();
    }

    [Fact]
    public void UpdateAdditionalAttributes_AddsTabIndex()
    {
        var parent = new FluentCxSleekDial();
        var item = new SleekDialItem();
        var cut = RenderComponent<SleekDialItemView>(parameters => parameters
            .AddCascadingValue(parent)
            .Add(p => p.Item, item)
        );

        Assert.True(cut.Instance.AdditionalAttributes.ContainsKey("tabindex"));
        Assert.Equal(1, cut.Instance.AdditionalAttributes["tabindex"]);
    }

    [Fact]
    public void GetAngle_ReturnsEmptyIfNulls()
    {
        var parent = new FluentCxSleekDial();
        parent.Mode = SleekDialMode.Radial;

        var item = new SleekDialItem();
        var cut = RenderComponent<SleekDialItemView>(parameters => parameters
            .AddCascadingValue(parent)
            .Add(p => p.Item, item)
        );

        Assert.Contains("--sleekdial-radial-angle:;", cut.Markup);
    }

    [Fact]
    public void InternalClass_ReturnsCssString()
    {
        var parent = new FluentCxSleekDial { Mode = SleekDialMode.Radial, Position = FloatingPosition.TopCenter };
        var item = new SleekDialItem();
        var cut = RenderComponent<SleekDialItemView>(parameters => parameters
            .AddCascadingValue(parent)
            .Add(p => p.Item, item)
        );

        Assert.Contains("sleekdialitem-radial", cut.Markup);
    }

    [Fact]
    public void InternalStyle_ReturnsStyleString()
    {
        var parent = new FluentCxSleekDial { Mode = SleekDialMode.Radial };
        var item = new SleekDialItem();
        var cut = RenderComponent<SleekDialItemView>(parameters => parameters
            .AddCascadingValue(parent)
            .Add(p => p.Item, item)
        );

        Assert.Contains("display", cut.Markup);
    }

    [Fact]
    public void InternalButtonStyle_ReturnsButtonStyleString()
    {
        var parent = new FluentCxSleekDial { Mode = SleekDialMode.Linear };
        var item = new SleekDialItem();
        var cut = RenderComponent<SleekDialItemView>(parameters => parameters
            .AddCascadingValue(parent)
            .Add(p => p.Item, item)
        );

        Assert.Contains("border-radius", cut.Markup);
    }
}
