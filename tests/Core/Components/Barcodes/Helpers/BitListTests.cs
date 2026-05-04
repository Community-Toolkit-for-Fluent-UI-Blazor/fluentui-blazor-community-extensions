using FluentUI.Blazor.Community.Components.Helpers;
using Xunit;

namespace Components.Tests.Components.Barcodes.Helpers;

public class BitListTests
{
    [Fact]
    public void AddBits_AppendsValuesInOrder()
    {
        var bits = new BitList();

        bits.Add(true);
        bits.Add(5, 3);

        var result = bits.ToArray();

        Assert.Equal(4, bits.Length);
        Assert.Equal(new[] { true, true, false, true }, result);
    }
}
