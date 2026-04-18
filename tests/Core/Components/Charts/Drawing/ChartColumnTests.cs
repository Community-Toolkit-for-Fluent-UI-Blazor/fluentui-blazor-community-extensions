using FluentUI.Blazor.Community.Components.Charts.Drawing;
using Xunit;

namespace Components.Tests.Components.Charts.Drawing;

public class ChartColumnTests
{
    [Fact]
    public void ChartColumn_AssignsValues()
    {
        var column = new ChartColumn
        {
            Id = "id",
            X = 1,
            Y = 2,
            Width = 3,
            Height = 4,
            CategoryIndex = 0,
            Value = 10
        };

        Assert.Equal("id", column.Id);
        Assert.Equal(1, column.X);
        Assert.Equal(2, column.Y);
        Assert.Equal(3, column.Width);
        Assert.Equal(4, column.Height);
        Assert.Equal(0, column.CategoryIndex);
        Assert.Equal(10, column.Value);
    }
}
