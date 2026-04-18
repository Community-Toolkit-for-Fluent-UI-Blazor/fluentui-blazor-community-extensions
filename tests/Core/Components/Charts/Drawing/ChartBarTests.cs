using FluentUI.Blazor.Community.Components.Charts.Drawing;
using Xunit;

namespace Components.Tests.Components.Charts.Drawing;

public class ChartBarTests
{
    [Fact]
    public void ChartBar_AssignsValues()
    {
        var bar = new ChartBar
        {
            Id = "id",
            X = 1,
            Y = 2,
            Width = 3,
            Height = 4,
            CategoryIndex = 0,
            Value = 10
        };

        Assert.Equal("id", bar.Id);
        Assert.Equal(1, bar.X);
        Assert.Equal(2, bar.Y);
        Assert.Equal(3, bar.Width);
        Assert.Equal(4, bar.Height);
        Assert.Equal(0, bar.CategoryIndex);
        Assert.Equal(10, bar.Value);
    }
}
