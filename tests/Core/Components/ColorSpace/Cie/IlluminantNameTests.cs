using FluentUI.Blazor.Community.Components.ColorSpace.Cie;
using Xunit;

namespace Components.Tests.Components.ColorSpace.Cie;

public class IlluminantNameTests
{
    [Fact]
    public void IlluminantName_HasExpectedValues()
    {
        var values = Enum.GetValues<IlluminantName>();

        Assert.Equal(20, values.Length);
        Assert.Contains(IlluminantName.A, values);
        Assert.Contains(IlluminantName.D65, values);
        Assert.Contains(IlluminantName.F12, values);
    }
}
