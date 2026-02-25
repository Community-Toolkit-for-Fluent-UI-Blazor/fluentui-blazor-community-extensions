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

public class SchedulerAgendaViewTests : TestBase
{
    public SchedulerAgendaViewTests()
    {
        JSInterop.Mode = Bunit.JSRuntimeMode.Loose;
        Services.AddSingleton(UnitTestLibraryConfiguration);
        Services.AddFluentUIComponents();
        Services.AddFluentCxUIComponents();
    }

    [Fact]
    public void Renders_empty_days_when_no_items_and_hide_empty_false()
    {
        // Arrange
        var date = new DateTime(2025, 6, 1);

        // Act
        var cut = RenderComponent<SchedulerAgendaView<string>>(parameters => parameters
            .Add(p => p.CurrentDate, date)
            .Add(p => p.NumberOfDays, 3)
            .Add(p => p.Items, new List<SchedulerItem<string>>())
            .Add(p => p.HideEmptyDays, false)
        );

        // Assert: one row per day (agenda-single)
        var rows = cut.FindAll("tr.agenda-row");
        Assert.Equal(3, rows.Count);
        // each row should contain empty agenda-cell elements
        Assert.All(rows, r => Assert.NotEmpty(r.QuerySelectorAll(".agenda-cell")));
    }

    [Fact]
    public void Does_not_render_days_when_no_items_and_hide_empty_true()
    {
        // Arrange
        var date = new DateTime(2025, 6, 1);

        // Act
        var cut = RenderComponent<SchedulerAgendaView<string>>(parameters => parameters
            .Add(p => p.CurrentDate, date)
            .Add(p => p.NumberOfDays, 3)
            .Add(p => p.Items, new List<SchedulerItem<string>>())
            .Add(p => p.HideEmptyDays, true)
        );

        // Assert: no agenda rows rendered
        var rows = cut.FindAll("tr.agenda-row");
        Assert.Empty(rows);
    }

    [Fact]
    public void Single_day_item_is_rendered_with_times_and_title()
    {
        // Arrange
        var start = new DateTime(2025, 6, 5, 9, 30, 0);
        var item = new SchedulerItem<string>
        {
            Id = 1,
            Title = "Meeting A",
            Start = start,
            End = start.AddHours(1),
            Exceptions = new List<DateTime>()
        };

        // Act
        var cut = RenderComponent<SchedulerAgendaView<string>>(parameters => parameters
            .Add(p => p.CurrentDate, start.Date)
            .Add(p => p.NumberOfDays, 1)
            .Add(p => p.Items, new List<SchedulerItem<string>> { item })
        );

        // Assert: one agenda row with the event title and time range
        var eventCells = cut.FindAll(".agenda-event .agenda-cell").Select(e => e.TextContent.Trim());
        Assert.Contains("Meeting A", eventCells);
        var timeCells = cut.FindAll(".agenda-hour .agenda-cell").Select(e => e.TextContent.Trim());
        Assert.Contains("09:30 - 10:30", timeCells);
    }

    [Fact]
    public void Multi_day_item_is_listed_on_each_day_with_correct_segment_texts()
    {
        // Arrange
        var start = new DateTime(2025, 6, 1, 15, 0, 0); // day1 15:00
        var end = new DateTime(2025, 6, 3, 11, 0, 0);   // day3 11:00
        var item = new SchedulerItem<string>
        {
            Id = 2,
            Title = "Conference",
            Start = start,
            End = end,
            Exceptions = new List<DateTime>()
        };

        // Act
        var cut = RenderComponent<SchedulerAgendaView<string>>(parameters => parameters
            .Add(p => p.CurrentDate, start.Date)
            .Add(p => p.NumberOfDays, 3)
            .Add(p => p.Items, new List<SchedulerItem<string>> { item })
        );

        // Assert: three days rendered (multi-day event appears on each)
        var rows = cut.FindAll("tr.agenda-row");
        Assert.NotEmpty(rows);

        // Check that for day1 we have "15:00 - 23:59"
        Assert.Contains(rows, r => r.TextContent.Contains("15:00 - 23:59") && r.TextContent.Contains("Conference"));

        // Check that for middle day we have "00:00 - 23:59"
        Assert.Contains(rows, r => r.TextContent.Contains("00:00 - 23:59") && r.TextContent.Contains("Conference"));

        // Check that for day3 we have "00:00 - 11:00"
        Assert.Contains(rows, r => r.TextContent.Contains("00:00 - 11:00") && r.TextContent.Contains("Conference"));
    }

    [Fact]
    public void Items_in_a_day_are_sorted_by_start_then_end_then_id()
    {
        // Arrange
        var day = new DateTime(2025, 7, 10);
        var a = new SchedulerItem<string> { Id = 3, Title = "A", Start = day.AddHours(9), End = day.AddHours(10), Exceptions = new List<DateTime>() };
        var b = new SchedulerItem<string> { Id = 4, Title = "B", Start = day.AddHours(8), End = day.AddHours(9), Exceptions = new List<DateTime>() };
        var c = new SchedulerItem<string> { Id = 5, Title = "C", Start = day.AddHours(9), End = day.AddHours(11), Exceptions = new List<DateTime>() };

        var items = new List<SchedulerItem<string>> { a, b, c };

        // Act
        var cut = RenderComponent<SchedulerAgendaView<string>>(parameters => parameters
            .Add(p => p.CurrentDate, day)
            .Add(p => p.NumberOfDays, 1)
            .Add(p => p.Items, items)
        );

        // Collect event titles in the rendered order for that day
        var eventCells = cut.FindAll("tr.agenda-row")
                           .SelectMany(tr => tr.QuerySelectorAll(".agenda-event .agenda-cell"))
                           .Select(e => e.TextContent.Trim())
                           .Where(t => !string.IsNullOrWhiteSpace(t))
                           .ToList();

        // Expected order by start time: B (08:00), A (09:00-10:00), C (09:00-11:00 but ends later so after A)
        Assert.True(eventCells.Count >= 3);
        Assert.Equal("B", eventCells[0]);
        Assert.Equal("A", eventCells[1]);
        Assert.Equal("C", eventCells[2]);
    }
}
