using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Scheduler;

public class PaddingFTests
{
    private static Type GetPaddingFType()
    {
        var asm = typeof(ElementDimensions).Assembly;
        return asm.GetType("FluentUI.Blazor.Community.Components.PaddingF", throwOnError: true)!;
    }

    [Fact]
    public void Constructor_SetsProperties()
    {
        var t = GetPaddingFType();
        var ctor = t.GetConstructor(new[] { typeof(float), typeof(float), typeof(float), typeof(float) })!;
        var instance = ctor.Invoke(new object[] { 1f, 2f, 3f, 4f })!;

        var left = (float)t.GetProperty("Left")!.GetValue(instance)!;
        var top = (float)t.GetProperty("Top")!.GetValue(instance)!;
        var right = (float)t.GetProperty("Right")!.GetValue(instance)!;
        var bottom = (float)t.GetProperty("Bottom")!.GetValue(instance)!;

        Assert.Equal(1f, left);
        Assert.Equal(2f, top);
        Assert.Equal(3f, right);
        Assert.Equal(4f, bottom);
    }

    [Fact]
    public void DefaultInstance_IsZeroForAllSides()
    {
        var t = GetPaddingFType();
        var instance = Activator.CreateInstance(t)!;

        Assert.Equal(0f, (float)t.GetProperty("Left")!.GetValue(instance)!);
        Assert.Equal(0f, (float)t.GetProperty("Top")!.GetValue(instance)!);
        Assert.Equal(0f, (float)t.GetProperty("Right")!.GetValue(instance)!);
        Assert.Equal(0f, (float)t.GetProperty("Bottom")!.GetValue(instance)!);
    }

    [Fact]
    public void Properties_AreSettableViaReflection()
    {
        var t = GetPaddingFType();
        var instance = Activator.CreateInstance(t)!;

        var leftProp = t.GetProperty("Left")!;
        var topProp = t.GetProperty("Top")!;
        var rightProp = t.GetProperty("Right")!;
        var bottomProp = t.GetProperty("Bottom")!;

        leftProp.SetValue(instance, 5f);
        topProp.SetValue(instance, 6f);
        rightProp.SetValue(instance, 7f);
        bottomProp.SetValue(instance, 8f);

        Assert.Equal(5f, (float)leftProp.GetValue(instance)!);
        Assert.Equal(6f, (float)topProp.GetValue(instance)!);
        Assert.Equal(7f, (float)rightProp.GetValue(instance)!);
        Assert.Equal(8f, (float)bottomProp.GetValue(instance)!);
    }

    [Fact]
    public void Equality_SameValues_AreEqual()
    {
        var t = GetPaddingFType();
        var ctor = t.GetConstructor(new[] { typeof(float), typeof(float), typeof(float), typeof(float) })!;
        var a = ctor.Invoke(new object[] { 1f, 2f, 3f, 4f })!;
        var b = ctor.Invoke(new object[] { 1f, 2f, 3f, 4f })!;

        // Equals(object)
        var equalsMethod = t.GetMethod("Equals", new[] { typeof(object) })!;
        var eq = (bool)equalsMethod.Invoke(a, new object[] { b })!;
        Assert.True(eq);

        // GetHashCode same
        var hcA = (int)t.GetMethod("GetHashCode")!.Invoke(a, null)!;
        var hcB = (int)t.GetMethod("GetHashCode")!.Invoke(b, null)!;
        Assert.Equal(hcA, hcB);
    }
}
