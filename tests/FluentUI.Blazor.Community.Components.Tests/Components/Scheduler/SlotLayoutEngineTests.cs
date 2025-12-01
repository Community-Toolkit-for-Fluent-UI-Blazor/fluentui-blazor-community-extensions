using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Scheduler;

public class SlotLayoutEngineTests
{
    private static DateTime D(int year, int month, int day, int hour = 0, int minute = 0) =>
        new DateTime(year, month, day, hour, minute, 0);

    [Fact]
    public void ComputeLayout_NonOverlapping_Items_AllInColumnZero_WithColumnCountOne()
    {
        var date = D(2025, 11, 24);
        var a = new SchedulerItem<object> { Id = 1, Start = date.AddHours(9), End = date.AddHours(10) };
        var b = new SchedulerItem<object> { Id = 2, Start = date.AddHours(10), End = date.AddHours(11) };
        var items = new List<SchedulerItem<object>> { b, a }; // intentionally unsorted input

        var results = SlotLayoutEngine.ComputeLayout(items);

        Assert.Equal(2, results.Count);
        foreach (var r in results)
        {
            Assert.Equal(0, r.ColumnIndex);
            Assert.Equal(1, r.ColumnCount);
        }

        // items were sorted by start time so order in results matches a then b
        Assert.Equal(a.Id, results[0].Item.Id);
        Assert.Equal(b.Id, results[1].Item.Id);
    }

    [Fact]
    public void ComputeLayout_AdjacentItems_DoNotOverlap_SameColumn()
    {
        var date = D(2025, 11, 24);
        var a = new SchedulerItem<object> { Id = 1, Start = date.AddHours(8), End = date.AddHours(9) };
        var b = new SchedulerItem<object> { Id = 2, Start = date.AddHours(9), End = date.AddHours(10) }; // touches a.End
        var items = new List<SchedulerItem<object>> { a, b };

        var results = SlotLayoutEngine.ComputeLayout(items);

        Assert.Equal(2, results.Count);
        Assert.All(results, r => Assert.Equal(0, r.ColumnIndex));
        Assert.All(results, r => Assert.Equal(1, r.ColumnCount));
    }

    [Fact]
    public void ComputeLayout_OverlappingItems_AssignedDifferentColumns_And_GroupColumnCountSet()
    {
        var date = D(2025, 11, 24);
        var a = new SchedulerItem<object> { Id = 10, Start = date.AddHours(9), End = date.AddHours(10) };
        var b = new SchedulerItem<object> { Id = 20, Start = date.AddHours(9).AddMinutes(30), End = date.AddHours(10).AddMinutes(30) };
        var items = new List<SchedulerItem<object>> { a, b };

        var results = SlotLayoutEngine.ComputeLayout(items);

        Assert.Equal(2, results.Count);

        var ra = results.Single(r => r.Item.Id == a.Id);
        var rb = results.Single(r => r.Item.Id == b.Id);

        // Must be in different columns
        Assert.NotEqual(ra.ColumnIndex, rb.ColumnIndex);

        // ColumnCount must equal number of active columns in group (2)
        Assert.Equal(2, ra.ColumnCount);
        Assert.Equal(2, rb.ColumnCount);
    }

    [Fact]
    public void GroupOverlapping_Splits_Items_Into_Correct_Groups()
    {
        var date = D(2025, 11, 24);
        var a = new SchedulerItem<object> { Id = 1, Start = date.AddHours(8), End = date.AddHours(10) };
        var b = new SchedulerItem<object> { Id = 2, Start = date.AddHours(9), End = date.AddHours(11) }; // overlaps a
        var c = new SchedulerItem<object> { Id = 3, Start = date.AddHours(13), End = date.AddHours(14) }; // separate

        var items = new List<SchedulerItem<object>> { a, b, c };

        var groups = SlotLayoutEngine.GroupOverlapping(items);

        // Expect two groups: [a,b] and [c]
        Assert.Equal(2, groups.Count);
        Assert.Contains(groups, g => g.Count == 2 && g.Any(i => i.Id == a.Id) && g.Any(i => i.Id == b.Id));
        Assert.Contains(groups, g => g.Count == 1 && g[0].Id == c.Id);
    }

    [Fact]
    public void ComputeLayout_MultipleGroups_ColumnCounts_AreIndependent()
    {
        var date = D(2025, 11, 24);
        var g1a = new SchedulerItem<object> { Id = 11, Start = date.AddHours(8), End = date.AddHours(10) };
        var g1b = new SchedulerItem<object> { Id = 12, Start = date.AddHours(9), End = date.AddHours(11) };

        var g2a = new SchedulerItem<object> { Id = 21, Start = date.AddHours(13), End = date.AddHours(14) };
        var g2b = new SchedulerItem<object> { Id = 22, Start = date.AddHours(13).AddMinutes(15), End = date.AddHours(14).AddMinutes(15) };
        var g2c = new SchedulerItem<object> { Id = 23, Start = date.AddHours(14).AddMinutes(15), End = date.AddHours(15) };

        var items = new List<SchedulerItem<object>> { g1a, g1b, g2a, g2b, g2c };

        var results = SlotLayoutEngine.ComputeLayout(items);

        var r11 = results.Single(r => r.Item.Id == g1a.Id);
        var r12 = results.Single(r => r.Item.Id == g1b.Id);
        Assert.Equal(2, r11.ColumnCount);
        Assert.Equal(2, r12.ColumnCount);

        var r21 = results.Single(r => r.Item.Id == g2a.Id);
        var r22 = results.Single(r => r.Item.Id == g2b.Id);
        var r23 = results.Single(r => r.Item.Id == g2c.Id);
        Assert.Equal(2, r21.ColumnCount);
        Assert.Equal(2, r22.ColumnCount);
        Assert.Equal(1, r23.ColumnCount);
    }
}
