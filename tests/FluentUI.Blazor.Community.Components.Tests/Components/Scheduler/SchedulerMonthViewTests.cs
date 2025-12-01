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

public class SchedulerMonthViewTests : TestBase
{
    public SchedulerMonthViewTests()
    {
        JSInterop.Mode = Bunit.JSRuntimeMode.Loose;
        Services.AddSingleton(UnitTestLibraryConfiguration);
        Services.AddFluentUIComponents();
        Services.AddFluentCxUIComponents();
    }

    [Fact]
    public void Renders_day_label_when_slot_present()
    {
        // Arrange
        var date = new DateTime(2025, 06, 10);
        var slot = new SchedulerSlot("JourA", date, date);
        var slots = new List<SchedulerSlot> { slot };

        // Act
        var cut = RenderComponent<SchedulerMonthView>(parameters => parameters
            .Add(p => p.Date, date)
            .Add(p => p.Slots, slots)
        );

        // Assert
        var labels = cut.FindAll(".scheduler-day-label").Select(e => e.TextContent.Trim());
        Assert.Contains("JourA", labels);
    }

    [Fact]
    public void Uses_slot_template_when_provided()
    {
        // Arrange
        var date = new DateTime(2025, 06, 12);
        var slot = new SchedulerSlot("AvecTpl", date, date);
        var slots = new List<SchedulerSlot> { slot };

        RenderFragment<SchedulerSlot> template = s => __builder =>
        {
            __builder.AddMarkupContent(0, $"<span class='tpl'>TPL-{s.Label}</span>");
        };

        // Act
        var cut = RenderComponent<SchedulerMonthView>(parameters => parameters
            .Add(p => p.Date, date)
            .Add(p => p.Slots, slots)
            .Add(p => p.SlotTemplate, template)
        );

        // Assert
        var tpl = cut.FindAll("span.tpl").Select(e => e.TextContent.Trim());
        Assert.Contains("TPL-AvecTpl", tpl);
    }

    [Fact]
    public void Clicking_more_button_invokes_OnDaySelected_with_slot_start()
    {
        // Arrange
        var date = new DateTime(2025, 06, 15);
        var slot = new SchedulerSlot("OverflowDay", date, date, Disabled: false);
        var slots = new List<SchedulerSlot> { slot };
        var overflow = new Dictionary<DateTime, int> { [date.Date] = 3 };

        DateTime? selected = null;
        var callback = EventCallback.Factory.Create<DateTime>(this, (DateTime dt) => selected = dt);

        var cut = RenderComponent<SchedulerMonthView>(parameters => parameters
            .Add(p => p.Date, date)
            .Add(p => p.Slots, slots)
            .Add(p => p.OverflowByDay, overflow)
            .Add(p => p.OnDaySelected, callback)
        );

        // Find the rendered "more" button inside the scheduler-item-more container and click it
        var moreContainers = cut.FindAll(".scheduler-item-more");
        Assert.NotEmpty(moreContainers);

        var button = moreContainers.SelectMany(c => c.GetElementsByTagName("fluent-button")).FirstOrDefault();
        Assert.NotNull(button);

        // Act
        button!.Click();

        // Assert
        Assert.Equal(date.Date, selected?.Date);
    }

    [Fact]
    public void Double_click_invokes_OnSlotDoubleClick_and_disabled_slot_ignored()
    {
        // Arrange
        var date = new DateTime(2025, 06, 20);
        var enabledSlot = new SchedulerSlot("DoubleMe", date, date, Disabled: false);
        var disabledSlot = new SchedulerSlot("NoClick", date.AddDays(1), date.AddDays(1), Disabled: true);
        var slots = new List<SchedulerSlot> { enabledSlot, disabledSlot };

        var invokedCount = 0;
        var callback = EventCallback.Factory.Create<SchedulerSlot>(this, (SchedulerSlot s) => invokedCount++);

        var cut = RenderComponent<SchedulerMonthView>(parameters => parameters
            .Add(p => p.Date, date)
            .Add(p => p.Slots, slots)
            .Add(p => p.OnSlotDoubleClick, callback)
        );

        // find enabled cell and trigger double-click
        var enabledCell = cut.FindAll(".scheduler-day-cell")
                             .FirstOrDefault(e => e.TextContent?.Contains("DoubleMe") == true);
        Assert.NotNull(enabledCell);

        enabledCell!.TriggerEvent("ondblclick", new MouseEventArgs());

        // find disabled cell and trigger double-click
        var disabledCell = cut.FindAll(".scheduler-day-cell")
                              .FirstOrDefault(e => e.TextContent?.Contains("NoClick") == true);
        Assert.NotNull(disabledCell);

        disabledCell!.TriggerEvent("ondblclick", new MouseEventArgs());

        // Assert: only enabled slot invoked
        Assert.Equal(1, invokedCount);
    }
}
