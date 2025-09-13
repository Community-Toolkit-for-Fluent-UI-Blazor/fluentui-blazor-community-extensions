namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a collection of localized labels used for UI elements in a signature-related application.
/// </summary>
/// <remarks>This record provides a set of default labels in English, as well as predefined localized labels for
/// other languages, such as French. The labels can be customized by setting the properties to desired values.</remarks>
public record SignatureLabels
{
    /// <summary>
    /// Gets the default instance of <see cref="SignatureLabels"/> with labels in English.
    /// </summary>
    public static SignatureLabels Default => new();

    /// <summary>
    /// Gets an instance of <see cref="SignatureLabels"/> with labels localized in French.
    /// </summary>
    public static SignatureLabels French => new()
    {
        Export = "Exporter",
        Undo = "Annuler",
        Redo = "Rétablir",
        Clear = "Effacer",
        Eraser = "Gomme",
        Pen = "Stylo",
        Settings = "Paramètres",
        ExportSettings = "Paramètres d'exportation",
        Cancel = "Annuler",
        PenSettings = "Paramètres du stylo",
        EraserSettings = "Paramètres de la gomme",
        GridSettings = "Paramètres de la grille",
        WatermarkSettings = "Paramètres du filigrane",
        Apply = "Appliquer",
        ExportFormatLabel = "Format d'exportation",
        ExportQualityLabel = "Qualité d'exportation ({0:P})",
        BackgroundGridColorLabel = "Couleur de fond de la grille",
        GridColorLineLabel = "Couleur des lignes de la grille",
        GridCellSizeLabel = "Taille des cellules de la grille ({0}px)",
        GridOpacityLabel = "Opacité de la grille ({0:P})",
        BoldEveryLabel = "Épaissir chaque N-ième ligne",
        StrokeWidthLabel = "Largeur du trait ({0}px)",
        MarginLabel = "Marge ({0}px)",
        PointRadiusLabel = "Rayon du point ({0}px)",
        ShowAxesLabel = "Afficher les axes (X et Y)",
        GridDisplayModeLabel = "Mode d'affichage de la grille",
        GridDisplayModeNoneLabel = "Aucune",
        GridDisplayModeLinesLabel = "Lignes",
        GridDisplayModeDotsLabel = "Points",
        EraserOpacityLabel = "Opacité de la gomme ({0:P})",
        EraserShapeLabel = "Forme de la gomme",
        EraserShapeCircleLabel = "Cercle",
        EraserShapeSquareLabel = "Carré",
        EraserSizeLabel = "Taille de la gomme ({0}px)",
        EraserSoftEdgeLabel = "Bords doux de la gomme",
        EraserPartialEraseLabel = "Effacement partiel",
        PreviewPenLabel = "Aperçu du stylo",
        PenColorLabel = "Couleur du stylo",
        PenOpacityLabel = "Opacité du stylo ({0:P})",
        PressureEnabledLabel = "Sensibilité à la pression activée",
        SmoothingLabel = "Lissage activé",
        ShadowEnabledLabel = "Ombre activée",
        ShadowColorLabel = "Couleur de l'ombre",
        WatermarkTextLabel = "Texte du filigrane",
        WatermarkColorLabel = "Couleur du filigrane",
        WatermarkFontSizeLabel = "Taille de la police du filigrane ({0}px)",
        WatermarkOpacityLabel = "Opacité du filigrane ({0:P})",
        WatermarkRotationLabel = "Rotation du filigrane ({0}°)",
        WatermarkLetterSpacingLabel = "Espacement des lettres du filigrane ({0}px)",
        WatermakRepeatLabel = "Répéter le filigrane"
    };

    /// <summary>
    /// Gets or sets the label for the export button.
    /// </summary>
    public string Export { get; set; } = "Export";

    /// <summary>
    /// Gets or sets the label for the undo button.
    /// </summary>
    public string Undo { get; set; } = "Undo";

    /// <summary>
    /// Gets or sets the label for the redo button.
    /// </summary>
    public string Redo { get; set; } = "Redo";

