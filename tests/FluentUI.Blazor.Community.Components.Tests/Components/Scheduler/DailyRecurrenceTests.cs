using System.Reflection;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Tests;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Scheduler;

public class DailyRecurrenceTests : TestBase
{
    public DailyRecurrenceTests()
    {
        JSInterop.Mode = Bunit.JSRuntimeMode.Loose;
        Services.AddFluentUIComponents();
    }

    [Fact]
    public void DailyRecurrence_CanBeInstantiated()
    {
        var cut = RenderComponent<DailyRecurrence>();
        Assert.NotNull(cut.Instance);
    }

    [Fact]
    public void AddException_AddsException_WhenExceptionDateSetAndNotPresent()
    {
        var cut = RenderComponent<DailyRecurrence>();
        var instance = cut.Instance;
        var target = new DateTime(2025, 12, 24);

        // Set private field _exceptionDate
        var field = instance.GetType().GetField("_exceptionDate", BindingFlags.Instance | BindingFlags.NonPublic)!;
        field.SetValue(instance, target);

        // Ensure not present initially
        Assert.DoesNotContain(target, instance.Exceptions);

        // Invoke private AddException
        var method = instance.GetType().GetMethod("AddException", BindingFlags.Instance | BindingFlags.NonPublic)!;
        method.Invoke(instance, Array.Empty<object>());

        // Assert added and _exceptionDate cleared
        Assert.Contains(target, instance.Exceptions);
        Assert.Null(field.GetValue(instance));
    }

    [Fact]
    public void AddException_DoesNotAddDuplicate_ButClearsExceptionDate()
    {
        var cut = RenderComponent<DailyRecurrence>();
        var instance = cut.Instance;
        var target = new DateTime(2025, 12, 25);
        var field = instance.GetType().GetField("_exceptionDate", BindingFlags.Instance | BindingFlags.NonPublic)!;
        field.SetValue(instance, target);

        // Invoke AddException
        var method = instance.GetType().GetMethod("AddException", BindingFlags.Instance | BindingFlags.NonPublic)!;
        method.Invoke(instance, []);

        // No duplicate added, but _exceptionDate cleared
        Assert.Single(instance.Exceptions, target);
        Assert.Null(field.GetValue(instance));
    }

    [Fact]
    public void RemoveException_RemovesDate_WhenPresent()
    {
        var cut = RenderComponent<DailyRecurrence>();
        var instance = cut.Instance;
        var target = new DateTime(2026, 1, 1);

        instance.Exceptions.Add(target);

        // Invoke private RemoveException
        var method = instance.GetType().GetMethod("RemoveException", BindingFlags.Instance | BindingFlags.NonPublic)!;
        method.Invoke(instance, new object[] { target });

        Assert.DoesNotContain(target, instance.Exceptions);
    }

    [Fact]
    public void GetAppearance_ReturnsAccent_WhenPrimaryTrue_ElseNeutral()
    {
        var mi = typeof(DailyRecurrence).GetMethod("GetAppearance", BindingFlags.NonPublic | BindingFlags.Static)!;

        var accent = (Appearance)mi.Invoke(null, new object[] { true })!;
        var neutral = (Appearance)mi.Invoke(null, new object[] { false })!;

        Assert.Equal(Appearance.Accent, accent);
        Assert.Equal(Appearance.Neutral, neutral);
    }
}
