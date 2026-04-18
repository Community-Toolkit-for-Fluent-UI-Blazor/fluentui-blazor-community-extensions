using FluentUI.Blazor.Community.Components.ColorSpace.Icc;
using Xunit;

namespace Components.Tests.Components.ColorSpace.Icc;

public class IccProfileNameTests
{
    [Fact]
    public void IccProfileName_HasExpectedValues()
    {
        var values = Enum.GetValues<IccProfileName>();

        Assert.Equal(20, values.Length);
        Assert.Contains(IccProfileName.Adobe, values);
        Assert.Contains(IccProfileName.Srgb, values);
        Assert.Contains(IccProfileName.WideGamut, values);
    }
}
