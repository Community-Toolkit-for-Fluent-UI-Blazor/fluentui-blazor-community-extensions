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

public class SchedulerWeekViewTests : TestBase
{
    public SchedulerWeekViewTests()
    {
        JSInterop.Mode = Bunit.JSRuntimeMode.Loose;
        Services.AddSingleton(UnitTestLibraryConfiguration);
        Services.AddFluentUIComponents();
        Services.AddFluentCxUIComponents();
    }

    private static List<DateTime> BuildWeekDays(DateTime start)
    {
        return Enumerable.Range(0, 7).Select(i => start.AddDays(i)).ToList();
    }

    [Fact]
    public void Renders_slot_label_when_slot_present()
    {
        // Arrange
        var weekStart = new DateTime(2025, 6, 2); // Monday
        var weekDays = BuildWeekDays(weekStart);

        var slotStart = new DateTime(2025, 6, 4, 10, 0, 0); // Wednesday 10:00
        var slot = new SchedulerSlot("EventA", slotStart, slotStart.AddHours(1), false, 10, 2);

        var slots = new List<SchedulerSlot> { slot };

        // Act
        var cut = RenderComponent<SchedulerWeekView>(parameters => parameters
            .Add(p => p.WeekDays, weekDays)
            .Add(p => p.Slots, slots)
            .Add(p => p.SubdivisionCount, 1)
        );

        // Assert
        var labels = cut.FindAll(".scheduler-week-day-label").Select(e => e.TextContent.Trim());
        Assert.Contains("EventA", labels);
    }

    [Fact]
    public void Uses_slot_template_when_provided()
    {
        // Arrange
        var weekStart = new DateTime(2025, 6, 2);
        var weekDays = BuildWeekDays(weekStart);

        var slotStart = new DateTime(2025, 6, 5, 14, 0, 0); // Thursday 14:00
        var slot = new SchedulerSlot("EventTpl", slotStart, slotStart.AddHours(1), false, 14, 3);

        var slots = new List<SchedulerSlot> { slot };

        RenderFragment<SchedulerSlot> template = s => __builder =>
        {
            __builder.AddMarkupContent(0, $"<span class='tpl'>TEMPLATE-{s.Label}</span>");
        };

        // Act
        var cut = RenderComponent<SchedulerWeekView>(parameters => parameters
            .Add(p => p.WeekDays, weekDays)
            .Add(p => p.Slots, slots)
            .Add(p => p.SlotTemplate, template)
            .Add(p => p.SubdivisionCount, 1)
        );

        // Assert
        var tpl = cut.FindAll("span.tpl").Select(e => e.TextContent.Trim());
        Assert.Contains("TEMPLATE-EventTpl", tpl);
    }

    [Fact]
    public void Renders_outside_working_hours_fill_for_slot_outside_hours()
    {
        // Arrange
        var weekStart = new DateTime(2025, 6, 2);
        var weekDays = BuildWeekDays(weekStart);

        // slot at 02:00 (outside default workday 08:00-17:00)
        var slotStart = new DateTime(2025, 6, 3, 2, 0, 0);
        var slot = new SchedulerSlot("NightEvent", slotStart, slotStart.AddHours(1), false, 2, 1);

        var cut = RenderComponent<SchedulerWeekView>(parameters => parameters
            .Add(p => p.WeekDays, weekDays)
            .Add(p => p.Slots, new List<SchedulerSlot> { slot })
            .Add(p => p.SubdivisionCount, 1)
        );

        // Assert: an outside-working-hours fill is rendered somewhere in the grid
        var fills = cut.FindAll(".outside-working-hours-fill");
        Assert.NotEmpty(fills);
    }

    [Fact]
    public void Double_click_invokes_OnSlotDoubleClick_callback()
    {
        // Arrange
        var weekStart = new DateTime(2025, 6, 2);
        var weekDays = BuildWeekDays(weekStart);

        var slotStart = new DateTime(2025, 6, 6, 11, 0, 0); // Friday 11:00
        var slot = new SchedulerSlot("ClickMe", slotStart, slotStart.AddHours(1), false, 11, 4);

        var slots = new List<SchedulerSlot> { slot };

        var called = false;
        var callback = EventCallback.Factory.Create<SchedulerSlot>(this, (SchedulerSlot s) => { called = true; });

        var cut = RenderComponent<SchedulerWeekView>(parameters => parameters
            .Add(p => p.WeekDays, weekDays)
            .Add(p => p.Slots, slots)
            .Add(p => p.OnSlotDoubleClick, callback)
            .Add(p => p.SubdivisionCount, 1)
        );

        // Find the cell that contains the slot label
        var cell = cut.FindAll(".scheduler-week-day")
                      .FirstOrDefault(e => e.TextContent?.Contains("ClickMe") == true);

        Assert.NotNull(cell);

        // Act: trigger double-click
        cell.TriggerEvent("ondblclick", new MouseEventArgs());

        // Assert
        Assert.True(called, "OnSlotDoubleClick should have been invoked on double-click.");
    }
}
