using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Scheduler;

public class SchedulerSlotTests
{
    [Fact]
    public void Constructor_Assigns_PositionalParameters_And_Defaults()
    {
        var start = new DateTime(2025, 11, 30, 9, 0, 0);
        var end = new DateTime(2025, 11, 30, 10, 0, 0);
        var slot = new SchedulerSlot("Meeting", start, end);

        Assert.Equal("Meeting", slot.Label);
        Assert.Equal(start, slot.Start);
        Assert.Equal(end, slot.End);
        Assert.False(slot.Disabled);
        Assert.Equal(0, slot.Row);
        Assert.Equal(0, slot.Column);
    }

    [Fact]
    public void Constructor_AllParameters_CanBeSet()
    {
        var start = new DateTime(2025, 12, 1, 8, 0, 0);
        var end = new DateTime(2025, 12, 1, 9, 30, 0);
        var slot = new SchedulerSlot("Block", start, end, true, 2, 3);

        Assert.Equal("Block", slot.Label);
        Assert.Equal(start, slot.Start);
        Assert.Equal(end, slot.End);
        Assert.True(slot.Disabled);
        Assert.Equal(2, slot.Row);
        Assert.Equal(3, slot.Column);
    }

    [Fact]
    public void TwoSlots_WithSameValues_AreEqual_ByValue()
    {
        var start = new DateTime(2025, 11, 30, 9, 0, 0);
        var end = new DateTime(2025, 11, 30, 10, 0, 0);

        var a = new SchedulerSlot("X", start, end, false, 0, 0);
        var b = new SchedulerSlot("X", start, end, false, 0, 0);

        Assert.Equal(a, b);
        Assert.True(a == b);
        Assert.Equal(a.GetHashCode(), b.GetHashCode());
    }

    [Fact]
    public void Deconstruct_Provides_All_PositionalValues()
    {
        var start = new DateTime(2025, 12, 2, 14, 0, 0);
        var end = new DateTime(2025, 12, 2, 15, 0, 0);
        var slot = new SchedulerSlot("Demo", start, end, true, 1, 4);

        var (label, s, e, disabled, row, col) = slot;

        Assert.Equal("Demo", label);
        Assert.Equal(start, s);
        Assert.Equal(end, e);
        Assert.True(disabled);
        Assert.Equal(1, row);
        Assert.Equal(4, col);
    }

    [Fact]
    public void WithExpression_Copies_And_Allows_Modification()
    {
        var start = new DateTime(2025, 12, 3, 10, 0, 0);
        var end = new DateTime(2025, 12, 3, 11, 0, 0);
        var original = new SchedulerSlot("Orig", start, end, false, 0, 0);

        var modified = original with { Label = "Mod", Column = 2 };

        Assert.Equal("Mod", modified.Label);
        Assert.Equal(2, modified.Column);
        // other values preserved
        Assert.Equal(original.Start, modified.Start);
        Assert.Equal(original.End, modified.End);
        Assert.Equal(original.Row, modified.Row);
        Assert.Equal(original.Disabled, modified.Disabled);
    }

    [Fact]
    public void ToString_Includes_PropertyNames_And_Values()
    {
        var start = new DateTime(2025, 12, 4, 9, 15, 0);
        var end = new DateTime(2025, 12, 4, 10, 15, 0);
        var slot = new SchedulerSlot("S", start, end, false, 0, 1);

        var s = slot.ToString();

        Assert.Contains("Label", s);
        Assert.Contains("Start", s);
        Assert.Contains("End", s);
        Assert.Contains("S", s);
        Assert.Contains(start.ToString(), s);
        Assert.Contains(end.ToString(), s);
    }
}
