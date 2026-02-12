using System;
using System.Collections.Generic;
using System.Reflection;
using FluentUI.Blazor.Community.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Xunit;

namespace Components.Tests.Components.SleekDial;

public class SleekDialLayoutEngineTests
{
    [Fact]
    public void ComputeLayout_Linear_ReturnsExpectedLayout()
    {
        var result = InvokeComputeLayout(
            new List<SleekDialItem> { null!, null!, null! },
            itemSize: 10,
            mode: SleekDialMode.Linear,
            linearSettings: new SleekDialLinearSettings(),
            linearDirection: SleekDialLinearDirection.Default,
            radialSettings: new SleekDialRadialSettings(),
            animation: new SleekDialAnimationSettings { Stagger = false },
            position: FloatingPosition.TopLeft,
            width: 0,
            height: 0,
            fabWidth: 0,
            fabHeight: 0);

        var layouts = GetLayouts(result);

        Assert.Equal(3, layouts.Count);
        Assert.Equal(10, GetPopupWidth(result));
        Assert.Equal(58, GetPopupHeight(result));
        Assert.Equal(0, layouts[0].X);
        Assert.Equal(0, layouts[0].Y);
    }

    [Fact]
    public void ComputeLayout_Radial_ReturnsExpectedLayout()
    {
        var result = InvokeComputeLayout(
            new List<SleekDialItem> { null!, null!, null! },
            itemSize: 10,
            mode: SleekDialMode.Radial,
            linearSettings: new SleekDialLinearSettings(),
            linearDirection: SleekDialLinearDirection.Default,
            radialSettings: new SleekDialRadialSettings(),
            animation: new SleekDialAnimationSettings { Stagger = false },
            position: FloatingPosition.TopLeft,
            width: 0,
            height: 0,
            fabWidth: 0,
            fabHeight: 0);

        var layouts = GetLayouts(result);

        Assert.Equal(3, layouts.Count);
        Assert.Equal(220, GetPopupWidth(result));
        Assert.Equal(220, GetPopupHeight(result));
        Assert.Equal(0, layouts[0].Angle);
        Assert.Equal(90, layouts[2].Angle);
    }

    private static object InvokeComputeLayout(
        List<SleekDialItem> items,
        int itemSize,
        SleekDialMode mode,
        SleekDialLinearSettings linearSettings,
        SleekDialLinearDirection linearDirection,
        SleekDialRadialSettings radialSettings,
        SleekDialAnimationSettings animation,
        FloatingPosition position,
        int width,
        int height,
        int fabWidth,
        int fabHeight)
    {
        var layoutEngineType = GetLayoutEngineType();
        var computeLayout = layoutEngineType.GetMethod("ComputeLayout", BindingFlags.Public | BindingFlags.Static);

        Assert.NotNull(computeLayout);

        return computeLayout!.Invoke(null, new object[]
        {
            items,
            itemSize,
            mode,
            linearSettings,
            linearDirection,
            radialSettings,
            animation,
            position,
            width,
            height,
            fabWidth,
            fabHeight
        })!;
    }

    private static List<SleekDialItemLayout> GetLayouts(object result)
    {
        var layoutsProperty = result.GetType().GetProperty("Layouts", BindingFlags.Public | BindingFlags.Instance);
        var layouts = layoutsProperty?.GetValue(result) as List<SleekDialItemLayout>;

        Assert.NotNull(layouts);

        return layouts!;
    }

    private static int GetPopupWidth(object result)
    {
        var property = result.GetType().GetProperty("PopupWidth", BindingFlags.Public | BindingFlags.Instance);
        return (int)(property?.GetValue(result) ?? 0);
    }

    private static int GetPopupHeight(object result)
    {
        var property = result.GetType().GetProperty("PopupHeight", BindingFlags.Public | BindingFlags.Instance);
        return (int)(property?.GetValue(result) ?? 0);
    }

    private static Type GetLayoutEngineType()
    {
        var assembly = typeof(SleekDialItem).Assembly;
        return assembly.GetType("FluentUI.Blazor.Community.Components.SleekDialLayoutEngine", throwOnError: true)!;
    }
}
