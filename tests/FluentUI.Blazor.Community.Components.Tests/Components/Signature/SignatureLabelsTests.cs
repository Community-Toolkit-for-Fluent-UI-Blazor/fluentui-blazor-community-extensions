using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Signature;

public class SignatureLabelsTests
{
    [Fact]
    public void Default_Should_Have_English_Labels()
    {
        var labels = SignatureLabels.Default;
        Assert.Equal("Export", labels.Export);
        Assert.Equal("Undo", labels.Undo);
        Assert.Equal("Redo", labels.Redo);
        Assert.Equal("Clear", labels.Clear);
        Assert.Equal("Eraser", labels.Eraser);
        Assert.Equal("Pen", labels.Pen);
        Assert.Equal("Settings", labels.Settings);
        Assert.Equal("Export Settings", labels.ExportSettings);
        Assert.Equal("Cancel", labels.Cancel);
        Assert.Equal("Pen Settings", labels.PenSettings);
        Assert.Equal("Eraser Settings", labels.EraserSettings);
        Assert.Equal("Grid Settings", labels.GridSettings);
        Assert.Equal("Watermark Settings", labels.WatermarkSettings);
        Assert.Equal("Apply", labels.Apply);
        Assert.Equal("Export Format", labels.ExportFormatLabel);
        Assert.Equal("Export Quality ({0:P})", labels.ExportQualityLabel);
        Assert.Equal("Grid Background Color", labels.BackgroundGridColorLabel);
        Assert.Equal("Grid Line Color", labels.GridColorLineLabel);
        Assert.Equal("Grid Cell Size ({0}px)", labels.GridCellSizeLabel);
        Assert.Equal("Grid Opacity ({0:P})", labels.GridOpacityLabel);
        Assert.Equal("Bold Every {0}th Line", labels.BoldEveryLabel);
        Assert.Equal("Stroke Width ({0}px)", labels.StrokeWidthLabel);
        Assert.Equal("Margin ({0}px)", labels.MarginLabel);
        Assert.Equal("Point Radius ({0}px)", labels.PointRadiusLabel);
        Assert.Equal("Show Axes (X and Y)", labels.ShowAxesLabel);
        Assert.Equal("Grid Display Mode", labels.GridDisplayModeLabel);
        Assert.Equal("None", labels.GridDisplayModeNoneLabel);
        Assert.Equal("Lines", labels.GridDisplayModeLinesLabel);
        Assert.Equal("Dots", labels.GridDisplayModeDotsLabel);
        Assert.Equal("Eraser Opacity ({0:P})", labels.EraserOpacityLabel);
        Assert.Equal("Eraser Shape", labels.EraserShapeLabel);
        Assert.Equal("Circle", labels.EraserShapeCircleLabel);
        Assert.Equal("Square", labels.EraserShapeSquareLabel);
        Assert.Equal("Eraser Size ({0}px)", labels.EraserSizeLabel);
        Assert.Equal("Eraser Soft Edges", labels.EraserSoftEdgeLabel);
        Assert.Equal("Partial Erase", labels.EraserPartialEraseLabel);
        Assert.Equal("Pen Preview", labels.PreviewPenLabel);
        Assert.Equal("Pen Color", labels.PenColorLabel);
        Assert.Equal("Pen Opacity ({0:P})", labels.PenOpacityLabel);
        Assert.Equal("Pressure Sensitivity Enabled", labels.PressureEnabledLabel);
        Assert.Equal("Smoothing Enabled", labels.SmoothingLabel);
        Assert.Equal("Shadow Enabled", labels.ShadowEnabledLabel);
        Assert.Equal("Shadow Color", labels.ShadowColorLabel);
        Assert.Equal("Watermark Text", labels.WatermarkTextLabel);
        Assert.Equal("Watermark Color", labels.WatermarkColorLabel);
        Assert.Equal("Watermark Font Size ({0}px)", labels.WatermarkFontSizeLabel);
        Assert.Equal("Watermark Opacity ({0:P})", labels.WatermarkOpacityLabel);
        Assert.Equal("Watermark Rotation ({0}°)", labels.WatermarkRotationLabel);
        Assert.Equal("Watermark Letter Spacing ({0}px)", labels.WatermarkLetterSpacingLabel);
        Assert.Equal("Repeat Watermark", labels.WatermakRepeatLabel);
    }

