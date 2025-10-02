namespace FluentUI.Blazor.Community.Components.Tests.Components.Animations.Layouts;
public class SnakeLayoutTests
{
    [Fact]
    public void Update_SetsCorrectSnakeOffsets()
    {
        var layout = new SnakeLayout { Columns = 3, CellWidth = 10, CellHeight = 20 };
        var element = new AnimatedElement();

        layout.GetType().GetMethod("Update", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
    .Invoke(layout, [4, 9, element]);

        Assert.Equal(10, element.OffsetXState.EndValue);
        Assert.Equal(20, element.OffsetYState.EndValue);
    }
}