    /// <summary>
    /// Gets or sets the label for the clear button.
    /// </summary>
    public string Clear { get; set; } = "Clear";

    /// <summary>
    /// Gets or sets the label for the eraser button.
    /// </summary>
    public string Eraser { get; set; } = "Eraser";

    /// <summary>
    /// Gets or sets the label for the pen button.
    /// </summary>
    public string Pen { get; set; } = "Pen";

    /// <summary>
    /// Gets or sets the label for the settings button.
    /// </summary>
    public string Settings { get; set; } = "Settings";

    /// <summary>
    /// Gets or sets the label for the export settings section.
    /// </summary>
    public string ExportSettings { get; set; } = "Export Settings";

    /// <summary>
    /// Gets or sets the label for the cancel button.
    /// </summary>
    public string Cancel { get; set; } = "Cancel";

    /// <summary>
    /// Gets or sets the label for the pen settings section.
    /// </summary>
    public string PenSettings { get; set; } = "Pen Settings";

    /// <summary>
    /// Gets or sets the label for the eraser settings section.
    /// </summary>
    public string EraserSettings { get; set; } = "Eraser Settings";

    /// <summary>
    /// Gets or sets the label for the grid settings section.
    /// </summary>
    public string GridSettings { get; set; } = "Grid Settings";

    /// <summary>
    /// Gets or sets the label for the watermark settings section.
    /// </summary>
    public string WatermarkSettings { get; set; } = "Watermark Settings";

    /// <summary>
    /// Gets or sets the label for the apply.
    /// </summary>
    public string Apply { get; set; } = "Apply";

    /// <summary>
    /// Gets or sets the label for the export format selection.
    /// </summary>
    public string ExportFormatLabel { get; set; } = "Export Format";

    /// <summary>
    /// Gets or sets the label for the export quality selection.
    /// </summary>
    public string ExportQualityLabel { get; set; } = "Export Quality ({0:P})";

    /// <summary>
    /// Gets or sets the label for the background grid color.
    /// </summary>
    public string BackgroundGridColorLabel { get; set; } = "Grid Background Color";

    /// <summary>
    /// Gets or sets the label for the grid line color.
    /// </summary>
    public string GridColorLineLabel { get; set; } = "Grid Line Color";

    /// <summary>
    /// Gets or sets the label for the grid cell size.
    /// </summary>
    public string GridCellSizeLabel { get; set; } = "Grid Cell Size ({0}px)";

    /// <summary>
    /// Gets or sets the label for the grid opacity.
    /// </summary>
    public string GridOpacityLabel { get; set; } = "Grid Opacity ({0:P})";

    /// <summary>
    /// Gets or sets the label for the Nth line bolding option.
    /// </summary>
    public string BoldEveryLabel { get; set; } = "Bold Every {0}th Line";

    /// <summary>
    /// Gets or sets the label for the width of the stroke.
    /// </summary>
    public string StrokeWidthLabel { get; set; } = "Stroke Width ({0}px)";

    /// <summary>
    /// Gets or sets the label for the margin.
    /// </summary>
    public string MarginLabel { get; set; } = "Margin ({0}px)";

    /// <summary>
    /// Gets or sets the label for the point radius.
    /// </summary>
    public string PointRadiusLabel { get; set; } = "Point Radius ({0}px)";

    /// <summary>
    /// Gets or sets the label for the axes visibility option.
    /// </summary>
    public string ShowAxesLabel { get; set; } = "Show Axes (X and Y)";

    /// <summary>
    /// Gets or sets the label for the grid display mode option.
    /// </summary>
    public string GridDisplayModeLabel { get; set; } = "Grid Display Mode";

    /// <summary>
    /// Gets or sets the label for the grid display mode "None" option.
    /// </summary>
    public string GridDisplayModeNoneLabel { get; set; } = "None";

    /// <summary>
    /// Gets or sets the label for the grid display mode "Lines" option.
    /// </summary>
    public string GridDisplayModeLinesLabel { get; set; } = "Lines";

