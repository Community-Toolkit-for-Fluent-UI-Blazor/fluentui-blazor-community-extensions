using System.ComponentModel;

namespace FluentUI.Blazor.Community.Components.Tests.Effects;

public class LengthUnitTests
{
    [Theory]
    [InlineData(LengthUnit.Pixels, "px")]
    [InlineData(LengthUnit.Percent, "%")]
    [InlineData(LengthUnit.Em, "em")]
    [InlineData(LengthUnit.Rem, "rem")]
    [InlineData(LengthUnit.ViewportWidth, "vw")]
    [InlineData(LengthUnit.ViewportHeight, "vh")]
    [InlineData(LengthUnit.ViewportMin, "vmin")]
    [InlineData(LengthUnit.ViewportMax, "vmax")]
    [InlineData(LengthUnit.Centimeters, "cm")]
    [InlineData(LengthUnit.Millimeters, "mm")]
    [InlineData(LengthUnit.Inches, "in")]
    [InlineData(LengthUnit.Points, "pt")]
    [InlineData(LengthUnit.Picas, "pc")]
    [InlineData(LengthUnit.Character, "ch")]
    [InlineData(LengthUnit.XHeight, "ex")]
    public void LengthUnit_Should_Have_Correct_Description(LengthUnit unit, string expectedDescription)
    {
        // Récupère l'attribut Description de l'enum
        var type = typeof(LengthUnit);
        var memberInfo = type.GetMember(unit.ToString()).First();
        var descriptionAttribute = memberInfo.GetCustomAttributes(typeof(DescriptionAttribute), false)
                                            .Cast<DescriptionAttribute>()
                                            .FirstOrDefault();

        Assert.NotNull(descriptionAttribute);
        Assert.Equal(expectedDescription, descriptionAttribute.Description);
    }
}
