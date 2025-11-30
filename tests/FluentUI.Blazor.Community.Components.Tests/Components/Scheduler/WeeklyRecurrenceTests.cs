using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Tests;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Scheduler;

public class WeeklyRecurrenceTests : TestBase
{
    public WeeklyRecurrenceTests()
    {
        JSInterop.Mode = Bunit.JSRuntimeMode.Loose;
        Services.AddFluentUIComponents();
    }

    [Fact]
    public void WeeklyRecurrence_CanBeInstantiated()
    {
        var cut = RenderComponent<WeeklyRecurrence>();
        Assert.NotNull(cut.Instance);
    }

    [Fact]
    public void AddException_AddsException_WhenExceptionDateSetAndNotPresent()
    {
        var cut = RenderComponent<WeeklyRecurrence>();
        var instance = cut.Instance;
        var target = new DateTime(2026, 6, 21);

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
        var cut = RenderComponent<WeeklyRecurrence>();
        var instance = cut.Instance;
        var target = new DateTime(2026, 12, 25);

        var field = instance.GetType().GetField("_exceptionDate", BindingFlags.Instance | BindingFlags.NonPublic)!;
        field.SetValue(instance, target);

        var method = instance.GetType().GetMethod("AddException", BindingFlags.Instance | BindingFlags.NonPublic)!;
        method.Invoke(instance, Array.Empty<object>());

        Assert.Single(instance.Exceptions, target);
        Assert.Null(field.GetValue(instance));
    }

    [Fact]
    public void RemoveException_RemovesDate_WhenPresent()
    {
        var cut = RenderComponent<WeeklyRecurrence>();
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
        var mi = typeof(WeeklyRecurrence).GetMethod("GetAppearance", BindingFlags.NonPublic | BindingFlags.Static)!;

        var accent = (Microsoft.FluentUI.AspNetCore.Components.Appearance)mi.Invoke(null, new object[] { true })!;
        var neutral = (Microsoft.FluentUI.AspNetCore.Components.Appearance)mi.Invoke(null, new object[] { false })!;

        Assert.Equal(Microsoft.FluentUI.AspNetCore.Components.Appearance.Accent, accent);
        Assert.Equal(Microsoft.FluentUI.AspNetCore.Components.Appearance.Neutral, neutral);
    }

    [Fact]
    public void ToggleDay_AddsAndRemovesDay_InRecurrenceDaysOfWeek()
    {
        // Create RecurrenceRule via reflection
        var recurrenceType = typeof(WeeklyRecurrence).Assembly.GetType("FluentUI.Blazor.Community.Components.RecurrenceRule", throwOnError: true)!;
        var recurrence = Activator.CreateInstance(recurrenceType)!;

        // Ensure DaysOfWeek property exists and is a List<DayOfWeek>
        var daysProp = recurrenceType.GetProperty("DaysOfWeek", BindingFlags.Public | BindingFlags.Instance);
        if (daysProp is null)
        {
            throw new InvalidOperationException("RecurrenceRule must expose a public DaysOfWeek property.");
        }

        var current = daysProp.GetValue(recurrence) as List<DayOfWeek>;
        List<DayOfWeek> daysList;
        if (current is null)
        {
            daysList = new();
            daysProp.SetValue(recurrence, daysList);
        }
        else
        {
            daysList = current;
        }

        // Render component with the Recurrence parameter set
        var cut = RenderComponent<WeeklyRecurrence>(parameters => parameters.Add(p => p.Recurrence, recurrence));
        var instance = cut.Instance;

        const DayOfWeek day = DayOfWeek.Wednesday;

        // Ensure day not present
        if (daysList.Contains(day))
        {
            daysList.Remove(day);
        }

        var toggle = instance.GetType().GetMethod("ToggleDay", BindingFlags.Instance | BindingFlags.NonPublic)!;

        // Act: toggle to add
        toggle.Invoke(instance, new object[] { day });

        // Assert added
        Assert.Contains(day, daysList);

        // Act: toggle to remove
        toggle.Invoke(instance, new object[] { day });

        // Assert removed
        Assert.DoesNotContain(day, daysList);
    }
}
