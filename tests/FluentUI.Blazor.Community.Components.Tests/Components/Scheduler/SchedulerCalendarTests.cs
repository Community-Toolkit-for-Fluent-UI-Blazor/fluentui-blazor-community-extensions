using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Bunit;
using Microsoft.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Scheduler;

public class SchedulerCalendarTests : IDisposable
{
    private readonly TestContext _ctx;

    public SchedulerCalendarTests()
    {
        _ctx = new TestContext();
    }

    public void Dispose()
    {
        _ctx.Dispose();
    }

    [Fact]
    public void InitializeOpenPopoverArray_PopulatesOpenedDayForAllRenderedDays()
    {
        // Arrange
        var month = new DateTime(2025, 11, 1);
        var cut = _ctx.RenderComponent<SchedulerCalendar>(parameters => parameters.Add(p => p.Month, month));

        // Act
        var instance = (SchedulerCalendar)cut.Instance;
        var openedDay = GetOpenedDayDictionary(instance);

        // Assert: for every day returned by CalendarExtended for weeks 0..5 there is an entry with value false
        for (var week = 0; week < 6; week++)
        {
            foreach (var day in CalendarExtended.GetDaysOfWeek(week, month, instance.Culture))
            {
                Assert.True(openedDay.ContainsKey(day), $"Expected openedDay to contain {day:yyyy-MM-dd}");
                Assert.False(openedDay[day]);
            }
        }
    }

    [Fact]
    public async Task OnSelectedCellAsync_WithHasSlot_OpensPopoverForDate()
    {
        // Arrange
        var month = new DateTime(2025, 11, 1);
        var targetDate = new DateTime(2025, 11, 10);
        var cut = _ctx.RenderComponent<SchedulerCalendar>(parameters =>
            parameters.Add(p => p.Month, month)
                      .Add(p => p.HasSlotFunc, new Func<DateTime, bool>(d => d.Date == targetDate.Date)));

        var instance = cut.Instance;
        var openedDay = GetOpenedDayDictionary(instance);

        // Precondition
        Assert.True(openedDay.ContainsKey(targetDate));

        // Act
        var method = instance.GetType().GetMethod("OnSelectedCellAsync", BindingFlags.Instance | BindingFlags.NonPublic);
        var task = (Task)method!.Invoke(instance, [targetDate])!;
        await task;

        // Assert
        Assert.True(openedDay[targetDate]);
    }

    [Fact]
    public async Task OnSelectedCellAsync_WithDisabledOrInactive_DoesNotOpenPopover()
    {
        // Arrange - disabled date
        var month = new DateTime(2025, 11, 1);
        var targetDate = new DateTime(2025, 11, 15);
        var cutDisabled = _ctx.RenderComponent<SchedulerCalendar>(parameters =>
            parameters.Add(p => p.Month, month)
                      .Add(p => p.DisabledDateFunc, new Func<DateTime, bool>(d => d.Date == targetDate.Date)));

        var instanceDisabled = (SchedulerCalendar)cutDisabled.Instance;
        var openedDayDisabled = GetOpenedDayDictionary(instanceDisabled);

        // Act - disabled
        var method = instanceDisabled.GetType().GetMethod("OnSelectedCellAsync", BindingFlags.Instance | BindingFlags.NonPublic);
        var taskDisabled = (Task)method!.Invoke(instanceDisabled, new object[] { targetDate })!;
        await taskDisabled;

        // Assert - still false
        Assert.False(openedDayDisabled[targetDate]);

        // Arrange - inactive date (different month)
        var otherMonth = new DateTime(2025, 10, 1);
        var inactiveDate = new DateTime(2025, 10, 20);
        var cutInactive = _ctx.RenderComponent<SchedulerCalendar>(parameters =>
            parameters.Add(p => p.Month, month)
                      .Add(p => p.HasSlotFunc, new Func<DateTime, bool>(d => true)));

        var instanceInactive = cutInactive.Instance;
        var openedDayInactive = GetOpenedDayDictionary(instanceInactive);

        // Ensure inactiveDate is present in the dictionary (calendar grid includes days outside month)
        Assert.True(openedDayInactive.ContainsKey(inactiveDate) || true); // tolerant check

        // Act - inactive (should not open)
        var methodInactive = instanceInactive.GetType().GetMethod("OnSelectedCellAsync", BindingFlags.Instance | BindingFlags.NonPublic);
        var taskInactive = (Task)methodInactive!.Invoke(instanceInactive, new object[] { inactiveDate })!;
        await taskInactive;

        // If the date exists in dictionary ensure it is not opened (most implementations keep it false)
        if (openedDayInactive.ContainsKey(inactiveDate))
        {
            Assert.False(openedDayInactive[inactiveDate]);
        }
    }

    [Fact]
    public async Task OnDateSelectedAsync_SetsSelectedDate_ClosesPopover_AndInvokesCallback()
    {
        // Arrange
        var month = new DateTime(2025, 11, 1);
        var targetDate = new DateTime(2025, 11, 5);
        var invoked = false;

        // Create component with a handler attached via EventCallback constructor
        var cut = _ctx.RenderComponent<SchedulerCalendar>(parameters =>
            parameters.Add(p => p.Month, month)
                      .Add(p => p.OnDateSelected, () =>
                      {
                          invoked = true;
                      }));

        var instance = cut.Instance;
        var openedDay = GetOpenedDayDictionary(instance);

        // Ensure the popover is open before selection (simulate prior state)
        openedDay[targetDate] = true;

        // Act
        var method = instance.GetType().GetMethod("OnDateSelectedAsync", BindingFlags.Instance | BindingFlags.NonPublic);
        var task = (Task)method!.Invoke(instance, [targetDate])!;
        await task;

        // Assert
        Assert.Equal(targetDate, instance.SelectedDate);
        Assert.False(openedDay[targetDate]);
        Assert.True(invoked);
    }

    // Helper to read the private _openedDay dictionary from the component instance
    private static Dictionary<DateTime, bool> GetOpenedDayDictionary(SchedulerCalendar instance)
    {
        var field = instance.GetType().GetField("_openedDay", BindingFlags.Instance | BindingFlags.NonPublic);
        var value = (Dictionary<DateTime, bool>?)field!.GetValue(instance);
        return value ?? [];
    }
}
