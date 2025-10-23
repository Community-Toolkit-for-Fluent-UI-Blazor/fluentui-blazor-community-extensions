using FluentUI.Blazor.Community.Helpers;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Animations.Layouts;
public class SunburstLayoutTests
{
    [Fact]
    public void Update_SetsRadialOffsets()
    {
        var layout = new SunburstLayout();
        layout.SetDimensions(200, 100);

        var element = new AnimatedElement();
        int count = 5;
        layout.GetType().GetMethod("Update", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
.Invoke(layout, [1, count, element]);

        double centerX = 100;
        double centerY = 50;
        double radiusStep = 50.0 / count;
        double angleStep = 360.0 / count;
        double angle = 1 * angleStep * MathHelper.Radians;
        double radius = radiusStep * (1 + 1);

        Assert.Equal(centerX + radius * Math.Cos(angle), element.OffsetXState.EndValue, 4);
        Assert.Equal(centerY + radius * Math.Sin(angle), element.OffsetYState.EndValue, 4);
    }
}
