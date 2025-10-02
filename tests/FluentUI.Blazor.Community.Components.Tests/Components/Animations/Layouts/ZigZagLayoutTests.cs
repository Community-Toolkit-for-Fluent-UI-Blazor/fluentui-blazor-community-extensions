namespace FluentUI.Blazor.Community.Components.Tests.Components.Animations.Layouts;
public class ZigZagLayoutTests
{
    [Theory]
    [InlineData(0, 60, 30, 0, 60, 30)]
    [InlineData(1, 60, 30, 1, 60, -30)]
    public void Update_SetsZigZagOffsets(int index, double stepX, double stepY, int expectedIndex, double expectedX, double expectedY)
    {
        var layout = new ZigZagLayout { StepX = stepX, StepY = stepY };
        var element = new AnimatedElement();
        layout.GetType().GetMethod("Update", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
.Invoke(layout, [ index, 5, element ]);

        Assert.Equal(index * stepX, element.OffsetXState.EndValue, 4);
        Assert.Equal((index % 2 == 0 ? 1 : -1) * stepY, element.OffsetYState.EndValue, 4);
    }
}
