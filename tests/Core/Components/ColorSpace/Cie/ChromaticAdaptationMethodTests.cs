using FluentUI.Blazor.Community.Components.ColorSpace.Cie;
using Xunit;

namespace Components.Tests.Components.ColorSpace.Cie;

public class ChromaticAdaptationMethodTests
{
    [Fact]
    public void ChromaticAdaptationMethod_HasExpectedValues()
    {
        var values = Enum.GetValues<ChromaticAdaptationMethod>();

        Assert.Equal(3, values.Length);
        Assert.Contains(ChromaticAdaptationMethod.XYZScaling, values);
        Assert.Contains(ChromaticAdaptationMethod.VonKries, values);
        Assert.Contains(ChromaticAdaptationMethod.Bradford, values);
    }
}
