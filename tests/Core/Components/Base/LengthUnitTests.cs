using System.ComponentModel;
using System.Reflection;
using FluentUI.Blazor.Community.Components;
using Xunit;

namespace FluentUI.Blazor.Community.Tests.Components.Base;

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
    public void LengthUnit_HasExpectedDescription(LengthUnit unit, string expected)
    {
        var field = typeof(LengthUnit).GetField(unit.ToString());
        var description = field?.GetCustomAttribute<DescriptionAttribute>()?.Description;

        Assert.Equal(expected, description);
    }
}
