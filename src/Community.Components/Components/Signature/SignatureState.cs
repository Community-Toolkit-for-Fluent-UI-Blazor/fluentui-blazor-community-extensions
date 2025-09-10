
namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the current state of the signature, including all strokes made by the user.
/// </summary>
internal sealed class SignatureState
{
    /// <summary>
    /// Gets the list of strokes that make up the signature.
    /// </summary>
    public List<SignatureStroke> Strokes { get; } = [];

    /// <summary>
    /// Gets the stack of undone strokes for redo functionality.
    /// </summary>
    public Stack<SignatureStroke> RedoStrokes { get; } = [];

    /// <summary>
    /// Gets or sets the current tool selected for drawing on the signature canvas.
    /// </summary>
    public SignatureTool CurrentTool { get; set; }

    /// <summary>
    /// Gets a value indicating whether the grid is shown.
    /// </summary>
    public bool ShowGrid => GridType != SignatureGridType.None;

    /// <summary>
    /// Gets or sets the type of grid to display on the signature canvas.
    /// </summary>
    public SignatureGridType GridType { get; set; } = SignatureGridType.None;

    /// <summary>
    /// Represents the width of the stroke.
    /// </summary>
    public float StrokeWidth { get; set; }

    /// <summary>
    /// Gets or sets the color of the stroke.
    /// </summary>
    public string PenColor { get; set; } = string.Empty;

    /// <summary>
    /// Represents the opacity of the stroke.
    /// </summary>
    public float PenOpacity { get; set; }

    /// <summary>
    /// Gets or sets the style of the stroke.
    /// </summary>
    public SignatureLineStyle StrokeStyle { get; set; }

    /// <summary>
    /// Gets or sets a value indicating if the line separator is visible.
    /// </summary>
    public bool ShowSeparatorLine { get; set; }

    /// <summary>
    /// Gets or sets a value indicating if the signature use smoothing.
    /// </summary>
    public bool UseSmooth { get; set; }

    /// <summary>
    /// Gets or sets a value indicating if we use the pressure of the pointer.
    /// </summary>
    public bool UsePointerPressure { get; set; }

    /// <summary>
    /// Gets or sets a value indicating if the shadow is used.
    /// </summary>
    public bool UseShadow { get; set; }

    /// <summary>
    /// Gets or sets the opacity of the shadow.
    /// </summary>
    public float ShadowOpacity { get; set; }

    /// <summary>
    /// Gets or sets the color of the shadow.
    /// </summary>
    public string ShadowColor { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the quality of the exported image (1-100).
    /// </summary>
    public int Quality { get; set; } = 90;

    /// <summary>
    /// Gets or sets the format of the export.
    /// </summary>
    public SignatureExportFormat ExportFormat { get; set; } = SignatureExportFormat.Webp;

    /// <summary>
    /// Clears all strokes from the signature.
    /// </summary>
    public void Clear()
    {
        Strokes.Clear();
        RedoStrokes.Clear();
    }

    /// <summary>
    /// Updates the current signature settings based on the specified <see cref="SignatureSettings"/> instance.
    /// </summary>
    /// <remarks>This method updates various properties related to the signature tool, such as the tool type,
    /// grid visibility,  stroke width, pen color, and other visual and functional settings. The caller must ensure that
    /// the  <paramref name="signatureSettings"/> parameter is not null and contains valid values for the desired
    /// updates.</remarks>
    /// <param name="signatureSettings">An instance of <see cref="SignatureSettings"/> containing the new settings to apply.  The properties of this
    /// object determine the updated values for the signature tool, appearance, and behavior.</param>
    internal void Update(SignatureSettings signatureSettings)
    {
        CurrentTool = signatureSettings.Tool;
        GridType = signatureSettings.GridType;
        StrokeWidth = signatureSettings.StrokeWidth;
        PenColor = signatureSettings.PenColor;
        PenOpacity = signatureSettings.PenOpacity;
        StrokeStyle = signatureSettings.StrokeStyle;
        ShowSeparatorLine = signatureSettings.ShowSeparatorLine;
        UseSmooth = signatureSettings.Smooth;
        UsePointerPressure = signatureSettings.UsePointerPressure;
        UseShadow = signatureSettings.UseShadow;
        ShadowOpacity = signatureSettings.ShadowOpacity;
        ShadowColor = signatureSettings.ShadowColor;
    }

    /// <summary>
    /// Updates the current export settings based on the specified <see cref="SignatureExportSettings"/>.
    /// </summary>
    /// <param name="signatureExportSettings">The export settings to apply. This parameter must not be <see langword="null"/>.</param>
    internal void Update(SignatureExportSettings signatureExportSettings)
    {
        ExportFormat = signatureExportSettings.Format;
        Quality = signatureExportSettings.Quality;
    }

    /// <summary>
    /// Resets the signature settings to their default values and updates the current state accordingly.
    /// </summary>
    /// <param name="signatureSettings">Settings of the signature.</param>
    internal void Reset(SignatureSettings signatureSettings)
    {
        signatureSettings.Reset();
        Update(signatureSettings);
    }
}
