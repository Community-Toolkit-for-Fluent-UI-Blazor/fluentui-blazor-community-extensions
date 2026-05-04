namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Defines a contract for processing a collection of signature strokes and returning the processed result.
/// </summary>
/// <remarks>Implementations of this interface can apply transformations, filtering, or analysis to digital ink
/// strokes, such as smoothing, simplification, or recognition. The specific processing behavior depends on the
/// implementation.</remarks>
public interface IStrokeProcessor
{
    /// <summary>
    /// Processes a collection of signature strokes and returns the resulting sequence after applying the transformation
    /// or analysis.
    /// </summary>
    /// <param name="strokes">The collection of signature strokes to process. Cannot be null.</param>
    /// <param name="options">The engine options that may influence the processing behavior. Cannot be null.</param>
    /// <returns>An enumerable collection of signature strokes representing the processed result. The returned sequence may be
    /// empty if no strokes are produced.</returns>
    IEnumerable<SignatureStroke> Process(IEnumerable<SignatureStroke> strokes, SignatureEngineOptions options);
}
