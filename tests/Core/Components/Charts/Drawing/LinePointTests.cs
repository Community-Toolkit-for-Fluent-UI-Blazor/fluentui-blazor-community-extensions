using FluentUI.Blazor.Community.Components.Charts.Drawing;
using Xunit;

namespace Components.Tests.Components.Charts.Drawing;

public class LinePointTests
{
    [Fact]
    public void LinePoint_AssignsValues()
    {
        var point = new LinePoint
        {
            Id = "pt",
            X = 1,
            Y = 2,
            CategoryIndex = 0,
            Value = 5
        };

        Assert.Equal("pt", point.Id);
        Assert.Equal(1, point.X);
        Assert.Equal(2, point.Y);
        Assert.Equal(0, point.CategoryIndex);
        Assert.Equal(5, point.Value);
    }
}
