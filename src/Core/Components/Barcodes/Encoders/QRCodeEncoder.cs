using System.Text;
using FluentUI.Blazor.Community.Components.Helpers;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides functionality to encode input data into a QR code format using specified encoding options.
/// </summary>
/// <remarks>This encoder supports generating QR codes with configurable encoding modes, versions, and error
/// correction levels. It implements the IBarcodeEncoder interface for QR code generation. Instances of this class are
/// intended for internal use and are not thread-safe.</remarks>
internal sealed class QRCodeEncoder : IBarcodeEncoder<Barcode2DPayload, QRCodeOptions>
{
    /// <summary>
    /// Gets the singleton instance of the QRCodeEncoder class.
    /// </summary>
    public static QRCodeEncoder Instance { get; } = new();

    /// <inheritdoc />
    public Barcode2DPayload Encode(string data, QRCodeOptions options)
    {
        var mode = QRCodec.ResolveEncodingMode(data, options.EncodingMode);
        var rawBits = QRCodec.EncodeData(data, mode);
        var version = QRCodec.ResolveVersion(rawBits, options.Version, mode, options.ErrorCorrection);
        var bitStream = QRCodec.BuildFinalBitStream(rawBits, Encoding.UTF8.GetBytes(data).Length, version, mode, options.ErrorCorrection);
        var codewords = QRCodec.AddErrorCorrection(bitStream, version, options.ErrorCorrection);
        var matrix = QRCodec.BuildMatrix(version, codewords, options.ErrorCorrection);
        var payload = ToPayload(matrix, data);

        return payload;
    }

    /// <summary>
    /// Converts a two-dimensional matrix and associated data into a Barcode2DPayload object representing the barcode
    /// structure.
    /// </summary>
    /// <param name="matrix">A two-dimensional array of nullable Boolean values indicating the filled state of each cell in the barcode
    /// matrix. A value of <see langword="true"/> indicates a filled cell; <see langword="false"/> or <see
    /// langword="null"/> indicates an empty cell.</param>
    /// <param name="data">The data string to associate with the barcode payload. This value is assigned to the payload's Value property.</param>
    /// <returns>A Barcode2DPayload object containing the specified data and a collection of cells representing the barcode
    /// matrix.</returns>
    private static Barcode2DPayload ToPayload(bool[,] matrix, string data)
    {
        var optimizedMatrix = MatrixHelper.Optimize(matrix);

        return new Barcode2DPayload
        {
            Value = data,
            Rows = matrix.GetLength(0),
            Columns = matrix.GetLength(1),
            Modules = optimizedMatrix,
            Texts = []
        };
    }
}
