using FluentUI.Blazor.Community.Components.ColorSpace.Cie;
using Xunit;

namespace Components.Tests.Components.ColorSpace.Cie;

public class IlluminantPointOfViewTests
{
    [Fact]
    public void IlluminantPointOfView_HasExpectedValues()
    {
        var values = Enum.GetValues<IlluminantPointOfView>();

        Assert.Equal(2, values.Length);
        Assert.Contains(IlluminantPointOfView.TwoDegrees, values);
        Assert.Contains(IlluminantPointOfView.TenDegrees, values);
    }
}
