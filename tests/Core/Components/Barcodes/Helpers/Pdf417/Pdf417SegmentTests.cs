using FluentUI.Blazor.Community.Components.Enums;
using FluentUI.Blazor.Community.Components.Helpers.Pdf417;
using Xunit;

namespace Components.Tests.Components.Barcodes.Helpers.Pdf417;

public class Pdf417SegmentTests
{
    [Fact]
    public void Segment_StoresModeAndContent()
    {
        var segment = new Pdf417Segment(Pdf417EncodingMode.Text, "ABC");

        Assert.Equal(Pdf417EncodingMode.Text, segment.Mode);
        Assert.Equal("ABC", segment.Content);
    }
}
