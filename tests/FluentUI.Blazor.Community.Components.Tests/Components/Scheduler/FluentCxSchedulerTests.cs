using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bunit;
using FluentUI.Blazor.Community.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Tests;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Scheduler;

public class FluentCxSchedulerTests : TestBase
{
    public FluentCxSchedulerTests()
    {
        JSInterop.Mode = Bunit.JSRuntimeMode.Loose;
        Services.AddSingleton(UnitTestLibraryConfiguration);
        Services.AddFluentUIComponents();
        Services.AddFluentCxUIComponents();
    }

    [Fact]
    public void Renders_without_throwing()
    {
        // Act
        var cut = RenderComponent<FluentCxScheduler<string>>();

        // Assert
        Assert.NotNull(cut.Instance);
    }

    [Fact]
    public void SetDayWeekTimelineSubdivisions_methods_update_properties()
    {
        // Arrange
        var cut = RenderComponent<FluentCxScheduler<string>>();

        // Act
        cut.InvokeAsync(() => cut.Instance.SetDaySubdivisions(6));
        cut.InvokeAsync(() => cut.Instance.SetWeekSubdivisions(8));
        cut.InvokeAsync(() => cut.Instance.SetTimelineSubdivisions(12));

        // Assert
        Assert.Equal(6, cut.Instance.DaySubdivisions);
        Assert.Equal(8, cut.Instance.WeekSubdivisions);
        Assert.Equal(12, cut.Instance.TimelineSubdivisions);
    }

    [Fact]
    public async Task LoadItemsAsync_populates_ItemsByDay_when_provider_returns_items()
    {
        // Arrange
        var date = new DateTime(2025, 06, 10);
        var item = new SchedulerItem<string>
        {
            Id = 1,
            Title = "X",
            Start = date,
            End = date.AddHours(1),
            Exceptions = new List<DateTime>()
        };

        var called = 0;
        Func<SchedulerFetchRequest, ValueTask<IEnumerable<SchedulerItem<string>>>> provider =
            req =>
            {
                called++;
                return new ValueTask<IEnumerable<SchedulerItem<string>>>(new[] { item });
            };

        // Act
        var cut = RenderComponent<FluentCxScheduler<string>>(parameters => parameters
            .Add(p => p.ItemsProvider, provider)
            .Add(p => p.CurrentDate, date)
        );

        // Assert provider was invoked at least once during initialization and ItemsByDay contains the date
        Assert.True(called > 0);
        Assert.True(cut.Instance.ItemsByDay.ContainsKey(item.Start.Date));
        Assert.Contains(item, cut.Instance.ItemsByDay[item.Start.Date]);
    }

    [Fact]
    public async Task ChangeViewAsync_and_MoveToNextPreviousAsync_update_current_date()
    {
        var mockModule = JSInterop.SetupModule("./_content/FluentUI.Blazor.Community.Components/Components/Scheduler/FluentCxScheduler.razor.js");
        mockModule.Setup<MeasureLayout>("measureLayout").SetResult(new()
        {
            CellSize = new(80, 80),
            ContentSize = new(0, 0, 800, 600),
            Gap = 2,
            HeaderHeight = 50,
            LabelSize = new(100, 30),
            Overlay = new(0, 0, 800, 600),
            Padding = new(5, 5, 5, 5)
        });

        // Arrange
        var start = new DateTime(2025, 01, 15);
        var cut = RenderComponent<FluentCxScheduler<string>>(parameters => parameters
            .Add(p => p.CurrentDate, start)
        );

        // Switch to Day view and move next/previous
        await cut.Instance.ChangeViewAsync(SchedulerView.Day);
        var dayBefore = cut.Instance.CurrentDate;
        await cut.Instance.MoveToNextAsync();
        Assert.Equal(dayBefore.AddDays(1), cut.Instance.CurrentDate);
        await cut.Instance.MoveToPreviousAsync();
        Assert.Equal(dayBefore, cut.Instance.CurrentDate);

        // Week view
        await cut.Instance.ChangeViewAsync(SchedulerView.Week);
        dayBefore = cut.Instance.CurrentDate;
        await cut.Instance.MoveToNextAsync();
        Assert.Equal(dayBefore.AddDays(7), cut.Instance.CurrentDate);
        await cut.Instance.MoveToPreviousAsync();
        Assert.Equal(dayBefore, cut.Instance.CurrentDate);

        // Month view
        await cut.Instance.ChangeViewAsync(SchedulerView.Month);
        dayBefore = cut.Instance.CurrentDate;
        await cut.Instance.MoveToNextAsync();
        Assert.Equal(dayBefore.AddMonths(1), cut.Instance.CurrentDate);
        await cut.Instance.MoveToPreviousAsync();
        Assert.Equal(dayBefore, cut.Instance.CurrentDate);

        // Year view
        await cut.Instance.ChangeViewAsync(SchedulerView.Year);
        dayBefore = cut.Instance.CurrentDate;
        await cut.Instance.MoveToNextAsync();
        Assert.Equal(dayBefore.AddYears(1), cut.Instance.CurrentDate);
        await cut.Instance.MoveToPreviousAsync();
        Assert.Equal(dayBefore, cut.Instance.CurrentDate);

        // Agenda view (NumberOfDays default = 7)
        await cut.Instance.ChangeViewAsync(SchedulerView.Agenda);
        dayBefore = cut.Instance.CurrentDate;
        await cut.Instance.MoveToNextAsync();
        Assert.Equal(dayBefore.AddDays(cut.Instance.NumberOfDays), cut.Instance.CurrentDate);
        await cut.Instance.MoveToPreviousAsync();
        Assert.Equal(dayBefore, cut.Instance.CurrentDate);

        // Timeline view (moves by 1 day)
        await cut.Instance.ChangeViewAsync(SchedulerView.Timeline);
        dayBefore = cut.Instance.CurrentDate;
        await cut.Instance.MoveToNextAsync();
        Assert.Equal(dayBefore.AddDays(1), cut.Instance.CurrentDate);
        await cut.Instance.MoveToPreviousAsync();
        Assert.Equal(dayBefore, cut.Instance.CurrentDate);
    }

    [Fact]
    public void ToggleShowNonWorkingHours_toggles_flag()
    {
        // Arrange
        var cut = RenderComponent<FluentCxScheduler<string>>();
        var before = cut.Instance.ShowNonWorkingHours;

        // Act
        cut.InvokeAsync(() => cut.Instance.ToggleShowNonWorkingHours());

        // Assert
        Assert.NotEqual(before, cut.Instance.ShowNonWorkingHours);
    }

    [Fact]
    public void SetAgendaDays_updates_NumberOfDays_and_slotbuilder()
    {
        // Arrange
        var cut = RenderComponent<FluentCxScheduler<string>>();

        // Act
        cut.InvokeAsync(() => cut.Instance.SetAgendaDays(10));

        // Assert
        Assert.Equal(10, cut.Instance.NumberOfDays);
    }
}
