using System.ComponentModel;
using System.Reflection;
using FluentUI.Blazor.Community.Components;
using Xunit;

namespace FluentUI.Blazor.Community.Tests.Components.Base;

public class AngleUnitTests
{
    [Theory]
    [InlineData(AngleUnit.Degrees, "deg")]
    [InlineData(AngleUnit.Radians, "rad")]
    [InlineData(AngleUnit.Gradians, "grad")]
    [InlineData(AngleUnit.Turns, "turn")]
    public void AngleUnit_HasExpectedDescription(AngleUnit unit, string expected)
    {
        var field = typeof(AngleUnit).GetField(unit.ToString());
        var description = field?.GetCustomAttribute<DescriptionAttribute>()?.Description;

        Assert.Equal(expected, description);
    }
}
