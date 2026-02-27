namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Defines a contract for processing input stroke points using additional pointer sample data.
/// </summary>
/// <remarks>Implementations of this interface can apply filtering, transformation, or enhancement to input stroke
/// data based on the provided pointer sample. This is typically used in scenarios such as digital ink processing or
/// signature capture, where input data may require normalization or analysis before further use.</remarks>
public interface IInputProcessor
{
    /// <summary>
    /// Processes a sequence of points using the specified pointer sample and returns the resulting collection of
    /// points.
    /// </summary>
    /// <param name="points">The collection of input stroke points to be processed. Cannot be null.</param>
    /// <param name="sample">The pointer sample that provides additional data for processing the stroke points. Cannot be null.</param>
    /// <param name="style">The stroke style that may influence how the points are processed, such as base width or pressure settings. Cannot be null.</param>
    /// <returns>An enumerable collection of processed points. The returned collection may be empty if no points are
    /// produced.</returns>
    IEnumerable<SignaturePoint> Process(
        IEnumerable<SignaturePoint> points,
        PointerSample sample,
        SignatureStrokeEngineStyle style);
}
