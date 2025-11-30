using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Tests;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Scheduler;

public class MonthlyRecurrenceTests : TestBase
{
    public MonthlyRecurrenceTests()
    {
        JSInterop.Mode = Bunit.JSRuntimeMode.Loose;
        Services.AddFluentUIComponents();
    }

    [Fact]
    public void MonthlyRecurrence_CanBeInstantiated()
    {
        var cut = RenderComponent<MonthlyRecurrence>();
        Assert.NotNull(cut.Instance);
    }

    [Fact]
    public void AddException_AddsException_WhenExceptionDateSetAndNotPresent()
    {
        var cut = RenderComponent<MonthlyRecurrence>();
        var instance = cut.Instance;
        var target = new DateTime(2026, 3, 15);

        var field = instance.GetType().GetField("_exceptionDate", BindingFlags.Instance | BindingFlags.NonPublic)!;
        field.SetValue(instance, target);

        Assert.DoesNotContain(target, instance.Exceptions);

        var method = instance.GetType().GetMethod("AddException", BindingFlags.Instance | BindingFlags.NonPublic)!;
        method.Invoke(instance, Array.Empty<object>());

        Assert.Contains(target, instance.Exceptions);
        Assert.Null(field.GetValue(instance));
    }

    [Fact]
    public void AddException_DoesNotAddDuplicate_ButClearsExceptionDate()
    {
        var cut = RenderComponent<MonthlyRecurrence>();
        var instance = cut.Instance;
        var target = new DateTime(2026, 4, 1);

        var field = instance.GetType().GetField("_exceptionDate", BindingFlags.Instance | BindingFlags.NonPublic)!;
        field.SetValue(instance, target);

        var method = instance.GetType().GetMethod("AddException", BindingFlags.Instance | BindingFlags.NonPublic)!;
        method.Invoke(instance, []);

        Assert.Single(instance.Exceptions, target);
        Assert.Null(field.GetValue(instance));
    }

    [Fact]
    public void RemoveException_RemovesDate_WhenPresent()
    {
        var cut = RenderComponent<MonthlyRecurrence>();
        var instance = cut.Instance;
        var target = new DateTime(2027, 1, 1);

        instance.Exceptions.Add(target);

        var method = instance.GetType().GetMethod("RemoveException", BindingFlags.Instance | BindingFlags.NonPublic)!;
        method.Invoke(instance, new object[] { target });

        Assert.DoesNotContain(target, instance.Exceptions);
    }

    [Fact]
    public void GetAppearance_ReturnsAccent_WhenPrimaryTrue_ElseNeutral()
    {
        var mi = typeof(MonthlyRecurrence).GetMethod("GetAppearance", BindingFlags.NonPublic | BindingFlags.Static)!;

        var accent = (Appearance)mi.Invoke(null, new object[] { true })!;
        var neutral = (Appearance)mi.Invoke(null, new object[] { false })!;

        Assert.Equal(Appearance.Accent, accent);
        Assert.Equal(Appearance.Neutral, neutral);
    }
}
