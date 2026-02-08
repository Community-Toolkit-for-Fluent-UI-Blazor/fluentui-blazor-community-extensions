using FluentUI.Blazor.Community.Components;
using Xunit;

namespace FluentUI.Blazor.Community.Tests.Components.Pictures;

public class PictureFetchPriorityTests
{
    [Theory]
    [InlineData(PictureFetchPriority.Auto, 0)]
    [InlineData(PictureFetchPriority.High, 1)]
    [InlineData(PictureFetchPriority.Low, 2)]
    public void PictureFetchPriority_HasExpectedValue(PictureFetchPriority value, int expected)
    {
        Assert.Equal(expected, (int)value);
    }
}
