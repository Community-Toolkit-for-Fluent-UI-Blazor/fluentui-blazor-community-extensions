namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides a composite importer that attempts to import a signature using multiple underlying importers. Returns the
/// first successful result.
/// </summary>
/// <remarks>This class allows chaining multiple signature importers and tries each in sequence until one
/// successfully imports the signature. If all importers fail or return no strokes, an empty result is returned. This is
/// useful for supporting multiple signature formats or fallback strategies.</remarks>
public class CompositeSurfaceImporter<TPayload>
{
    /// <summary>
    /// Represents the collection of signature importers used to process or import signatures.
    /// </summary>
    private readonly List<ISurfaceImporter<TPayload>> _importers = [];

    /// <summary>
    /// Adds the specified signature importer to the composite importer collection.
    /// </summary>
    /// <remarks>This method enables fluent configuration by returning the composite importer instance. The
    /// added importer will be used when importing signatures.</remarks>
    /// <param name="importer">The signature importer to add. Cannot be null.</param>
    /// <returns>The current instance of the composite signature importer, allowing for method chaining.</returns>
    public CompositeSurfaceImporter<TPayload> Add(ISurfaceImporter<TPayload> importer)
    {
        _importers.Add(importer);

        return this;
    }

    /// <inheritdoc />
    public async ValueTask<ImportSurfaceResult<TPayload>> ImportAsync(string input)
    {
        foreach (var importer in _importers)
        {
            if (!importer.IsValidInput(input))
            {
                continue;
            }

            var result = await importer.ImportAsync(input);

            if (result.Success)
            {
                return result;
            }
        }

        return new(ErrorMessage: "No valid importers found or all importers failed to import the signature.");
    }
}
