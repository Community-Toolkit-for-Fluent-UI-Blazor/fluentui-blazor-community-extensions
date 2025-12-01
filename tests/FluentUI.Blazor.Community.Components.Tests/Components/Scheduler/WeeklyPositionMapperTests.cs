using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Scheduler;

public class WeeklyPositionMapperTests
{
    private static MeasureLayout CreateDefaultMeasureLayout()
    {
        return new MeasureLayout
        {
            Overlay = new RectangleF(0, 0, 700, 1000),
            CellSize = new SizeF(100, 100),
            LabelSize = new SizeF(50, 20),
            ContentSize = new RectangleF(0, 0, 700, 800),
            Padding = new PaddingF(4f, 4f, 4f, 4f),
            Gap = 0f,
            UsableHeight = 800f,
            HeaderHeight = 48f,
            Local = new PointF(0, 0)
        };
    }

    [Fact]
    public void Map_ReturnsEmpty_When_SlotsIsNull()
    {
        var itemsByWeek = new Dictionary<(DateTime, int), List<SchedulerItem<object>>>();
        var layout = CreateDefaultMeasureLayout();
        var mapper = new WeekGridPositionMapper<object>(
            CultureInfo.InvariantCulture,
            showNonWorkingHour: true,
            startHour: TimeSpan.Zero,
            endHour: TimeSpan.FromHours(24),
            weekSlotHeight: 10,
            itemsByWeek: itemsByWeek,
            layout: layout,
            maxItemsCount: 5);

        var result = mapper.Map(null, null!, new ElementDimensions(700f, 1000f), DateTime.UtcNow);
        Assert.Empty(result);
    }

    [Fact]
    public void Map_ReturnsEmpty_When_SlotsIsEmpty()
    {
        var itemsByWeek = new Dictionary<(DateTime, int), List<SchedulerItem<object>>>();
        var layout = CreateDefaultMeasureLayout();
        var mapper = new WeekGridPositionMapper<object>(
            CultureInfo.InvariantCulture,
            showNonWorkingHour: true,
            startHour: TimeSpan.Zero,
            endHour: TimeSpan.FromHours(24),
            weekSlotHeight: 10,
            itemsByWeek: itemsByWeek,
            layout: layout,
            maxItemsCount: 5);

        var emptySlots = Array.Empty<SchedulerSlot>();
        var result = mapper.Map(emptySlots, null!, new ElementDimensions(700f, 1000f), DateTime.UtcNow);
        Assert.Empty(result);
    }

    [Fact]
    public void Map_ReturnsSingleMappedRect_ForSingleDayItem()
    {
        var startDate = new DateTime(2025, 11, 24); // monday
        var item = new SchedulerItem<object>
        {
            Id = 1,
            Title = "Meeting",
            Start = startDate.AddHours(9).AddMinutes(0),
            End = startDate.AddHours(10).AddMinutes(30)
        };

        var itemsByWeek = new Dictionary<(DateTime, int), List<SchedulerItem<object>>>
        {
            [(startDate, 0)] = new List<SchedulerItem<object>> { item }
        };

        var layout = CreateDefaultMeasureLayout();
        var mapper = new WeekGridPositionMapper<object>(
            CultureInfo.InvariantCulture,
            showNonWorkingHour: true,
            startHour: TimeSpan.Zero,
            endHour: TimeSpan.FromHours(24),
            weekSlotHeight: 20,
            itemsByWeek: itemsByWeek,
            layout: layout,
            maxItemsCount: 5);

        var slots = new[] { new SchedulerSlot("slot", startDate, startDate.AddHours(1)) };
        var results = mapper.Map(slots, item, new ElementDimensions(700f, 1000f), startDate).ToList();

        Assert.Single(results);
        var mapped = results.Single();

        // Anchors: same start and end date -> left & right anchors true; single-week -> top & bottom true
        Assert.True(mapped.ShowLeftAnchor);
        Assert.True(mapped.ShowRightAnchor);
        Assert.True(mapped.ShowTopAnchor);
        Assert.True(mapped.ShowBottomAnchor);

        // Rectangle should have positive width and height
        Assert.True(mapped.Rect.Width > 0);
        Assert.True(mapped.Rect.Height > 0);
    }

    [Fact]
    public void Map_DistributesWidth_ForOverlappingItems()
    {
        var startDate = new DateTime(2025, 11, 24);
        var item1 = new SchedulerItem<object>
        {
            Id = 10,
            Title = "A",
            Start = startDate.AddHours(9),
            End = startDate.AddHours(11)
        };

        var item2 = new SchedulerItem<object>
        {
            Id = 20,
            Title = "B",
            Start = startDate.AddHours(9),
            End = startDate.AddHours(11)
        };

        var itemsInKey = new List<SchedulerItem<object>> { item1, item2 };
        var itemsByWeek = new Dictionary<(DateTime, int), List<SchedulerItem<object>>>
        {
            [(startDate, 0)] = itemsInKey
        };

        var layout = CreateDefaultMeasureLayout();
        // use maxItemsCount = 2 so both items are rendered
        var mapper = new WeekGridPositionMapper<object>(
            CultureInfo.InvariantCulture,
            showNonWorkingHour: true,
            startHour: TimeSpan.Zero,
            endHour: TimeSpan.FromHours(24),
            weekSlotHeight: 20,
            itemsByWeek: itemsByWeek,
            layout: layout,
            maxItemsCount: 2);

        var slots = new[] { new SchedulerSlot("slot", startDate, startDate.AddHours(1)) };

        var r1 = mapper.Map(slots, item1, new ElementDimensions(700f, 1000f), startDate).Single();
        var r2 = mapper.Map(slots, item2, new ElementDimensions(700f, 1000f), startDate).Single();

        // Both items must have same width (availableWidth / 2 minus margins)
        Assert.InRange(Math.Abs(r1.Rect.Width - r2.Rect.Width), 0, 0.5f);

        // Lefts should not be equal (two columns)
        Assert.NotEqual(r1.Rect.Left, r2.Rect.Left);

        // Expected stride approximate calculation
        var cellWidth = (700f - (7 - 1)) / 7f;
        var availableWidth = cellWidth - layout.Padding.Left - layout.Padding.Right;
        var expectedStride = availableWidth / 2f;
        var expectedItemWidth = Math.Max(0f, expectedStride - 2f * 2f); // margin = 2

        Assert.InRange(r1.Rect.Width, expectedItemWidth - 1f, expectedItemWidth + 1f);
        Assert.InRange(r2.Rect.Width, expectedItemWidth - 1f, expectedItemWidth + 1f);
    }
}
