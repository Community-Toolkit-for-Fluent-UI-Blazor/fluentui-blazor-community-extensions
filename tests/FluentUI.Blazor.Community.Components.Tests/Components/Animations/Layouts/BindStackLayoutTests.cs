using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Animations.Layouts;
public class BindStackLayoutTests
{
    [Fact]
    public void Update_Should_Set_States_Correctly_When_Not_Reversed()
    {
        // Arrange
        var layout = new BindStackLayout
        {
            OffsetX = 10,
            OffsetY = 20,
            Spacing = 5,
            Reversed = false,
            VariantOpacity = 0.1
        };
        var element = new AnimatedElement();
        int index = 2;
        int count = 4;

        // Act
        layout.GetType().GetMethod("Update", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .Invoke(layout, new object[] { index, count, element });

        // Assert
        Assert.NotNull(element.OffsetXState);
        Assert.Equal(10 + index * 5, element.OffsetXState.EndValue);
        Assert.Equal(10, element.OffsetXState.StartValue);

        Assert.NotNull(element.OffsetYState);
        Assert.Equal(20 + index * 5, element.OffsetYState.EndValue);
        Assert.Equal(20, element.OffsetYState.StartValue);

        Assert.NotNull(element.OpacityState);
        Assert.Equal(1.0 - (index * 0.1), element.OpacityState.EndValue, 4);
        Assert.Equal(1.0, element.OpacityState.StartValue, 4);
    }

    [Fact]
    public void Update_Should_Set_States_Correctly_When_Reversed()
    {
        // Arrange
        var layout = new BindStackLayout
        {
            OffsetX = 0,
            OffsetY = 0,
            Spacing = 10,
            Reversed = true,
            VariantOpacity = 0.2
        };
        var element = new AnimatedElement();
        int index = 1;
        int count = 3;

        // Act
        layout.GetType().GetMethod("Update", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .Invoke(layout, new object[] { index, count, element });

        // Assert
        Assert.NotNull(element.OffsetXState);
        Assert.Equal(0 + (count - index) * 10, element.OffsetXState.EndValue);
        Assert.Equal(0, element.OffsetXState.StartValue);

        Assert.NotNull(element.OffsetYState);
        Assert.Equal(0 + (count - index) * 10, element.OffsetYState.EndValue);
        Assert.Equal(0, element.OffsetYState.StartValue);

        Assert.NotNull(element.OpacityState);
        Assert.Equal(1.0 - ((count - index - 1) * 0.2), element.OpacityState.EndValue, 4);
        Assert.Equal(1.0, element.OpacityState.StartValue, 4);
    }

    [Theory]
    [InlineData(0, 5, 0.05, false, 1.0)]
    [InlineData(2, 5, 0.05, false, 0.9)]
    [InlineData(0, 5, 0.05, true, 0.8)]
    [InlineData(2, 5, 0.05, true, 0.9)]
    public void Update_Should_Apply_VariantOpacity_Correctly(int index, int count, double variantOpacity, bool reversed, double expectedOpacity)
    {
        // Arrange
        var layout = new BindStackLayout
        {
            OffsetX = 0,
            OffsetY = 0,
            Spacing = 1,
            Reversed = reversed,
            VariantOpacity = variantOpacity
        };

        var element = new AnimatedElement();

        // Act
        layout.GetType().GetMethod("Update", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .Invoke(layout, [index, count, element]);

        // Assert
        Assert.NotNull(element.OpacityState);
        Assert.Equal(expectedOpacity, element.OpacityState.EndValue, 4);
    }
}
