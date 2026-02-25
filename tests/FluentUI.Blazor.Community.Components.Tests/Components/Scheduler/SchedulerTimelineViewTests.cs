using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bunit;
using FluentUI.Blazor.Community.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Tests;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Scheduler;

public class SchedulerTimelineViewTests : TestBase
{
    public SchedulerTimelineViewTests()
    {
        JSInterop.Mode = Bunit.JSRuntimeMode.Loose;
        Services.AddSingleton(UnitTestLibraryConfiguration);
        Services.AddFluentUIComponents();
        Services.AddFluentCxUIComponents();
    }

    [Fact]
    public void Renders_cell_for_slot_within_timeline_range()
    {
        // Arrange
        var current = new DateTime(2025, 06, 10);
        var start = current.Date.AddHours(9);
        var end = start.AddHours(1);

        var slot = new SchedulerSlot("Meeting", start, end);
        var slots = new List<SchedulerSlot> { slot };

        // Act
        var cut = RenderComponent<SchedulerTimelineView>(parameters => parameters
            .Add(p => p.CurrentDate, current)
            .Add(p => p.Slots, slots)
            .Add(p => p.TimelineStart, TimeSpan.FromHours(8))
            .Add(p => p.TimelineEnd, TimeSpan.FromHours(17))
            .Add(p => p.SubdivisionCount, 1)
            .Add(p => p.CellWidth, 50)
        );

        // Assert
        var cells = cut.FindAll(".scheduler-timeline-cell");
        Assert.Single(cells);
        Assert.Contains("Meeting", cells[0].TextContent);
    }

    [Fact]
    public void Skips_slots_completely_outside_timeline()
    {
        // Arrange
        var current = new DateTime(2025, 06, 10);
        var before = current.Date.AddHours(0);
        var after = current.Date.AddHours(23);

        var slotBefore = new SchedulerSlot("Before", before, before.AddHours(1));
        var slotAfter = new SchedulerSlot("After", after, after.AddHours(1));
        var slots = new List<SchedulerSlot> { slotBefore, slotAfter };

        // Act
        var cut = RenderComponent<SchedulerTimelineView>(parameters => parameters
            .Add(p => p.CurrentDate, current)
            .Add(p => p.Slots, slots)
            .Add(p => p.TimelineStart, TimeSpan.FromHours(8))
            .Add(p => p.TimelineEnd, TimeSpan.FromHours(17))
            .Add(p => p.ShowNonWorkingHours, false)
            .Add(p => p.SubdivisionCount, 1)
        );

        // Assert: none rendered because both are outside 08:00-17:00
        var cells = cut.FindAll(".scheduler-timeline-cell");
        Assert.Empty(cells);
    }

    [Fact]
    public void Uses_slot_template_when_provided()
    {
        // Arrange
        var current = new DateTime(2025, 06, 10);
        var start = current.Date.AddHours(10);
        var slot = new SchedulerSlot("TplEvent", start, start.AddHours(1));
        var slots = new List<SchedulerSlot> { slot };

        RenderFragment<SchedulerSlot> template = s => __builder =>
        {
            __builder.AddMarkupContent(0, $"<div class='my-tpl'>TPL-{s.Label}</div>");
        };

        // Act
        var cut = RenderComponent<SchedulerTimelineView>(parameters => parameters
            .Add(p => p.CurrentDate, current)
            .Add(p => p.Slots, slots)
            .Add(p => p.SlotTemplate, template)
            .Add(p => p.SubdivisionCount, 1)
        );

        // Assert
        var tpl = cut.FindAll(".my-tpl").Select(e => e.TextContent.Trim());
        Assert.Contains("TPL-TplEvent", tpl);
    }

    [Fact]
    public void Double_click_invokes_OnSlotDoubleClick_callback()
    {
        // Arrange
        var current = new DateTime(2025, 06, 10);
        var start = current.Date.AddHours(11);
        var slot = new SchedulerSlot("ClickSlot", start, start.AddHours(1));
        var slots = new List<SchedulerSlot> { slot };

        var invoked = false;
        var callback = EventCallback.Factory.Create<SchedulerSlot>(this, (SchedulerSlot s) => { invoked = true; });

        var cut = RenderComponent<SchedulerTimelineView>(parameters => parameters
            .Add(p => p.CurrentDate, current)
            .Add(p => p.Slots, slots)
            .Add(p => p.OnSlotDoubleClick, callback)
            .Add(p => p.SubdivisionCount, 1)
        );

        var cell = cut.FindAll(".scheduler-timeline-cell").FirstOrDefault();
        Assert.NotNull(cell);

        // Act
        cell.TriggerEvent("ondblclick", new MouseEventArgs());

        // Assert
        Assert.True(invoked);
    }

    [Fact]
    public void GridWidth_reflects_total_subdivisions_times_cellwidth()
    {
        // Arrange
        var current = new DateTime(2025, 06, 10);
        var subdivisionsPerHour = 2; // 30-minute subdivisions
        var cellWidth = 40;
        var timelineStart = TimeSpan.FromHours(8);
        var timelineEnd = TimeSpan.FromHours(17); // 9 hours
        var expectedMinutes = (timelineEnd - timelineStart).TotalMinutes;
        var minutesPerSubdivision = 60.0 / subdivisionsPerHour;
        var expectedTotalSubdivisions = (int)Math.Round(expectedMinutes / minutesPerSubdivision);
        var expectedGridWidth = expectedTotalSubdivisions * cellWidth;

        // Act
        var cut = RenderComponent<SchedulerTimelineView>(parameters => parameters
            .Add(p => p.CurrentDate, current)
            .Add(p => p.Slots, new List<SchedulerSlot>())
            .Add(p => p.TimelineStart, timelineStart)
            .Add(p => p.TimelineEnd, timelineEnd)
            .Add(p => p.SubdivisionCount, subdivisionsPerHour)
            .Add(p => p.CellWidth, cellWidth)
            .Add(p => p.ShowNonWorkingHours, false)
        );

        // Assert: header contains width style with expected pixel value
        var header = cut.Find(".scheduler-timeline-header");
        var style = header.GetAttribute("style") ?? string.Empty;
        Assert.Contains($"width:{expectedGridWidth}px", style.Replace(" ", ""), StringComparison.OrdinalIgnoreCase);
    }
}
