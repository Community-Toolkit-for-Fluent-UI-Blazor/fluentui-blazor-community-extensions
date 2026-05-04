namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a pipeline for processing a collection of signature strokes through a sequence of stroke processors.
///  Each processor in the pipeline applies a transformation or analysis to the input strokes,
///  and the output of one processor serves as the input for the next processor in the sequence.
///  The final output is the result of processing the original strokes through all the configured processors in order.
/// </summary>
public sealed class StrokePipeline
{
    /// <summary>
    /// Represents the sequence of stroke processors that will be applied to the input strokes. 
    /// </summary>
    private readonly List<IStrokeProcessor> _processors;

    /// <summary>
    /// Initializes a new instance of the StrokePipeline class with the specified collection of stroke processors.
    /// </summary>
    /// <param name="processors">The collection of stroke processors to be used in the pipeline. Cannot be null.</param>
    public StrokePipeline(params IStrokeProcessor[] processors)
    {
        _processors = [.. processors];
    }

    /// <summary>
    /// Processes a sequence of signature strokes through a pipeline of processors.
    /// </summary>
    /// <remarks>Each processor in the pipeline is applied in sequence to the input strokes. The output of one
    /// processor is used as the input for the next.</remarks>
    /// <param name="strokes">The collection of signature strokes to process. Cannot be null.</param>
    /// <param name="engineOptions">The engine options that may influence the processing behavior. Cannot be null.</param>
    /// <returns>An enumerable collection of signature strokes resulting from the processing pipeline.</returns>
    public IEnumerable<SignatureStroke> Process(IEnumerable<SignatureStroke> strokes, SignatureEngineOptions engineOptions)
    {
        var current = strokes;

        foreach (var p in _processors)
        {
            current = p.Process(current, engineOptions);
        }

        return current;
    }
}
