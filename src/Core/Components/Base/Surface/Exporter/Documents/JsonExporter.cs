namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides functionality to export signature stroke data as a JSON file.
/// </summary>
/// <typeparam name="TPayload">The type of the payload containing the data to be exported. Must be a non-nullable type.</typeparam>
/// <remarks>This exporter serializes a collection of signature strokes to JSON format and packages the result for
/// download or further processing. It is typically used to enable users to save or transmit digital signatures in a
/// widely supported, text-based format.</remarks>
public sealed class JsonExporter<TPayload> : DocumentExporterBase<TPayload>
{
    /// <inheritdoc /> 
    /// <summary>
    /// Initializes a new instance of the <see cref="JsonExporter{TPayload}" /> class.
    /// </summary>
    public JsonExporter()
        : base(new JsonDocumentPayloadBuilder<TPayload>())
    { }

    /// <inheritdoc />
    protected override string GetFormatExtension()
    {
        return "json";
    }
}
