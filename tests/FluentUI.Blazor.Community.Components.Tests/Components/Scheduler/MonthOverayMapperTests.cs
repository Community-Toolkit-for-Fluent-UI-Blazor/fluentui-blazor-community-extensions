using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Scheduler;

public class MonthPositionMapperTests
{
    private static Assembly ComponentsAssembly => typeof(ElementDimensions).Assembly;

    private static Type GetInternalType(string name) => ComponentsAssembly.GetType($"FluentUI.Blazor.Community.Components.{name}", throwOnError: true)!;

    private static object CreateMeasureLayout(float gap = 4f, float labelHeight = 10f, float cellHeight = 40f, float usableHeight = 30f, PaddingF? padding = null)
    {
        var measureType = GetInternalType("MeasureLayout");
        var instance = Activator.CreateInstance(measureType)!;

        // Set simple struct properties directly (SizeF, RectangleF, PointF are public types)
        measureType.GetProperty("Gap")!.SetValue(instance, gap);
        measureType.GetProperty("LabelSize")!.SetValue(instance, new SizeF(0f, labelHeight));
        measureType.GetProperty("CellSize")!.SetValue(instance, new SizeF(0f, cellHeight));
        measureType.GetProperty("UsableHeight")!.SetValue(instance, usableHeight);

        // Padding is internal PaddingF; create via reflection
        var paddingValue = padding ?? new PaddingF(2f, 2f, 2f, 2f);
        var paddingType = GetInternalType("PaddingF");
        var paddingCtor = paddingType.GetConstructor(new[] { typeof(float), typeof(float), typeof(float), typeof(float) })!;
        var paddingInstance = paddingCtor.Invoke(new object[] { paddingValue.Left, paddingValue.Top, paddingValue.Right, paddingValue.Bottom })!;
        measureType.GetProperty("Padding")!.SetValue(instance, paddingInstance);

        // other properties (LabelSize already set), ContentSize/Overlay/Local can remain default
        return instance;
    }

    private static object CreateSchedulerSlot(DateTime start)
    {
        return new SchedulerSlot(string.Empty, start, start.AddDays(1), false, 0, 0);
    }

    [Fact]
    public void Map_ReturnsEmpty_WhenSlotsNullOrEmpty()
    {
        // Arrange
        var mapperTypeDef = GetInternalType("MonthPositionMapper`1");
        var mapperType = mapperTypeDef.MakeGenericType(typeof(string));
        var itemsByDay = new Dictionary<DateTime, List<SchedulerItem<string>>>();
        var measure = CreateMeasureLayout();
        var ctor = mapperType.GetConstructor(new[] { typeof(int), typeof(Dictionary<DateTime, List<SchedulerItem<string>>>), GetInternalType("MeasureLayout"), typeof(int) })!;
        var mapper = ctor.Invoke(new object[] { 7, itemsByDay, measure, 3 });

        // Act
        var mapMethod = mapperType.GetMethod("Map", BindingFlags.Instance | BindingFlags.Public)!;
        var resultEmpty = (IEnumerable)mapMethod.Invoke(mapper, new object[] { null, null, new ElementDimensions(100, 100), DateTime.Today })!;

        // Assert
        Assert.NotNull(resultEmpty);
        Assert.Empty(resultEmpty);
    }

