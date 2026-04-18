using FluentUI.Blazor.Community.Components.ColorSpace.Vision;
using Xunit;

namespace Components.Tests.Components.ColorSpace.Vision;

public class ConfusionLineSetTests
{
    [Fact]
    public void ConfusionLineSet_ProvidesLines()
    {
        Assert.NotEqual(default, ConfusionLineSet.Protanopia);
        Assert.NotEqual(default, ConfusionLineSet.Deutanopia);
        Assert.NotEqual(default, ConfusionLineSet.Tritanopia);
        Assert.Equal(default, ConfusionLineSet.Unknown);
    }
}
