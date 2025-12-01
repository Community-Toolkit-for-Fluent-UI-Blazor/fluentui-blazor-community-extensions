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

public class SchedulerDayViewTests : TestBase
{
    public SchedulerDayViewTests()
    {
        JSInterop.Mode = Bunit.JSRuntimeMode.Loose;
        Services.AddSingleton(UnitTestLibraryConfiguration);
        Services.AddFluentUIComponents();
        Services.AddFluentCxUIComponents();
    }

    [Fact]
    public void Renders_slot_time_for_slot_in_range()
    {
        // Arrange
        var current = new DateTime(2025, 06, 10);
        var slotStart = current.Date.AddHours(10).AddMinutes(30);
        var slot = new SchedulerSlot("Meet", slotStart, slotStart.AddHours(1));
        var slots = new List<SchedulerSlot> { slot };

        // Act
        var cut = RenderComponent<SchedulerDayView>(parameters => parameters
            .Add(p => p.CurrentDate, current)
            .Add(p => p.Slots, slots)
            .Add(p => p.SubdivisionCount, 1)
            .Add(p => p.ShowNonWorkingHours, true)
        );

        // Assert
        var slotElements = cut.FindAll(".scheduler-slot");
        Assert.Single(slotElements);
        Assert.Contains(slotStart.ToString("mm"), slotElements[0].TextContent);
    }

    [Fact]
    public void Does_not_render_slot_outside_working_hours_when_disabled()
    {
        // Arrange
        var current = new DateTime(2025, 06, 10);
        var early = current.Date.AddHours(2); // 02:00
        var slot = new SchedulerSlot("Early", early, early.AddHours(1));
        var slots = new List<SchedulerSlot> { slot };

        // Act
        var cut = RenderComponent<SchedulerDayView>(parameters => parameters
            .Add(p => p.CurrentDate, current)
            .Add(p => p.Slots, slots)
            .Add(p => p.SubdivisionCount, 1)
            .Add(p => p.ShowNonWorkingHours, false) // hide non-working hours
            .Add(p => p.WorkDayStart, TimeSpan.FromHours(8))
            .Add(p => p.WorkDayEnd, TimeSpan.FromHours(17))
        );

        // Assert
        var slotElements = cut.FindAll(".scheduler-slot");
        Assert.Empty(slotElements);
    }

    [Fact]
    public void Uses_slot_template_when_provided()
    {
        // Arrange
        var current = new DateTime(2025, 06, 10);
        var start = current.Date.AddHours(9);
        var slot = new SchedulerSlot("Tpl", start, start.AddHours(1));
        var slots = new List<SchedulerSlot> { slot };

        RenderFragment<SchedulerSlot> template = s => __builder =>
        {
            __builder.AddMarkupContent(0, $"<span class='tpl'>TEMPLATE-{s.Label}</span>");
        };

        // Act
        var cut = RenderComponent<SchedulerDayView>(parameters => parameters
            .Add(p => p.CurrentDate, current)
            .Add(p => p.Slots, slots)
            .Add(p => p.SlotTemplate, template)
            .Add(p => p.SubdivisionCount, 1)
            .Add(p => p.ShowNonWorkingHours, true)
        );

        // Assert
        var tpl = cut.FindAll("span.tpl").Select(e => e.TextContent.Trim());
        Assert.Contains("TEMPLATE-Tpl", tpl);
    }

    [Fact]
    public void Double_click_invokes_OnSlotDoubleClick_callback()
    {
        // Arrange
        var current = new DateTime(2025, 06, 10);
        var start = current.Date.AddHours(14);
        var slot = new SchedulerSlot("Click", start, start.AddHours(1));
        var slots = new List<SchedulerSlot> { slot };

        SchedulerSlot? received = null;
        var callback = EventCallback.Factory.Create<SchedulerSlot>(this, (SchedulerSlot s) => received = s);

        var cut = RenderComponent<SchedulerDayView>(parameters => parameters
            .Add(p => p.CurrentDate, current)
            .Add(p => p.Slots, slots)
            .Add(p => p.OnSlotDoubleClick, callback)
            .Add(p => p.SubdivisionCount, 1)
            .Add(p => p.ShowNonWorkingHours, true)
        );

        var cell = cut.FindAll(".scheduler-slot").FirstOrDefault();
        Assert.NotNull(cell);

        // Act
        cell!.TriggerEvent("ondblclick", new MouseEventArgs());

        // Assert
        Assert.NotNull(received);
        Assert.Equal(slot.Label, received!.Label);
    }

    [Fact]
    public void Renders_correct_number_of_hour_rows_when_non_working_hours_hidden()
    {
        // Arrange
        var current = new DateTime(2025, 06, 10);
        var startHour = 8;
        var endHour = 17;
        var expected = endHour - startHour; // 9 rows

        // Act
        var cut = RenderComponent<SchedulerDayView>(parameters => parameters
            .Add(p => p.CurrentDate, current)
            .Add(p => p.SubdivisionCount, 1)
            .Add(p => p.ShowNonWorkingHours, false)
            .Add(p => p.WorkDayStart, TimeSpan.FromHours(startHour))
            .Add(p => p.WorkDayEnd, TimeSpan.FromHours(endHour))
        );

        // Assert
        var hours = cut.FindAll(".scheduler-hour");
        Assert.Equal(expected, hours.Count);
    }
}