    [Fact]
    public void French_Should_Have_French_Labels()
    {
        var labels = SignatureLabels.French;
        Assert.Equal("Exporter", labels.Export);
        Assert.Equal("Annuler", labels.Undo);
        Assert.Equal("Rétablir", labels.Redo);
        Assert.Equal("Effacer", labels.Clear);
        Assert.Equal("Gomme", labels.Eraser);
        Assert.Equal("Stylo", labels.Pen);
        Assert.Equal("Paramètres", labels.Settings);
        Assert.Equal("Paramètres d'exportation", labels.ExportSettings);
        Assert.Equal("Annuler", labels.Cancel);
        Assert.Equal("Paramètres du stylo", labels.PenSettings);
        Assert.Equal("Paramètres de la gomme", labels.EraserSettings);
        Assert.Equal("Paramètres de la grille", labels.GridSettings);
        Assert.Equal("Paramètres du filigrane", labels.WatermarkSettings);
        Assert.Equal("Appliquer", labels.Apply);
        Assert.Equal("Format d'exportation", labels.ExportFormatLabel);
        Assert.Equal("Qualité d'exportation ({0:P})", labels.ExportQualityLabel);
        Assert.Equal("Couleur de fond de la grille", labels.BackgroundGridColorLabel);
        Assert.Equal("Couleur des lignes de la grille", labels.GridColorLineLabel);
        Assert.Equal("Taille des cellules de la grille ({0}px)", labels.GridCellSizeLabel);
        Assert.Equal("Opacité de la grille ({0:P})", labels.GridOpacityLabel);
        Assert.Equal("Épaissir chaque N-ième ligne", labels.BoldEveryLabel);
        Assert.Equal("Largeur du trait ({0}px)", labels.StrokeWidthLabel);
        Assert.Equal("Marge ({0}px)", labels.MarginLabel);
        Assert.Equal("Rayon du point ({0}px)", labels.PointRadiusLabel);
        Assert.Equal("Afficher les axes (X et Y)", labels.ShowAxesLabel);
        Assert.Equal("Mode d'affichage de la grille", labels.GridDisplayModeLabel);
        Assert.Equal("Aucune", labels.GridDisplayModeNoneLabel);
        Assert.Equal("Lignes", labels.GridDisplayModeLinesLabel);
        Assert.Equal("Points", labels.GridDisplayModeDotsLabel);
        Assert.Equal("Opacité de la gomme ({0:P})", labels.EraserOpacityLabel);
        Assert.Equal("Forme de la gomme", labels.EraserShapeLabel);
        Assert.Equal("Cercle", labels.EraserShapeCircleLabel);
        Assert.Equal("Carré", labels.EraserShapeSquareLabel);
        Assert.Equal("Taille de la gomme ({0}px)", labels.EraserSizeLabel);
        Assert.Equal("Bords doux de la gomme", labels.EraserSoftEdgeLabel);
        Assert.Equal("Effacement partiel", labels.EraserPartialEraseLabel);
        Assert.Equal("Aperçu du stylo", labels.PreviewPenLabel);
        Assert.Equal("Couleur du stylo", labels.PenColorLabel);
        Assert.Equal("Opacité du stylo ({0:P})", labels.PenOpacityLabel);
        Assert.Equal("Sensibilité à la pression activée", labels.PressureEnabledLabel);
        Assert.Equal("Lissage activé", labels.SmoothingLabel);
        Assert.Equal("Ombre activée", labels.ShadowEnabledLabel);
        Assert.Equal("Couleur de l'ombre", labels.ShadowColorLabel);
        Assert.Equal("Texte du filigrane", labels.WatermarkTextLabel);
        Assert.Equal("Couleur du filigrane", labels.WatermarkColorLabel);
        Assert.Equal("Taille de la police du filigrane ({0}px)", labels.WatermarkFontSizeLabel);
        Assert.Equal("Opacité du filigrane ({0:P})", labels.WatermarkOpacityLabel);
        Assert.Equal("Rotation du filigrane ({0}°)", labels.WatermarkRotationLabel);
        Assert.Equal("Espacement des lettres du filigrane ({0}px)", labels.WatermarkLetterSpacingLabel);
        Assert.Equal("Répéter le filigrane", labels.WatermakRepeatLabel);
    }

    [Fact]
    public void Properties_Should_Be_Settable()
    {
        var labels = new SignatureLabels
        {
            Export = "TestExport",
            Undo = "TestUndo"
        };
        Assert.Equal("TestExport", labels.Export);
        Assert.Equal("TestUndo", labels.Undo);
    }
}
