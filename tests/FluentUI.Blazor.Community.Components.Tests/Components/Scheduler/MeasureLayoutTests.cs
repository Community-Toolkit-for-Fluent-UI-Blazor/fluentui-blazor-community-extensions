using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Scheduler;

public class MeasureLayoutTests
{
    private static Type GetMeasureLayoutType()
    {
        var asm = typeof(FluentUI.Blazor.Community.Components.ElementDimensions).Assembly;
        return asm.GetType("FluentUI.Blazor.Community.Components.MeasureLayout", throwOnError: true)!;
    }

    private static Type GetPaddingFType()
    {
        var asm = typeof(FluentUI.Blazor.Community.Components.ElementDimensions).Assembly;
        return asm.GetType("FluentUI.Blazor.Community.Components.PaddingF", throwOnError: true)!;
    }

    [Fact]
    public void DefaultInstance_PropertiesHaveDefaultValues()
    {
        var t = GetMeasureLayoutType();
        var instance = Activator.CreateInstance(t)!;

        Assert.NotNull(instance);

        // default RectangleF/SizeF/PointF fields should be zeros
        var overlay = (RectangleF)t.GetProperty("Overlay")!.GetValue(instance)!;
        var cellSize = (SizeF)t.GetProperty("CellSize")!.GetValue(instance)!;
        var labelSize = (SizeF)t.GetProperty("LabelSize")!.GetValue(instance)!;
        var contentSize = (RectangleF)t.GetProperty("ContentSize")!.GetValue(instance)!;
        var gap = (float)t.GetProperty("Gap")!.GetValue(instance)!;
        var usableHeight = (float)t.GetProperty("UsableHeight")!.GetValue(instance)!;
        var headerHeight = (float)t.GetProperty("HeaderHeight")!.GetValue(instance)!;
        var local = (PointF)t.GetProperty("Local")!.GetValue(instance)!;

        Assert.Equal(0f, overlay.X);
        Assert.Equal(0f, overlay.Y);
        Assert.Equal(0f, overlay.Width);
        Assert.Equal(0f, overlay.Height);

        Assert.Equal(0f, cellSize.Width);
        Assert.Equal(0f, cellSize.Height);

        Assert.Equal(0f, labelSize.Width);
        Assert.Equal(0f, labelSize.Height);

        Assert.Equal(0f, contentSize.Width);
        Assert.Equal(0f, contentSize.Height);

        Assert.Equal(0f, gap);
        Assert.Equal(0f, usableHeight);
        Assert.Equal(0f, headerHeight);

        Assert.Equal(0f, local.X);
        Assert.Equal(0f, local.Y);
    }

    [Fact]
    public void Instances_WithSameValues_AreEqualAndHaveSameHashCode()
    {
        var t = GetMeasureLayoutType();
        var paddingType = GetPaddingFType();

        // create padding instance via constructor
        var paddingCtor = paddingType.GetConstructor(new[] { typeof(float), typeof(float), typeof(float), typeof(float) })!;
        var padding = paddingCtor.Invoke(new object[] { 1f, 2f, 3f, 4f });

        // build property values
        var overlay = new RectangleF(0, 0, 100, 200);
        var cellSize = new SizeF(50, 50);
        var labelSize = new SizeF(10, 20);
        var contentSize = new RectangleF(0, 0, 300, 400);
        var gap = 8f;
        var usableHeight = 250f;
        var headerHeight = 32f;
        var local = new PointF(5f, 6f);

        // create and populate first instance
        var a = Activator.CreateInstance(t)!;
        t.GetProperty("Overlay")!.SetValue(a, overlay);
        t.GetProperty("CellSize")!.SetValue(a, cellSize);
        t.GetProperty("LabelSize")!.SetValue(a, labelSize);
        t.GetProperty("ContentSize")!.SetValue(a, contentSize);
        t.GetProperty("Padding")!.SetValue(a, padding);
        t.GetProperty("Gap")!.SetValue(a, gap);
        t.GetProperty("UsableHeight")!.SetValue(a, usableHeight);
        t.GetProperty("HeaderHeight")!.SetValue(a, headerHeight);
        t.GetProperty("Local")!.SetValue(a, local);

        // create and populate second instance with same values
        var b = Activator.CreateInstance(t)!;
        t.GetProperty("Overlay")!.SetValue(b, overlay);
        t.GetProperty("CellSize")!.SetValue(b, cellSize);
        t.GetProperty("LabelSize")!.SetValue(b, labelSize);
        t.GetProperty("ContentSize")!.SetValue(b, contentSize);
        t.GetProperty("Padding")!.SetValue(b, padding);
        t.GetProperty("Gap")!.SetValue(b, gap);
        t.GetProperty("UsableHeight")!.SetValue(b, usableHeight);
        t.GetProperty("HeaderHeight")!.SetValue(b, headerHeight);
        t.GetProperty("Local")!.SetValue(b, local);

        // equality checks (record -> value equality)
        Assert.Equal(a, b);
        Assert.True(a.Equals(b));
        var hashA = t.GetMethod("GetHashCode")!.Invoke(a, null);
        var hashB = t.GetMethod("GetHashCode")!.Invoke(b, null);
        Assert.Equal(hashA, hashB);
    }

    [Fact]
    public void ChangingOneProperty_MakesInstancesNotEqual()
    {
        var t = GetMeasureLayoutType();
        var paddingType = GetPaddingFType();
        var paddingCtor = paddingType.GetConstructor(new[] { typeof(float), typeof(float), typeof(float), typeof(float) })!;
        var padding = paddingCtor.Invoke(new object[] { 1f, 2f, 3f, 4f });

        var baseOverlay = new RectangleF(0, 0, 100, 200);

        var a = Activator.CreateInstance(t)!;
        var b = Activator.CreateInstance(t)!;

        // set same values
        t.GetProperty("Overlay")!.SetValue(a, baseOverlay);
        t.GetProperty("Overlay")!.SetValue(b, baseOverlay);
        t.GetProperty("Padding")!.SetValue(a, padding);
        t.GetProperty("Padding")!.SetValue(b, padding);

        // modify one property on b
        var modifiedOverlay = new RectangleF(1, 2, 3, 4);
        t.GetProperty("Overlay")!.SetValue(b, modifiedOverlay);

        Assert.NotEqual(a, b);
    }

    [Fact]
    public void ToString_ContainsPropertyNamesAndValues()
    {
        var t = GetMeasureLayoutType();
        var instance = Activator.CreateInstance(t)!;

        // set a couple properties to ensure output contains meaningful values
        t.GetProperty("Gap")!.SetValue(instance, 12f);
        t.GetProperty("HeaderHeight")!.SetValue(instance, 7f);

        var s = instance.ToString() ?? string.Empty;

        Assert.Contains("Gap", s, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("HeaderHeight", s, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("12", s);
        Assert.Contains("7", s);
    }
}
