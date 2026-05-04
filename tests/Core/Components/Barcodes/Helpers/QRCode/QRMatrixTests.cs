using FluentUI.Blazor.Community.Components;
using FluentUI.Blazor.Community.Components.Enums;
using Xunit;

namespace Components.Tests.Components.Barcodes.Helpers.QRCode;

public class QRMatrixTests
{
    [Fact]
    public void BuildMatrix_CreatesExpectedSize()
    {
        var dataBits = QRCodec.EncodeData("HELLO", QREncodingMode.Alphanumeric);
        var version = QRCodec.ResolveVersion(dataBits, QRVersion.V1, QREncodingMode.Alphanumeric, QRErrorCorrectionLevel.Low);
        var stream = QRCodec.BuildFinalBitStream(dataBits, 5, version, QREncodingMode.Alphanumeric, QRErrorCorrectionLevel.Low);
        var codewords = QRCodec.AddErrorCorrection(stream, version, QRErrorCorrectionLevel.Low);
        var matrix = QRCodec.BuildMatrix(version, codewords, QRErrorCorrectionLevel.Low);

        Assert.Equal(21, matrix.GetLength(0));
        Assert.True(matrix[0, 0]);
    }
}
