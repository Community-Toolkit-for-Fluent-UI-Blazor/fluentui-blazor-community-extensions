namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the labels used in the Signature component for localization.
/// </summary>
public record SignatureLabels
{
    /// <summary>
    /// Gets the default English labels.
    /// </summary>
    public static SignatureLabels Default { get; } = new();

    /// <summary>
    /// Gets the French labels.
    /// </summary>
    public static SignatureLabels French { get; } = new()
    {
        Maximize = "Agrandir",
        Clear = "Effacer",
        Undo = "Annuler",
        Redo = "Rétablir",
        Eraser = "Gomme",
        Pen = "Stylo",
        Export = "Exporter",
        Settings = "Paramètres",
        StrokeWidth = "Épaisseur du trait",
        PenOpacity = "Opacité du stylo",
        PenColor = "Couleur du stylo",
        StrokeLineStyle = "Style de ligne",
        SolidLine = "Solide",
        DottedLine = "Pointillé",
        DashedLine = "Tiret",
        ShowGrid = "Afficher la grille",
        ShowSeparatorLine = "Afficher la ligne de séparation",
        ShadowColor = "Couleur de l'ombrage",
        ShadowOpacity = "Opacité de l'ombrage",
        UsePointerPressure = "Utiliser la pression du stylet",
        UseShadow = "Utiliser l'ombrage",
        UseSmoothing = "Utiliser le lissage",
        ExportFormat = "Format d'exportation",
        SignatureSettings = "Paramètres de la signature"
    };

    /// <summary>
    /// Gets or sets the label for the maximize button.
    /// </summary>
    public string Maximize { get; set; } = "Maximize";

    /// <summary>
    /// Gets or sets the label for the clear button.
    /// </summary>
    public string Clear { get; set; } = "Clear";

    /// <summary>
    /// Gets or sets the label for the undo button.
    /// </summary>
    public string Undo { get; set; } = "Undo";

    /// <summary>
    /// Gets or sets the label for the redo button.
    /// </summary>
    public string Redo { get; set; } = "Redo";

    /// <summary>
    /// Gets or sets the label for the eraser button.
    /// </summary>
    public string Eraser { get; set; } = "Eraser";

    /// <summary>
    /// Gets or sets the label for the pen button.
    /// </summary>
    public string Pen { get; set; } = "Pen";

    /// <summary>
    /// Gets or sets the label for the pen opacity.
    /// </summary>
    public string PenOpacity { get; set; } = "Pen opacity";

    /// <summary>
    /// Gets or sets the label for the export button.
    /// </summary>
    public string Export { get; set; } = "Export";

    /// <summary>
    /// Gets or sets the label for the settings button.
    /// </summary>
    public string Settings { get; set; } = "Settings";

    /// <summary>
    /// Gets or sets the label for the stroke width.
    /// </summary>
    public string StrokeWidth { get; set; } = "Stroke width";

    /// <summary>
    /// Gets or sets the label for the pen color.
    /// </summary>
    public string PenColor { get; set; } = "Pen color";

    /// <summary>
    /// Gets or sets the label for the stroke line style.
    /// </summary>
    public string StrokeLineStyle { get; set; } = "Stroke line style";

    /// <summary>
    /// Gets or sets the label for solid line style.
    /// </summary>
    public string SolidLine { get; set; } = "Solid";

    /// <summary>
    /// Gets or sets the label for dotted line style.
    /// </summary>
    public string DottedLine { get; set; } = "Dotted";

    /// <summary>
    /// Gets or sets the label for dashed line style.
    /// </summary>
    public string DashedLine { get; set; } = "Dashed";

    /// <summary>
    /// Gets or sets the label for the show grid option.
    /// </summary>
    public string ShowGrid { get; set; } = "Show grid";

    /// <summary>
    /// Gets or sets the label for the show separator line option.
    /// </summary>
    public string ShowSeparatorLine { get; set; } = "Show separator line";

    /// <summary>
    /// Gets or sets the label for the use smoothing option.
    /// </summary>
    public string UseSmoothing { get; set; } = "Use smoothing";

    /// <summary>
    /// Gets or sets the label for the use pointer pressure option.
    /// </summary>
    public string UsePointerPressure { get; set; } = "Use pointer pressure";

    /// <summary>
    /// Gets or sets the label for the use shadow option.
    /// </summary>
    public string UseShadow { get; set; } = "Use shadow";

    /// <summary>
    /// Gets or sets the label for the shadow color option.
    /// </summary>
    public string ShadowColor { get; set; } = "Shadow color";

    /// <summary>
    /// Gets or sets the label for the shadow opacity option.
    /// </summary>
    public string ShadowOpacity { get; set; } = "Shadow opacity";

    /// <summary>
    /// Gets or sets the label for the export format option.
    /// </summary>
    public string ExportFormat { get; set; } = "Export format";

    /// <summary>
    /// Gets or sets the label for the signature settings section.
    /// </summary>
    public string SignatureSettings { get; set; } = "Signature settings";
}
