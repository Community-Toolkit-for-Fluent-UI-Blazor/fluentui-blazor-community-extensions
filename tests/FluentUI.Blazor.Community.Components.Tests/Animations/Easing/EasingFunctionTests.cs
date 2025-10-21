namespace FluentUI.Blazor.Community.Components.Tests.Animations;

public class EasingFunctionTests
{
    [Theory]
    [InlineData(EasingFunction.Linear)]
    [InlineData(EasingFunction.Back)]
    [InlineData(EasingFunction.Bounce)]
    [InlineData(EasingFunction.Circular)]
    [InlineData(EasingFunction.Cubic)]
    [InlineData(EasingFunction.Elastic)]
    [InlineData(EasingFunction.Exponential)]
    [InlineData(EasingFunction.Quadratic)]
    [InlineData(EasingFunction.Quartic)]
    [InlineData(EasingFunction.Quintic)]
    public void EasingFunction_ShouldContainAllExpectedValues(EasingFunction function)
    {
        Assert.True(Enum.IsDefined(function));
    }

    [Theory]
    [InlineData("Linear", EasingFunction.Linear)]
    [InlineData("Back", EasingFunction.Back)]
    [InlineData("Bounce", EasingFunction.Bounce)]
    [InlineData("Circular", EasingFunction.Circular)]
    [InlineData("Cubic", EasingFunction.Cubic)]
    [InlineData("Elastic", EasingFunction.Elastic)]
    [InlineData("Exponential", EasingFunction.Exponential)]
    [InlineData("Quadratic", EasingFunction.Quadratic)]
    [InlineData("Quartic", EasingFunction.Quartic)]
    [InlineData("Quintic", EasingFunction.Quintic)]
    public void EasingFunction_Parse_ShouldReturnCorrectEnum(string name, EasingFunction expected)
    {
        var result = Enum.Parse<EasingFunction>(name);
        Assert.Equal(expected, result);
    }
}
