namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents configuration options for customizing the appearance and behavior of a signature canvas.
/// </summary>
/// <remarks>This record provides a comprehensive set of options to control the visual style, dimensions, and 
/// interactive behavior of a signature canvas. It includes settings for stroke appearance, grid lines,  background,
/// watermark, and more. Default values are provided for all properties, allowing for  easy customization of only the
/// desired options.</remarks>
/// <param name="StrokeWidth">Represents the width of the stroke.</param>
/// <param name="PenColor">Represents the color of the pen.</param>
/// <param name="PenOpacity">Represents the opacity of the pen.</param>
/// <param name="ShadowColor">Represents the color of the shadow.</param>
/// <param name="ShadowOpacity">Represents the opacity of the shadow.</param>
/// <param name="StrokeStyle">Represents the style of the stroke.</param>
/// <param name="Smooth">Represents if the stroke are smooth.</param>
/// <param name="UseShadow">Value indicating if the shadow is used.</param>
/// <param name="UsePointerPressure">Value indicating if we use the pressure of the pointer.</param>
/// <param name="Width">Width of the signature zone.</param>
/// <param name="Height">Height of the signature zone.</param>
/// <param name="Background">Background color of the signature zone.</param>
/// <param name="ShowSeparatorLine">Value indicating if the separator line is visible.</param>
/// <param name="SeparatorY">Represents the height from the top where the signature line is drawn.</param>
/// <param name="SeparatorLineColor">Represents the color of the separator line.</param>
/// <param name="ShowGrid">Value indicating if the grid is visible.</param>
/// <param name="GridColor">Represents the color of the grid lines.</param>
/// <param name="GridSpacing">Represents the spacing between the grid lines.</param>
/// <param name="GridType">Represents the type of the grid.</param>
/// <param name="GridOpacity">Represents the opactiry of the grid.</param>
/// <param name="WatermarkText">Represents the text of the watermark.</param>
/// <param name="WatermarkOpacity">Represents the opacity of the watermark.</param>
internal record SignatureOptions(
    float StrokeWidth = 2.0f,
    string PenColor = "#000000",
    float PenOpacity = 1.0f,
    string ShadowColor = "#000000",
    float ShadowOpacity = 0.3f,
    SignatureLineStyle StrokeStyle = SignatureLineStyle.Solid,
    bool Smooth = true,
    bool UseShadow = false,
    bool UsePointerPressure = false,
    int Width = 300,
    int Height = 150,
    string Background = "#FFFFFF",
    bool ShowSeparatorLine = true,
    float SeparatorY = 1.0f,
    string SeparatorLineColor = "#0058E9",
    bool ShowGrid = false,
    string GridColor = "#e0e0e0",
    float GridSpacing = 20.0f,
    SignatureGridType GridType = SignatureGridType.Lines,
    float GridOpacity = 0.5f,
    string? WatermarkText = null,
    float WatermarkOpacity = 0.1f)
{
}
