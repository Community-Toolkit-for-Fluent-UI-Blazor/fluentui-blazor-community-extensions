using FluentUI.Blazor.Community.Components.Charts.Drawing;
using Xunit;

namespace Components.Tests.Components.Charts.Drawing;

public class LinePathTests
{
    [Fact]
    public void LinePath_AssignsValues()
    {
        var path = new LinePath
        {
            Id = "path",
            Smooth = true,
            Points = [new ChartPoint(1, 2)]
        };

        Assert.Equal("path", path.Id);
        Assert.True(path.Smooth);
        Assert.Single(path.Points);
    }
}