    /// <summary>
    /// Gets or sets the label for the grid display mode "Dots" option.
    /// </summary>
    public string GridDisplayModeDotsLabel { get; set; } = "Dots";

    /// <summary>
    /// Gets or sets the label for the eraser shape option.
    /// </summary>
    public string EraserShapeLabel { get; set; } = "Eraser Shape";

    /// <summary>
    /// Gets or sets the label for the eraser shape "Circle" option.
    /// </summary>
    public string EraserShapeCircleLabel { get; set; } = "Circle";

    /// <summary>
    /// Gets or sets the label for the eraser shape "Square" option.
    /// </summary>
    public string EraserShapeSquareLabel { get; set; } = "Square";

    /// <summary>
    /// Gets or sets the label for the eraser size option.
    /// </summary>
    public string EraserSizeLabel { get; set; } = "Eraser Size ({0}px)";

    /// <summary>
    /// Gets or sets the label for the eraser opacity option.
    /// </summary>
    public string EraserOpacityLabel { get; set; } = "Eraser Opacity ({0:P})";

    /// <summary>
    /// Gets or sets the label for the eraser soft edges option.
    /// </summary>
    public string EraserSoftEdgeLabel { get; set; } = "Eraser Soft Edges";

    /// <summary>
    /// Gets or sets the label for the eraser partial erase option.
    /// </summary>
    public string EraserPartialEraseLabel { get; set; } = "Partial Erase";

    /// <summary>
    /// Gets or sets the label for the pen preview section.
    /// </summary>
    public string PreviewPenLabel { get; set; } = "Pen Preview";

    /// <summary>
    /// Gets or sets the label for the pen color selection.
    /// </summary>
    public string PenColorLabel { get; set; } = "Pen Color";

    /// <summary>
    /// Gets or sets the label for the pen opacity selection.
    /// </summary>
    public string PenOpacityLabel { get; set; } = "Pen Opacity ({0:P})";

    /// <summary>
    /// Gets or sets the label for the pressure sensitivity enable option.
    /// </summary>
    public string PressureEnabledLabel { get; set; } = "Pressure Sensitivity Enabled";

    /// <summary>
    /// Gets or sets the label for the smoothing enable option.
    /// </summary>
    public string SmoothingLabel { get; set; } = "Smoothing Enabled";

    /// <summary>
    /// Gets or sets the label for the shadow enable option.
    /// </summary>
    public string ShadowEnabledLabel { get; set; } = "Shadow Enabled";

    /// <summary>
    /// Gets or sets the label for the shadow color option.
    /// </summary>
    public string ShadowColorLabel { get; set; } = "Shadow Color";

    /// <summary>
    /// Gets or sets the label for the watermark text option.
    /// </summary>
    public string WatermarkTextLabel { get; set; } = "Watermark Text";

    /// <summary>
    /// Gets or sets the label for the watermark text color option.
    /// </summary>
    public string WatermarkColorLabel { get; set; } = "Watermark Color";

    /// <summary>
    /// Gets or sets the label for the watermark font size option.
    /// </summary>
    public string WatermarkFontSizeLabel { get; set; } = "Watermark Font Size ({0}px)";

    /// <summary>
    /// Gets or sets the label for the watermark opacity option.
    /// </summary>
    public string WatermarkOpacityLabel { get; set; } = "Watermark Opacity ({0:P})";

    /// <summary>
    /// Gets or sets the label for the watermark rotation option.
    /// </summary>
    public string WatermarkRotationLabel { get; set; } = "Watermark Rotation ({0}°)";

    /// <summary>
    /// Gets or sets the label for the watermark letter spacing option.
    /// </summary>
    public string WatermarkLetterSpacingLabel { get; set; } = "Watermark Letter Spacing ({0}px)";

    /// <summary>
    /// Gets or sets the label for the watermark repeat option.
    /// </summary>
    public string WatermakRepeatLabel { get; set; } = "Repeat Watermark";
}
