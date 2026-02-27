namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides configuration options for customizing the behavior of the signature engine, including pen, eraser, pressure
/// sensitivity, stabilization, smoothing, interpolation, and undo/redo features.
/// </summary>
/// <remarks>Use this class to specify detailed settings for each aspect of the signature engine. Each property
/// exposes a set of options that control a specific feature, allowing fine-tuned adjustments to the signature input
/// experience.</remarks>
public sealed class SignatureEngineOptions : IDisposable
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SignatureRenderingOptions" /> class.
    /// </summary>
    /// <remarks>Subscribes to the Pen.OptionsChanged event to handle changes in pen options. This ensures
    /// that any updates to pen settings are reflected in the rendering options.</remarks>
    public SignatureEngineOptions()
    {
        Pen.OptionsChanged += OptionsChanged;
    }

    /// <summary>
    /// Occurs when the options associated with the component change.
    /// </summary>
    /// <remarks>Subscribe to this event to be notified when the options are modified, allowing you to update
    /// dependent logic or UI elements as needed.</remarks>
    public event EventHandler? OptionsChanged;

    /// <summary>
    /// Gets or sets the options used to configure the eraser tool for the signature component.
    /// </summary>
    public SignatureEraserOptions Eraser { get; set; } = new();

    /// <summary>
    /// Gets or sets the options used to control signature interpolation behavior.
    /// </summary>
    public SignatureInterpolationOptions Interpolation { get; set; } = new();

    /// <summary>
    /// Gets or sets the options used to configure the appearance and behavior of the signature pen.
    /// </summary>
    public SignaturePenEngineOptions Pen { get; set; } = new();

    /// <summary>
    /// Gets or sets the options that control pressure sensitivity for the signature input.
    /// </summary>
    public SignaturePressureOptions Pressure { get; set; } = new();

    /// <summary>
    /// Gets or sets the options used to configure the selection.
    /// </summary>
    public SignatureSelectionEngineOptions Selection { get; set; } = new();

    /// <summary>
    /// Gets or sets the options used to control signature smoothing behavior.
    /// </summary>
    /// <remarks>Use this property to configure how smoothing is applied to the signature input, such as
    /// adjusting the level of curve smoothing or filtering. The effect of these options depends on the implementation
    /// of the signature component.</remarks>
    public SignatureSmoothingOptions Smoothing { get; set; } = new();

    /// <summary>
    /// Gets or sets the options used to control signature stabilization behavior.
    /// </summary>
    public SignatureStabilizationOptions Stabilization { get; set; } = new();

    /// <summary>
    /// Gets or sets the stroke options used to render the signature.
    /// </summary>
    /// <remarks>Use this property to customize the appearance of the signature, such as color, thickness, and
    /// style. Changing these options will affect how the signature is displayed.</remarks>
    public SignatureStrokeOptions Stroke { get; set; } = new();

    /// <summary>
    /// Gets or sets the options used to configure the signature surface.
    /// </summary>
    public SignatureSurfaceOptions Surface { get; set; } = new();

    /// <summary>
    /// Gets or sets the options that control undo and redo functionality for the signature component.
    /// </summary>
    public SignatureUndoRedoOptions UndoRedo { get; set; } = new();

    /// <summary>
    /// Gets or sets the options that define the viewport for the signature area.
    /// </summary>
    public SignatureViewportOptions Viewport { get; set; } = new();

    /// <inheritdoc />
    public void Dispose()
    {
        Pen.OptionsChanged -= OptionsChanged;
    }
}

