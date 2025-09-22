namespace FluentUI.Blazor.Community.Components.Tests.Components.Animations.Layouts;
public class PinLayoutTests
{
    [Fact]
    public void Update_SetsPinCoordinates()
    {
        var layout = new PinLayout { PinX = 42, PinY = 99 };
        var element = new AnimatedElement();

        layout.ApplyLayout([element]);

        Assert.Equal(42, element.OffsetXState.EndValue);
        Assert.Equal(99, element.OffsetYState.EndValue);
    }
}