    [Fact]
    public void Map_SingleDayItem_ProducesOneRect_WithBothAnchorsTrue()
    {
        // Arrange: item that starts and ends same day
        var columns = 7;
        var date = new DateTime(2025, 11, 10);
        var item = new SchedulerItem<string>
        {
            Id = 1,
            Data = "p",
            Start = date,
            End = date,
            Title = "evt"
        };

        var itemsByDay = new Dictionary<DateTime, List<SchedulerItem<string>>>
        {
            [date.Date] = new List<SchedulerItem<string>> { item }
        };

        var measure = CreateMeasureLayout(gap: 2f, labelHeight: 8f, cellHeight: 50f, usableHeight: 30f, padding: new PaddingF(4f, 4f, 4f, 4f));
        var mapperType = GetInternalType("MonthPositionMapper`1").MakeGenericType(typeof(string));
        var ctor = mapperType.GetConstructor(new[] { typeof(int), typeof(Dictionary<DateTime, List<SchedulerItem<string>>>), GetInternalType("MeasureLayout"), typeof(int) })!;
        var mapper = ctor.Invoke(new object[] { columns, itemsByDay, measure, 5 });

        // Build slots array containing the date (as SchedulerSlot[])
        var slotType = GetInternalType("SchedulerSlot");
        var slotsArray = Array.CreateInstance(slotType, 1);
        var slot = CreateSchedulerSlot(date);
        slotsArray.SetValue(slot, 0);

        // Act
        var mapMethod = mapperType.GetMethod("Map", BindingFlags.Instance | BindingFlags.Public)!;
        var res = (IEnumerable<MappedItemRect>)mapMethod.Invoke(mapper, new object[] { slotsArray, item, new ElementDimensions(700, 400), date })!;

        // Assert
        var list = res.ToList();
        Assert.Single(list);

        var rect = list[0];
        Assert.True(rect.ShowLeftAnchor, "Start day should show left anchor");
        Assert.True(rect.ShowRightAnchor, "End day should show right anchor");
        Assert.True(rect.Rect.Width >= 0f);
        Assert.True(rect.Rect.Height >= 0f);
    }

    [Fact]
    public void Map_MultiDayItem_ProducesRects_PerDay_WithAnchorsOnEdges()
    {
        // Arrange: two-day span
        var columns = 7;
        var start = new DateTime(2025, 11, 10);
        var end = start.AddDays(1);
        var item = new SchedulerItem<string>
        {
            Id = 2,
            Data = "x",
            Start = start,
            End = end,
            Title = "two-day"
        };

        // Put the item into both days lists (ordered by Start in mapper constructor)
        var itemsByDay = new Dictionary<DateTime, List<SchedulerItem<string>>>
        {
            [start.Date] = new List<SchedulerItem<string>> { item },
            [end.Date] = new List<SchedulerItem<string>> { item }
        };

        var measure = CreateMeasureLayout(gap: 2f, labelHeight: 8f, cellHeight: 50f, usableHeight: 30f, padding: new PaddingF(3f, 3f, 3f, 3f));
        var mapperType = GetInternalType("MonthPositionMapper`1").MakeGenericType(typeof(string));
        var ctor = mapperType.GetConstructor(new[] { typeof(int), typeof(Dictionary<DateTime, List<SchedulerItem<string>>>), GetInternalType("MeasureLayout"), typeof(int) })!;
        var mapper = ctor.Invoke(new object[] { columns, itemsByDay, measure, 5 });

        // Build slots covering both days (two slots)
        var slotType = GetInternalType("SchedulerSlot");
        var slotsArray = Array.CreateInstance(slotType, 2);
        slotsArray.SetValue(CreateSchedulerSlot(start), 0);
        slotsArray.SetValue(CreateSchedulerSlot(end), 1);

        // Act
        var mapMethod = mapperType.GetMethod("Map", BindingFlags.Instance | BindingFlags.Public)!;
        var res = (IEnumerable<MappedItemRect>)mapMethod.Invoke(mapper, new object[] { slotsArray, item, new ElementDimensions(700, 400), start })!;

        // Assert - should produce two rects (one per day)
        var list = res.ToList();
        Assert.Equal(2, list.Count);

        // first day: left anchor true, right anchor false
        var first = list.First(r => r.ShowLeftAnchor);
        Assert.True(first.ShowLeftAnchor);
        Assert.False(first.ShowRightAnchor);

        // second day: right anchor true, left anchor false (there will be one with ShowRightAnchor true)
        var last = list.First(r => r.ShowRightAnchor);
        Assert.True(last.ShowRightAnchor);
        Assert.False(last.ShowLeftAnchor);
    }
}
