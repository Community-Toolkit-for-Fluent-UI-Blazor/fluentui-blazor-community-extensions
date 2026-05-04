namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a pipeline that processes pointer input samples through a sequence of input processors.
/// </summary>
/// <remarks>The input pipeline applies each registered input processor in order to transform pointer samples into
/// signature points. This class is typically used to modularize and compose input processing logic, such as smoothing,
/// filtering, or stylus pressure adjustment, for digital ink or signature capture scenarios. Instances of this class
/// are immutable after construction and can be safely used across multiple input samples.</remarks>
public sealed class InputPipeline
{
    /// <summary>
    /// Represents the collection of input processors used to handle input operations.
    /// </summary>
    private readonly List<IInputProcessor> _processors;

    /// <summary>
    /// Initializes a new instance of the <see cref="InputPipeline"/> class with the specified input processors.
    /// </summary>
    /// <param name="processors">Parameters representing the input processors to be included in the pipeline.
    ///  The order of processors determines the sequence of processing applied to input samples.</param>
    public InputPipeline(params IInputProcessor[] processors)
    {
        _processors = [.. processors];
    }

    /// <summary>
    /// Generates a sequence of <see cref="SignaturePoint"/> instances by processing the given <see cref="PointerSample"/>
    /// </summary>
    /// <param name="sample">Pointer sample containing the raw input data to be processed into signature points.</param>
    /// <param name="style">Stroke style providing context for how the input sample should be processed, such as base width and pressure settings.</param>
    /// <returns>Returns an enumerable collection of <see cref="SignaturePoint"/> instances resulting from processing the input sample through the pipeline.</returns>
    public IEnumerable<SignaturePoint> Process(PointerSample sample, SignatureStrokeEngineStyle style)
    {
        IEnumerable<SignaturePoint> points =
        [
            new SignaturePoint(
                x: sample.X,
                y: sample.Y,
                pressure: sample.Pressure ?? 1.0,
                velocity: 0.0,
                width: style.BaseWidth,
                timestamp: sample.Timestamp)
        ];

        foreach (var processor in _processors)
        {
            points = processor.Process(points, sample, style);
        }

        return points;
    }
}

