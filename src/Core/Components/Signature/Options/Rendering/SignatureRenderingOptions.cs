namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the configuration options for a signature component, including settings for the signature grid and related options.
/// </summary>
public sealed class SignatureRenderingOptions : IDisposable
{
    /// <summary>
    /// Initializes a new instance of the SignatureRenderingOptions class.
    /// </summary>
    /// <remarks>Subscribes to the Pen.OptionsChanged event to handle changes in pen options. This ensures
    /// that any updates to pen settings are reflected in the rendering options.</remarks>
    public SignatureRenderingOptions()
    {
        Pen.OptionsChanged += OnOptionsChanged;
    }

    /// <summary>
    /// Gets or sets the options used to configure the background of the signature component.
    /// </summary>
    public SignatureBackgroundOptions Background { get; set; } = new();

    /// <summary>
    /// Gets or sets the configuration options for the signature grid component.
    /// </summary>
    public SignatureGridOptions Grid { get; set; } = new();

    /// <summary>
    /// Gets or sets the options for configuring the axes of the signature component.
    /// </summary>
    public SignatureAxesOptions Axes { get; set; } = new();

    /// <summary>
    /// Gets or sets the options used to configure the watermark applied to the signature.
    /// </summary>
    public SignatureWatermarkOptions Watermark { get; set; } = new();

    /// <summary>
    /// Gets or sets the options that defines the pan and zoom behavior for the signature component.
    /// </summary>
    public SignatureViewOptions View { get; set; } = new();

    /// <summary>
    /// Gets or sets the options that define the current signature selection behavior.
    /// </summary>
    public SignatureSelectionRenderingOptions Selection { get; set; } = new();

    /// <summary>
    /// Gets or sets the rendering options for the signature pen.
    /// </summary>
    /// <remarks>Use this property to configure the appearance and behavior of the pen used for signature
    /// rendering, such as color, thickness, and style. Modifying these options affects how the signature is
    /// displayed.</remarks>
    public SignaturePenRenderingOptions Pen { get; set; } = new();

    /// <summary>
    /// Gets or sets the options used to configure the appearance and behavior of the signature stroke layer.
    /// </summary>
    public SignatureStrokeLayerOptions StrokeLayer {  get; set; } = new();

    /// <summary>
    /// Gets or sets the debug options for signature processing.
    /// </summary>
    public SignatureDebugOptions Debug { get; set; } = new();

    /// <summary>
    /// Gets the rendering options for the hover state of the signature component.
    /// </summary>
    /// <remarks>Use this property to configure how the signature is visually presented when hovered by the
    /// user. The options provided by the SignatureHoverRenderingOptions class allow customization of appearance and
    /// behavior during hover interactions.</remarks>
    public SignatureHoverRenderingOptions Hover { get; init; } = new();

    /// <summary>
    /// Occurs when the options associated with this instance have changed.
    /// </summary>
    /// <remarks>Subscribe to this event to be notified when the options are modified, allowing dependent
    /// components to react accordingly.</remarks>
    public event EventHandler? OptionsChanged;

    /// <inheritdoc />
    public void Dispose()
    {
        Pen.OptionsChanged -= OnOptionsChanged;
    }

    /// <summary>
    /// Handles the event when the options are changed and raises the OptionsChanged event.
    /// </summary>
    /// <param name="sender">The source of the event. This parameter is typically the object that raised the event.</param>
    /// <param name="e">An EventArgs object that contains no event data.</param>
    private void OnOptionsChanged(object? sender, EventArgs e)
    {
        OptionsChanged?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Resets all configurable options and tools to their default values.
    /// </summary>
    /// <remarks>Call this method to restore the initial state of all settings and tools. This is useful for
    /// reverting any changes made during the session and ensuring a consistent starting point.</remarks>
    public void Reset()
    {
        Axes.Reset();
        Grid.Reset();
        Watermark.Reset();
        Selection.Reset();
        Background.Reset();
        Pen.Reset();
        Debug.Reset();
        Hover.Reset();
    }
}
