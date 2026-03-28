using System.Text;
using FluentUI.Blazor.Community.Components;
using FluentUI.Blazor.Community.Components.Enums;
using Xunit;

namespace Components.Tests.Components.Barcodes.Helpers.QRCode;

public class QRCodecTests
{
    [Fact]
    public void ResolveEncodingMode_AutoSelectsNumeric()
    {
        var mode = QRCodec.ResolveEncodingMode("12345", QREncodingMode.Auto);

        Assert.Equal(QREncodingMode.Numeric, mode);
    }

    [Fact]
    public void ResolveEncodingMode_AutoSelectsAlphanumeric()
    {
        var mode = QRCodec.ResolveEncodingMode("HELLO-1", QREncodingMode.Auto);

        Assert.Equal(QREncodingMode.Alphanumeric, mode);
    }

    [Fact]
    public void ResolveEncodingMode_RespectsRequestedMode()
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

        var mode = QRCodec.ResolveEncodingMode("Hello!", QREncodingMode.Byte);

        Assert.Equal(QREncodingMode.Byte, mode);
    }

    [Fact]
    public void ResolveVersion_AutoReturnsVersion()
    {
        var dataBits = QRCodec.EncodeData("1234", QREncodingMode.Numeric);

        var version = QRCodec.ResolveVersion(dataBits, QRVersion.Auto, QREncodingMode.Numeric, QRErrorCorrectionLevel.Low);

        Assert.NotEqual(QRVersion.Auto, version);
    }
}
