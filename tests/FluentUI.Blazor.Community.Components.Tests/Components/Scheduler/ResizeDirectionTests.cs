using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Scheduler;

public class SchedulerResizeDirectionTests
{
    [Fact]
    public void Enum_HasExpectedNumberOfValues()
    {
        var count = Enum.GetValues(typeof(ResizeDirection)).Length;
        Assert.Equal(4, count);
    }

    [Fact]
    public void Enum_UnderlyingValues_AreStable()
    {
        Assert.Equal(0, (int)ResizeDirection.Top);
        Assert.Equal(1, (int)ResizeDirection.Bottom);
        Assert.Equal(2, (int)ResizeDirection.Left);
        Assert.Equal(3, (int)ResizeDirection.Right);
    }

    [Theory]
    [InlineData("Top", ResizeDirection.Top)]
    [InlineData("Bottom", ResizeDirection.Bottom)]
    [InlineData("Left", ResizeDirection.Left)]
    [InlineData("Right", ResizeDirection.Right)]
    public void Can_Parse_String_To_Enum(string name, ResizeDirection expected)
    {
        var parsed = (ResizeDirection)Enum.Parse(typeof(ResizeDirection), name);
        Assert.Equal(expected, parsed);
    }

    [Theory]
    [InlineData(ResizeDirection.Top, "Top")]
    [InlineData(ResizeDirection.Bottom, "Bottom")]
    [InlineData(ResizeDirection.Left, "Left")]
    [InlineData(ResizeDirection.Right, "Right")]
    public void ToString_Returns_MemberName(ResizeDirection value, string expectedName)
    {
        Assert.Equal(expectedName, value.ToString());
    }
}
