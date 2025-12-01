using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Bunit;
using FluentUI.Blazor.Community.Extensions;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Tests;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Scheduler;

public class SchedulerNavigationTests : TestBase
{
    public SchedulerNavigationTests()
    {
        // Autorise les appels JS import() non stubés
        JSInterop.Mode = Bunit.JSRuntimeMode.Loose;

        // Enregistre les services requis par les composants
        Services.AddFluentUIComponents();
        Services.AddFluentCxUIComponents();
    }

    [Fact]
    public void Renders_Three_Buttons()
    {
        // Arrange - parent scheduler minimal
        var parent = RenderComponent<FluentCxScheduler<object>>();

        // Act - render navigation with parent as cascading value
        var cut = RenderComponent<SchedulerNavigation<object>>(ps => ps.AddCascadingValue(parent.Instance));

        // Assert
        var buttons = cut.FindAll("fluent-button");
        Assert.Equal(3, buttons.Count);
    }

    [Fact]
    public void PreviousButton_Click_Moves_Parent_To_Previous()
    {
        // Arrange
        var initial = new DateTime(2025, 11, 20);
        var parent = RenderComponent<FluentCxScheduler<object>>(p => p.Add(p => p.CurrentDate, initial));

        // Ensure view is Day so MoveToPreviousAsync uses days
        SetPrivateField(parent.Instance, "_currentView", SchedulerView.Day);

        var cut = RenderComponent<SchedulerNavigation<object>>(ps => ps.AddCascadingValue(parent.Instance));
        var prevButton = cut.FindAll("fluent-button").First();

        // Act
        prevButton.Click();

        // Assert: current date decreased by 1 day
        var expected = initial.AddDays(-1);
        Assert.Equal(expected.Date, parent.Instance.CurrentDate.Date);
    }

    [Fact]
    public void NextButton_Click_Moves_Parent_To_Next()
    {
        // Arrange
        var initial = new DateTime(2025, 11, 20);
        var parent = RenderComponent<FluentCxScheduler<object>>(p => p.Add(p => p.CurrentDate, initial));

        SetPrivateField(parent.Instance, "_currentView", SchedulerView.Day);

        var cut = RenderComponent<SchedulerNavigation<object>>(ps => ps.AddCascadingValue(parent.Instance));
        var nextButton = cut.FindAll("fluent-button").Last();

        // Act
        nextButton.Click();

        // Assert: current date increased by 1 day
        var expected = initial.AddDays(1);
        Assert.Equal(expected.Date, parent.Instance.CurrentDate.Date);
    }

    [Fact]
    public void TodayButton_Click_Sets_Parent_To_Today()
    {
        // Arrange
        var initial = new DateTime(2000, 1, 1);
        var parent = RenderComponent<FluentCxScheduler<object>>(p => p.Add(p => p.CurrentDate, initial));

        SetPrivateField(parent.Instance, "_currentView", SchedulerView.Day);

        var cut = RenderComponent<SchedulerNavigation<object>>(ps => ps.AddCascadingValue(parent.Instance));
        // Middle button is "Today"
        var buttons = cut.FindAll("fluent-button");
        var todayButton = buttons.Skip(1).First();

        // Act
        todayButton.Click();

        // Assert: current date equals DateTime.Today (date-only comparison)
        Assert.Equal(DateTime.Today, parent.Instance.CurrentDate.Date);
    }

    // --- Helpers réflexion ---

    private static void SetPrivateField(object target, string fieldName, object? value)
    {
        var t = target.GetType();
        var f = t.GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
        if (f == null)
        {
            throw new InvalidOperationException($"Field '{fieldName}' not found on {t.FullName}.");
        }

        f.SetValue(target, value);
    }
}
