namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the style settings for a signature stroke, including both engine and rendering options.
/// </summary>
/// <remarks>Use this class to configure how signature strokes are generated and displayed. The style can be
/// cloned to create independent copies for customization or reuse.</remarks>
public sealed class SignatureStrokeStyle
{
    /// <summary>
    /// Gets or sets the engine style used for processing signature strokes.
    /// </summary>
    public SignatureStrokeEngineStyle Engine { get; set; } = new();

    /// <summary>
    /// Gets or sets the rendering style used for displaying signature strokes on the canvas.
    /// </summary>
    public SignatureStrokeRenderingStyle Rendering { get; set; } = new();

    /// <summary>
    /// Creates a new SignatureStrokeStyle object that is a deep copy of the current instance.
    /// </summary>
    /// <remarks>The returned object is independent of the original. Changes to the cloned instance do not
    /// affect the original SignatureStrokeStyle, and vice versa.</remarks>
    /// <returns>A new SignatureStrokeStyle instance with the same property values as the current object.</returns>
    public SignatureStrokeStyle Clone() => new()
    {
        Engine = Engine.Clone(),
        Rendering = Rendering.Clone()
    };
}

