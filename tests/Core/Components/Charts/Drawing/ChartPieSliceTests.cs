using FluentUI.Blazor.Community.Components.Charts.Drawing;
using FluentUI.Blazor.Community.Components.Enums;
using Xunit;

namespace Components.Tests.Components.Charts.Drawing;

public class ChartPieSliceTests
{
    [Fact]
    public void ChartPieSlice_AssignsValues()
    {
        var slice = new ChartPieSlice
        {
            Id = "slice",
            StartAngle = 0,
            EndAngle = 90,
            MidAngle = 45,
            LabelPosition = new ChartPoint(1, 2),
            Index = 0,
            Value = 10,
            GroupId = "group",
            InteractionState = ChartInteractionState.Normal
        };

        Assert.Equal("slice", slice.Id);
        Assert.Equal(0, slice.StartAngle);
        Assert.Equal(90, slice.EndAngle);
        Assert.Equal(45, slice.MidAngle);
        Assert.Equal(new ChartPoint(1, 2), slice.LabelPosition);
        Assert.Equal(0, slice.Index);
        Assert.Equal(10, slice.Value);
        Assert.Equal("group", slice.GroupId);
        Assert.Equal(ChartInteractionState.Normal, slice.InteractionState);
    }
}
