using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Scheduler;

public class TimelinePositionMapperTests
{
    private static DateTime Day(int year, int month, int day) => new DateTime(year, month, day);

    [Fact]
    public void Ctor_Throws_When_SubdivisionCount_Invalid()
    {
        var start = TimeSpan.FromHours(8);
        var end = TimeSpan.FromHours(17);
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new TimelinePositionMapper<object>(0, true, start, end, new Dictionary<DateTime, List<SchedulerItem<object>>?>()));
    }

    [Fact]
    public void Ctor_Throws_When_TimelineEnd_NotGreaterThanStart()
    {
        var start = TimeSpan.FromHours(9);
        var end = TimeSpan.FromHours(9);
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new TimelinePositionMapper<object>(4, true, start, end, new Dictionary<DateTime, List<SchedulerItem<object>>?>()));
    }

    [Fact]
    public void Ctor_Throws_When_GetItemsByDay_Null()
    {
        var start = TimeSpan.FromHours(8);
        var end = TimeSpan.FromHours(17);
        Assert.Throws<ArgumentNullException>(() =>
            new TimelinePositionMapper<object>(4, true, start, end, null!));
    }

    [Fact]
    public void GetRequiredHeight_ReturnsZero_When_NoItemsForDate()
    {
        var map = new Dictionary<DateTime, List<SchedulerItem<object>>>();
        var mapper = new TimelinePositionMapper<object>(4, true, TimeSpan.FromHours(8), TimeSpan.FromHours(17), map);

        var h = mapper.GetRequiredHeight(Day(2025, 11, 25));
        Assert.Equal(0, h);
    }

    [Fact]
    public void GetRequiredHeight_ComputesWithOverlappingItems()
    {
        var date = Day(2025, 11, 25);
        var item1 = new SchedulerItem<object> { Id = 1, Start = date.AddHours(9), End = date.AddHours(11) };
        var item2 = new SchedulerItem<object> { Id = 2, Start = date.AddHours(10), End = date.AddHours(12) };

        var map = new Dictionary<DateTime, List<SchedulerItem<object>>> { [date] = new List<SchedulerItem<object>> { item1, item2 } };
        var mapper = new TimelinePositionMapper<object>(4, false, TimeSpan.FromHours(8), TimeSpan.FromHours(17), map);

        // Two overlapping items -> 2 rows. Height = rows*HeightPerItem + (rows-1)*gap
        var expected = 2 * 40 + (2 - 1) * 2;
        var height = mapper.GetRequiredHeight(date);
        Assert.Equal(expected, height);
    }

    [Fact]
    public void InvalidateDateLayout_ClearsCachedLayout()
    {
        var date = Day(2025, 11, 25);
        var item = new SchedulerItem<object> { Id = 1, Start = date.AddHours(9), End = date.AddHours(10) };
        var map = new Dictionary<DateTime, List<SchedulerItem<object>>> { [date] = new List<SchedulerItem<object>> { item } };
        var mapper = new TimelinePositionMapper<object>(4, false, TimeSpan.FromHours(8), TimeSpan.FromHours(17), map);

        // Force layout computation
        var h1 = mapper.GetRequiredHeight(date);
        Assert.True(h1 > 0);

        mapper.InvalidateDateLayout(date);

        // After invalidation, recomputing should still work and not throw; height should be same
        var h2 = mapper.GetRequiredHeight(date);
        Assert.Equal(h1, h2);
    }

    [Fact]
    public void Map_ReturnsExpectedRect_ForSingleItem_WithCorrectLeftAndWidth()
    {
        var date = Day(2025, 11, 24);
        // timeline 8:00 - 17:00 (9 hours)
        var subdivisionCount = 4; // 15 minutes per subdivision
        var minutesPerSubdivision = 60f / subdivisionCount;
        var dayStart = TimeSpan.FromHours(8);
        var dayEnd = TimeSpan.FromHours(17);
        var totalMinutes = (dayEnd - dayStart).TotalMinutes;
        var totalSubdivisions = (int)Math.Round(totalMinutes / minutesPerSubdivision);

        var item = new SchedulerItem<object>
        {
            Id = 100,
            Start = date.AddHours(9),
            End = date.AddHours(10).AddMinutes(30) // 1.5 hour
        };

        var map = new Dictionary<DateTime, List<SchedulerItem<object>>> { [date] = new List<SchedulerItem<object>> { item } };
        var mapper = new TimelinePositionMapper<object>(subdivisionCount, false, dayStart, dayEnd, map);

        var containerWidth = 360f; // choose 360 so cellWidth = 360 / totalSubdivisions is simple
        var container = new ElementDimensions(containerWidth, 200f);

        var slots = new[] { new SchedulerSlot("all", date, date.AddHours(24)) };
        var results = mapper.Map(slots, item, container, date).ToList();

        Assert.Single(results);
        var rect = results.Single().Rect;

        var cellWidth = containerWidth / totalSubdivisions;
        // compute expected subdivisions:
        var startMinutes = (item.Start - date).TotalMinutes - dayStart.TotalMinutes; // (9-8)*60 = 60
        var endMinutes = (item.End - date).TotalMinutes - dayStart.TotalMinutes;     // (10.5-8)*60 = 150
        var startSubdivision = (int)Math.Floor(startMinutes / minutesPerSubdivision); // 60/15 = 4
        var endSubdivision = (int)Math.Ceiling(endMinutes / minutesPerSubdivision);   // 150/15 = 10

        var expectedLeft = startSubdivision * cellWidth;
        var expectedWidth = Math.Max(0f, (endSubdivision - startSubdivision) * cellWidth);

        Assert.Equal(expectedLeft, rect.Left, 3);
        Assert.Equal(expectedWidth, rect.Width, 3);

        // Anchors: both start and end on same date
        Assert.True(results.Single().ShowLeftAnchor);
        Assert.True(results.Single().ShowRightAnchor);
        Assert.False(results.Single().ShowTopAnchor);
        Assert.False(results.Single().ShowBottomAnchor);
    }
}
