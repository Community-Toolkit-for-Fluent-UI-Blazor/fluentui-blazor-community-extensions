namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents error correction information for a QR code, including the number of codewords per block and the number of
/// blocks.
/// </summary>
/// <remarks>This type is typically used to describe the error correction characteristics for a specific QR code
/// version and error correction level. It is intended for internal use within QR code encoding or decoding
/// operations.</remarks>
internal sealed class QREccInfo
{
    /// <summary>
    /// Gets the number of codewords contained in each block.
    /// </summary>
    public int DataCodewords { get; init; }

    /// <summary>
    /// Gets the total number of blocks represented by this instance.
    /// </summary>
    public int BlockCount { get; init; }

    /// <summary>
    /// Gets the number of error correction codewords used in the encoding process.
    /// </summary>
    public int EccCodewords { get; init; }
}

