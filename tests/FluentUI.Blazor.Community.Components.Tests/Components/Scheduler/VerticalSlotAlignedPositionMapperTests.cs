using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Scheduler;

public class VerticalSlotAlignedPositionMapperTests
{
    [Fact]
    public void Map_ReturnsEmpty_When_SlotsIsNull()
    {
        var mapper = new VerticalSlotAlignedPositionMapper<object>(
            showNonWorkingHours: true,
            workDayStart: TimeSpan.FromHours(8),
            workDayEnd: TimeSpan.FromHours(17));

        var item = new SchedulerItem<object> { Id = 1, Start = DateTime.Today, End = DateTime.Today.AddHours(1) };

        var result = mapper.Map([], item, new ElementDimensions(700f, 1000f), DateTime.Today);
        Assert.Empty(result);
    }

    [Fact]
    public void Map_ReturnsEmpty_When_SlotsIsEmpty()
    {
        var mapper = new VerticalSlotAlignedPositionMapper<object>(
            showNonWorkingHours: true,
            workDayStart: TimeSpan.FromHours(8),
            workDayEnd: TimeSpan.FromHours(17));

        var item = new SchedulerItem<object> { Id = 1, Start = DateTime.Today, End = DateTime.Today.AddHours(1) };

        var result = mapper.Map([], item, new ElementDimensions(700f, 1000f), DateTime.Today);
        Assert.Empty(result);
    }

    [Fact]
    public void Map_ReturnsEmpty_When_DateOutsideItemRange()
    {
        var mapper = new VerticalSlotAlignedPositionMapper<object>(
            showNonWorkingHours: true,
            workDayStart: TimeSpan.FromHours(8),
            workDayEnd: TimeSpan.FromHours(17));

        var item = new SchedulerItem<object>
        {
            Id = 1,
            Start = DateTime.Today.AddDays(1).AddHours(9),
            End = DateTime.Today.AddDays(1).AddHours(10)
        };

        var slots = new[] { new SchedulerSlot("s", DateTime.Today, DateTime.Today.AddHours(24)) };

        var result = mapper.Map(slots, item, new ElementDimensions(700f, 1000f), DateTime.Today);
        Assert.Empty(result);
    }

    [Fact]
    public void Map_MapsSingleDayItem_WithExpectedRect_When_ShowNonWorkingHoursTrue()
    {
        var mapper = new VerticalSlotAlignedPositionMapper<object>(
            showNonWorkingHours: true,
            workDayStart: TimeSpan.FromHours(8),
            workDayEnd: TimeSpan.FromHours(17),
            slotHeightPx: 60,
            subdivisionCount: 4);

        var date = new DateTime(2025, 11, 24);
        var item = new SchedulerItem<object>
        {
            Id = 42,
            Start = date.AddHours(9),
            End = date.AddHours(10).AddMinutes(30)
        };

        var slots = new[] { new SchedulerSlot("all", date, date.AddHours(24)) };
        var container = new ElementDimensions(700f, 1000f);

        var mapped = mapper.Map(slots, item, container, date).Single();

        // pixelsPerHour = slotHeightPx * subdivisionCount = 60 * 4 = 240
        var expectedTop = 9f * 240f;
        var expectedHeight = 1.5f * 240f;
        var expectedLeft = 8f; // margin = 8 (default)
        var expectedWidth = 700f - 2 * 8f;

        Assert.Equal(expectedTop, mapped.Rect.Top, 3);
        Assert.Equal(expectedHeight, mapped.Rect.Height, 3);
        Assert.Equal(expectedLeft, mapped.Rect.Left, 3);
        Assert.Equal(expectedWidth, mapped.Rect.Width, 3);

        Assert.True(mapped.ShowTopAnchor);
        Assert.True(mapped.ShowBottomAnchor);
    }

    [Fact]
    public void Map_Respects_SetLayout_Columns()
    {
        var mapper = new VerticalSlotAlignedPositionMapper<object>(
            showNonWorkingHours: true,
            workDayStart: TimeSpan.FromHours(8),
            workDayEnd: TimeSpan.FromHours(17),
            slotHeightPx: 60,
            subdivisionCount: 4);

        var date = DateTime.Today;
        var item = new SchedulerItem<object>
        {
            Id = 7,
            Start = date.AddHours(9),
            End = date.AddHours(10)
        };

        // Place item in column 1 of 3
        var layout = new List<SlotLayoutResult<object>>
        {
            new SlotLayoutResult<object> { Item = item, ColumnIndex = 1, ColumnCount = 3 }
        };

        mapper.SetLayout(layout);

        var slots = new[] { new SchedulerSlot("all", date, date.AddHours(24)) };
        var container = new ElementDimensions(300f, 1000f);

        var mapped = mapper.Map(slots, item, container, date).Single();

        var columnWidth = 300f / 3f;
        var expectedLeft = 1 * columnWidth + 8f;
        var expectedWidth = columnWidth - 16f; // -2*margin

        Assert.Equal(expectedLeft, mapped.Rect.Left, 3);
        Assert.Equal(expectedWidth, mapped.Rect.Width, 3);
    }

    [Fact]
    public void Map_ClipsToWorkDay_When_ShowNonWorkingHoursFalse()
    {
        var mapper = new VerticalSlotAlignedPositionMapper<object>(
            showNonWorkingHours: false,
            workDayStart: TimeSpan.FromHours(8),
            workDayEnd: TimeSpan.FromHours(17),
            slotHeightPx: 60,
            subdivisionCount: 4);

        var date = DateTime.Today;
        var item = new SchedulerItem<object>
        {
            Id = 9,
            Start = date.AddHours(7), // before workday
            End = date.AddHours(9)
        };

        var slots = new SchedulerSlot[24];

        for (var i = 0; i < 24; i++)
        {
            slots[i] = new SchedulerSlot(
                $"{i}:00",
                date.AddHours(i),
                date.AddHours(i + 1),
                i < 8 || i >= 17,
                i,
                0);
        }

        var container = new ElementDimensions(700f, 1000f);

        var mapped = mapper.Map(slots, item, container, date).Single();

        Assert.Equal(0f, mapped.Rect.Top, 3);
        Assert.Equal(240f, mapped.Rect.Height, 3);
    }
}
