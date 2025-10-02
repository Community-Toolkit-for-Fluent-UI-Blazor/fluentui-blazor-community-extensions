using System.ComponentModel;
using System.Reflection;

namespace FluentUI.Blazor.Community.Components.Tests.Effects;

public class AngleUnitTests
{
    [Theory]
    [InlineData(AngleUnit.Degrees, "deg")]
    [InlineData(AngleUnit.Radians, "rad")]
    [InlineData(AngleUnit.Gradians, "grad")]
    [InlineData(AngleUnit.Turns, "turn")]
    public void AngleUnit_ShouldHaveCorrectDescription(AngleUnit unit, string expectedDescription)
    {
        var memberInfo = typeof(AngleUnit).GetMember(unit.ToString())[0];
        var descriptionAttribute = memberInfo.GetCustomAttribute<DescriptionAttribute>();

        Assert.NotNull(descriptionAttribute);
        Assert.Equal(expectedDescription, descriptionAttribute.Description);
    }
}
