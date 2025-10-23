namespace FluentUI.Blazor.Community.Components.Tests.Animations;

public class EasingModeTests
{
    [Theory]
    [InlineData(EasingMode.In)]
    [InlineData(EasingMode.Out)]
    [InlineData(EasingMode.InOut)]
    public void EasingMode_Enum_Values_Should_Be_Valid(EasingMode mode)
    {
        Assert.True(Enum.IsDefined(mode));
    }

    [Theory]
    [InlineData("In", EasingMode.In)]
    [InlineData("Out", EasingMode.Out)]
    [InlineData("InOut", EasingMode.InOut)]
    public void EasingMode_Parse_From_String_Should_Work(string value, EasingMode expected)
    {
        var parsed = Enum.Parse<EasingMode>(value);
        Assert.Equal(expected, parsed);
    }

    [Theory]
    [InlineData(EasingMode.In, 0)]
    [InlineData(EasingMode.Out, 1)]
    [InlineData(EasingMode.InOut, 2)]
    public void EasingMode_To_Int_Should_Match_Declaration_Order(EasingMode mode, int expectedValue)
    {
        Assert.Equal(expectedValue, (int)mode);
    }
}
