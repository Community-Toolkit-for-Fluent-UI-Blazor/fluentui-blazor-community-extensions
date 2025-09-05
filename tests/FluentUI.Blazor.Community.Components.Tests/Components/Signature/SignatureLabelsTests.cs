using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Signature;
public class SignatureLabelsTests
{
    [Fact]
    public void DefaultLabels_AreEnglish()
    {
        var labels = SignatureLabels.Default;

        Assert.Equal("Clear", labels.Clear);
        Assert.Equal("Undo", labels.Undo);
        Assert.Equal("Redo", labels.Redo);
        Assert.Equal("Eraser", labels.Eraser);
        Assert.Equal("Pen", labels.Pen);
        Assert.Equal("Pen opacity", labels.PenOpacity);
        Assert.Equal("Export", labels.Export);
        Assert.Equal("Settings", labels.Settings);
        Assert.Equal("Stroke width", labels.StrokeWidth);
        Assert.Equal("Pen color", labels.PenColor);
        Assert.Equal("Stroke line style", labels.StrokeLineStyle);
        Assert.Equal("Solid", labels.SolidLine);
        Assert.Equal("Dotted", labels.DottedLine);
        Assert.Equal("Dashed", labels.DashedLine);
        Assert.Equal("Grid", labels.Grid);
        Assert.Equal("Show separator line", labels.ShowSeparatorLine);
        Assert.Equal("Use smoothing", labels.UseSmoothing);
        Assert.Equal("Use pointer pressure", labels.UsePointerPressure);
        Assert.Equal("Use shadow", labels.UseShadow);
        Assert.Equal("Shadow color", labels.ShadowColor);
        Assert.Equal("Shadow opacity", labels.ShadowOpacity);
        Assert.Equal("Export format", labels.ExportFormat);
        Assert.Equal("Signature settings", labels.SignatureSettings);
        Assert.Equal("Display options", labels.DisplayOptions);
        Assert.Equal("Export options", labels.ExportOptions);
        Assert.Equal("No grid", labels.NoGrid);
        Assert.Equal("Lines", labels.LinesGrid);
        Assert.Equal("Dots", labels.DotsGrid);
    }

    [Fact]
    public void FrenchLabels_AreFrench()
    {
        var labels = SignatureLabels.French;

        Assert.Equal("Effacer", labels.Clear);
        Assert.Equal("Annuler", labels.Undo);
        Assert.Equal("Rétablir", labels.Redo);
        Assert.Equal("Gomme", labels.Eraser);
        Assert.Equal("Stylo", labels.Pen);
        Assert.Equal("Opacité du stylo", labels.PenOpacity);
        Assert.Equal("Exporter", labels.Export);
        Assert.Equal("Paramètres", labels.Settings);
        Assert.Equal("Épaisseur du trait", labels.StrokeWidth);
        Assert.Equal("Couleur du stylo", labels.PenColor);
        Assert.Equal("Style de ligne", labels.StrokeLineStyle);
        Assert.Equal("Solide", labels.SolidLine);
        Assert.Equal("Pointillé", labels.DottedLine);
        Assert.Equal("Tiret", labels.DashedLine);
        Assert.Equal("Grille", labels.Grid);
        Assert.Equal("Afficher la ligne de séparation", labels.ShowSeparatorLine);
        Assert.Equal("Utiliser le lissage", labels.UseSmoothing);
        Assert.Equal("Utiliser la pression du stylet", labels.UsePointerPressure);
        Assert.Equal("Utiliser l'ombrage", labels.UseShadow);
        Assert.Equal("Couleur de l'ombrage", labels.ShadowColor);
        Assert.Equal("Opacité de l'ombrage", labels.ShadowOpacity);
        Assert.Equal("Format d'exportation", labels.ExportFormat);
        Assert.Equal("Paramètres de la signature", labels.SignatureSettings);
        Assert.Equal("Options d'affichage", labels.DisplayOptions);
        Assert.Equal("Options d'exportation", labels.ExportOptions);
        Assert.Equal("Aucune", labels.NoGrid);
        Assert.Equal("Lignes", labels.LinesGrid);
        Assert.Equal("Points", labels.DotsGrid);
    }

    [Fact]
    public void CanSetAllProperties()
    {
        var labels = new SignatureLabels
        {
            Clear = "C",
            Undo = "U",
            Redo = "R",
            Eraser = "E",
            Pen = "P",
            PenOpacity = "PO",
            Export = "EX",
            Settings = "S",
            StrokeWidth = "SW",
            PenColor = "PC",
            StrokeLineStyle = "SLS",
            SolidLine = "SOL",
            DottedLine = "DOT",
            DashedLine = "DASH",
            Grid = "G",
            ShowSeparatorLine = "SSL",
            UseSmoothing = "US",
            UsePointerPressure = "UPP",
            UseShadow = "USH",
            ShadowColor = "SC",
            ShadowOpacity = "SO",
            ExportFormat = "EF",
            SignatureSettings = "SS",
            DisplayOptions = "DO",
            ExportOptions = "EO",
            NoGrid = "NG",
            LinesGrid = "LG",
            DotsGrid = "DG"
        };

        Assert.Equal("C", labels.Clear);
        Assert.Equal("U", labels.Undo);
        Assert.Equal("R", labels.Redo);
        Assert.Equal("E", labels.Eraser);
        Assert.Equal("P", labels.Pen);
        Assert.Equal("PO", labels.PenOpacity);
        Assert.Equal("EX", labels.Export);
        Assert.Equal("S", labels.Settings);
        Assert.Equal("SW", labels.StrokeWidth);
        Assert.Equal("PC", labels.PenColor);
        Assert.Equal("SLS", labels.StrokeLineStyle);
        Assert.Equal("SOL", labels.SolidLine);
        Assert.Equal("DOT", labels.DottedLine);
        Assert.Equal("DASH", labels.DashedLine);
        Assert.Equal("G", labels.Grid);
        Assert.Equal("SSL", labels.ShowSeparatorLine);
        Assert.Equal("US", labels.UseSmoothing);
        Assert.Equal("UPP", labels.UsePointerPressure);
        Assert.Equal("USH", labels.UseShadow);
        Assert.Equal("SC", labels.ShadowColor);
        Assert.Equal("SO", labels.ShadowOpacity);
        Assert.Equal("EF", labels.ExportFormat);
        Assert.Equal("SS", labels.SignatureSettings);
        Assert.Equal("DO", labels.DisplayOptions);
        Assert.Equal("EO", labels.ExportOptions);
        Assert.Equal("NG", labels.NoGrid);
        Assert.Equal("LG", labels.LinesGrid);
        Assert.Equal("DG", labels.DotsGrid);
    }
}
