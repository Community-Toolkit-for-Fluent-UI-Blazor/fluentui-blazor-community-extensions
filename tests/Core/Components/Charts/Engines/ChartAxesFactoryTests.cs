using FluentUI.Blazor.Community.Components.Charts.Engines;
using FluentUI.Blazor.Community.Components.Charts.Factories;
using Xunit;

namespace Components.Tests.Components.Charts.Engines;

public class ChartAxesFactoryTests
{
    [Fact]
    public void ChartAxesFactory_ReturnsInstances()
    {
        Assert.NotNull(ChartAxesFactory.Bar);
        Assert.NotNull(ChartAxesFactory.Column);
        Assert.NotNull(ChartAxesFactory.CategoryLine);
    }
}
