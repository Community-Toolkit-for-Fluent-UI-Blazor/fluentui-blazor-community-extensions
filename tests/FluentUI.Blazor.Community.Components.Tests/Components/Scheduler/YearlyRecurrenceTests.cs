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

public class YearlyRecurrenceTests : TestBase
{
    public YearlyRecurrenceTests()
    {
        JSInterop.Mode = Bunit.JSRuntimeMode.Loose;
        Services.AddFluentUIComponents();
    }

    [Fact]
    public void YearlyRecurrence_CanBeInstantiated()
    {
        var cut = RenderComponent<YearlyRecurrence>();
        Assert.NotNull(cut.Instance);
    }

    [Fact]
    public void AddException_AddsException_WhenExceptionDateSetAndNotPresent()
    {
        var cut = RenderComponent<YearlyRecurrence>();
        var instance = cut.Instance;
        var target = new DateTime(2026, 7, 14);

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
        var cut = RenderComponent<YearlyRecurrence>();
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
        var cut = RenderComponent<YearlyRecurrence>();
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
        var mi = typeof(YearlyRecurrence).GetMethod("GetAppearance", BindingFlags.NonPublic | BindingFlags.Static)!;

        var accent = (Microsoft.FluentUI.AspNetCore.Components.Appearance)mi.Invoke(null, new object[] { true })!;
        var neutral = (Microsoft.FluentUI.AspNetCore.Components.Appearance)mi.Invoke(null, new object[] { false })!;

        Assert.Equal(Microsoft.FluentUI.AspNetCore.Components.Appearance.Accent, accent);
        Assert.Equal(Microsoft.FluentUI.AspNetCore.Components.Appearance.Neutral, neutral);
    }

    [Fact]
    public void ToggleMonth_AddsAndRemovesMonth_InRecurrenceMonths()
    {
        // Arrange: create a RecurrenceRule instance via reflection to avoid compile-time coupling
        var recurrenceType = typeof(YearlyRecurrence).Assembly.GetType("FluentUI.Blazor.Community.Components.RecurrenceRule", throwOnError: true)!;
        var recurrence = Activator.CreateInstance(recurrenceType)!;

        // Ensure Months list exists and is a List<int>
        var monthsProp = recurrenceType.GetProperty("Months", BindingFlags.Public | BindingFlags.Instance);
        List<int> monthsList;
        if (monthsProp is null)
        {
            throw new InvalidOperationException("RecurrenceRule must expose a public Months property.");
        }

        var current = monthsProp.GetValue(recurrence) as List<int>;
        if (current is null)
        {
            monthsList = new();
            monthsProp.SetValue(recurrence, monthsList);
        }
        else
        {
            monthsList = current;
        }

        // Render component with the Recurrence parameter set
        var cut = RenderComponent<YearlyRecurrence>(parameters => parameters.Add(p => p.Recurrence, recurrence));
        var instance = cut.Instance;

        // Ensure month 3 is not present
        const int month = 3;
        if (monthsList.Contains(month))
        {
            monthsList.Remove(month);
        }

        var toggle = instance.GetType().GetMethod("ToggleMonth", BindingFlags.Instance | BindingFlags.NonPublic)!;

        // Act: toggle to add
        toggle.Invoke(instance, new object[] { month });

        // Assert added
        Assert.Contains(month, monthsList);

        // Act: toggle to remove
        toggle.Invoke(instance, new object[] { month });

        // Assert removed
        Assert.DoesNotContain(month, monthsList);
    }
}
