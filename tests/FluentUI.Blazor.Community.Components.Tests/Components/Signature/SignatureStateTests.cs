using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Signature;
public class SignatureStateTests
{
    [Fact]
    public void Clear_RemovesAllStrokesAndRedoStrokes()
    {
        // Arrange
        var state = new SignatureState();
        state.Strokes.Add(new SignatureStroke());
        state.RedoStrokes.Push(new SignatureStroke());

        // Act
        state.Clear();

        // Assert
        Assert.Empty(state.Strokes);
        Assert.Empty(state.RedoStrokes);
    }

    [Fact]
    public void Update_WithSignatureSettings_UpdatesAllProperties()
    {
        // Arrange
        var state = new SignatureState();
        var settings = new SignatureSettings
        {
            Tool = SignatureTool.Eraser,
            GridType = SignatureGridType.Dots,
            StrokeWidth = 5.5f,
            PenColor = "#123456",
            PenOpacity = 0.7f,
            StrokeStyle = SignatureLineStyle.Dashed,
            ShowSeparatorLine = true,
            Smooth = false,
            UsePointerPressure = true,
            UseShadow = true,
            ShadowOpacity = 0.3f,
            ShadowColor = "#654321"
        };

        // Act
        state.Update(settings);

        // Assert
        Assert.Equal(SignatureTool.Eraser, state.CurrentTool);
        Assert.Equal(SignatureGridType.Dots, state.GridType);
        Assert.Equal(5.5f, state.StrokeWidth);
        Assert.Equal("#123456", state.PenColor);
        Assert.Equal(0.7f, state.PenOpacity);
        Assert.Equal(SignatureLineStyle.Dashed, state.StrokeStyle);
        Assert.True(state.ShowSeparatorLine);
        Assert.False(state.UseSmooth);
        Assert.True(state.UsePointerPressure);
        Assert.True(state.UseShadow);
        Assert.Equal(0.3f, state.ShadowOpacity);
        Assert.Equal("#654321", state.ShadowColor);
    }

    [Fact]
    public void Update_WithSignatureExportSettings_UpdatesExportFormatAndQuality()
    {
        // Arrange
        var state = new SignatureState();
        var exportSettings = new SignatureExportSettings
        {
            Format = SignatureExportFormat.Png,
            Quality = 80
        };

        // Act
        state.Update(exportSettings);

        // Assert
        Assert.Equal(SignatureExportFormat.Png, state.ExportFormat);
        Assert.Equal(80, state.Quality);
    }

    [Fact]
    public void Reset_CallsSignatureSettingsResetAndUpdatesState()
    {
        // Arrange
        var state = new SignatureState();
        var settings = new SignatureSettings
        {
            Tool = SignatureTool.Eraser,
            GridType = SignatureGridType.Dots,
            StrokeWidth = 5.5f,
            PenColor = "#123456",
            PenOpacity = 0.7f,
            StrokeStyle = SignatureLineStyle.Dashed,
            ShowSeparatorLine = true,
            Smooth = false,
            UsePointerPressure = true,
            UseShadow = true,
            ShadowOpacity = 0.3f,
            ShadowColor = "#654321"
        };

        // Act
        state.Reset(settings);

        // Assert: les valeurs doivent être revenues aux valeurs par défaut de SignatureSettings
        Assert.Equal(SignatureTool.Pen, state.CurrentTool);
        Assert.Equal(SignatureGridType.None, state.GridType);
        Assert.Equal(2.0f, state.StrokeWidth);
        Assert.Equal("#000000", state.PenColor);
        Assert.Equal(1.0f, state.PenOpacity);
        Assert.Equal(SignatureLineStyle.Solid, state.StrokeStyle);
        Assert.False(state.ShowSeparatorLine);
        Assert.True(state.UseSmooth);
        Assert.False(state.UsePointerPressure);
        Assert.False(state.UseShadow);
        Assert.Equal(0.6f, state.ShadowOpacity);
        Assert.Equal("#000000", state.ShadowColor);
    }
}
