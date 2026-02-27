namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides functionality to export signature stroke data to a binary file format using a predefined structure and
/// header.
/// </summary>
/// <typeparam name="TPayload">The type of the payload containing the data to be exported. Must be a non-nullable type.</typeparam>
/// <remarks>This class implements the ISignatureExporter interface to serialize signature data into a binary
/// format suitable for storage or transmission. The exported file includes a magic header and version information to
/// ensure compatibility and integrity. Use this exporter when a compact, binary representation of signature data is
/// required.</remarks>
public sealed class BinaryExporter<TPayload> : DocumentExporterBase<TPayload>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BinaryExporter{TPayload}" /> class.
    /// </summary>
    public BinaryExporter()
        : base(new BinaryDocumentBuilder<TPayload>())
    { }

    /// <inheritdoc />
    protected override string GetFormatExtension()
    {
        return "bin";
    }
}

