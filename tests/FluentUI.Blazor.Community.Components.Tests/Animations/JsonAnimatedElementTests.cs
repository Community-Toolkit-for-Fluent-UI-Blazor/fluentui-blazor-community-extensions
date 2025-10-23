using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Animations;

public class JsonAnimatedElementTests
{
    [Fact]
    public void Properties_SetAndGetValues()
    {
        var element = new JsonAnimatedElement
        {
            Id = "test",
            X = 1.0,
            Y = 2.0,
            ScaleX = 1.5,
            ScaleY = 2.5,
            Rotation = 45.0,
            Color = "#FF0000",
            BackgroundColor = "#00FF00",
            Opacity = 0.5,
            Value = 99.9
        };

        Assert.Equal("test", element.Id);
        Assert.Equal(1.0, element.X);
        Assert.Equal(2.0, element.Y);
        Assert.Equal(1.5, element.ScaleX);
        Assert.Equal(2.5, element.ScaleY);
        Assert.Equal(45.0, element.Rotation);
        Assert.Equal("#FF0000", element.Color);
        Assert.Equal("#00FF00", element.BackgroundColor);
        Assert.Equal(0.5, element.Opacity);
        Assert.Equal(99.9, element.Value);
    }
}
