using FluentUI.Blazor.Community.Components;
using FluentUI.Blazor.Community.Components.Enums;
using Xunit;

namespace Components.Tests.Components.Barcodes;

public class BarcodeTextTests
{
    [Fact]
    public void Defaults_AreConfigured()
    {
        var text = new BarcodeText();

        Assert.Equal(string.Empty, text.Text);
        Assert.Equal(SvgTextAnchor.Middle, text.Anchor);
        Assert.Equal(0, text.X);
        Assert.Equal(0, text.Y);
    }

    [Fact]
    public void Properties_SetInternalValues()
    {
        var text = new BarcodeText
        {
            X = 1,
            Y = 2,
            Text = "ABC",
            Anchor = SvgTextAnchor.End
        };

        Assert.Equal(1, text.X);
        Assert.Equal(2, text.Y);
        Assert.Equal("ABC", text.Text);
        Assert.Equal(SvgTextAnchor.End, text.Anchor);
    }
}
