using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a component that displays subtitles with customizable background styles and colors.
/// </summary>
public partial class SubtitleOverlay
    : FluentComponentBase
{
    /// <summary>
    /// Gets or sets the background style applied to the subtitle.
    /// </summary>
    [Parameter]
    public SubtitleBackground Background { get; set; }

    /// <summary>
    /// Gets or sets color of the background.
    /// </summary>
    [Parameter]
    public string? BackgroundColor { get; set; }

    /// <summary>
    /// Gets or sets the subtitle entry to display.
    /// </summary>
    [Parameter]
    public SubtitleEntry? Entry { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the subtitle overlay is visible.
    /// </summary>
    [Parameter]
    public bool IsVisible { get; set; }

    /// <summary>
    /// Gets the computed CSS style string based on the current background and color settings.
    /// </summary>
    private string? InternalStyle => new StyleBuilder(Style)
        .AddStyle("background-color", Convert(BackgroundColor, Background))
        .ToString();

    /// <summary>
    /// Converts the specified background color and subtitle background style to an HTML color string with the
    /// appropriate alpha transparency applied.
    /// </summary>
    /// <param name="backgroundColor">A string containing the HTML color code to use as the base background color. Can be null or empty, in which case
    /// a default color is used.</param>
    /// <param name="background">A value indicating the subtitle background style, which determines the alpha transparency to apply to the color.</param>
    /// <returns>An HTML color string representing the background color with the alpha transparency applied. Returns null if the
    /// conversion fails.</returns>
    private static string? Convert(string? backgroundColor, SubtitleBackground background)
    {
        var color = ColorTranslator.FromHtml(backgroundColor ?? string.Empty);
        var alpha = GetAlpha(background);

        return $"#{color.R:X2}{color.G:X2}{color.B:X2}{alpha:X2}";
    }

    /// <summary>
    /// Gets the alpha (opacity) value, in the range 0–255, corresponding to the specified subtitle background style.
    /// </summary>
    /// <param name="background">The subtitle background style for which to retrieve the alpha value.</param>
    /// <returns>An integer representing the alpha value for the specified background style. 255 indicates fully opaque; 0
    /// indicates fully transparent.</returns>
    /// <exception cref="NotImplementedException">Thrown if the specified background style is not recognized.</exception>
    private static int GetAlpha(SubtitleBackground background)
    {
        return background switch
        {
            SubtitleBackground.Solid => 255,
            SubtitleBackground.Opaque => 217,
            SubtitleBackground.SemiTransparent => 128,
            SubtitleBackground.Transparent => 0,
            _ => throw new NotImplementedException()
        };
    }

    /// <summary>
    /// Applies the specified subtitle options to update the background and background color settings.
    /// </summary>
    /// <param name="subtitleOptions">The subtitle options to apply. Cannot be null.</param>
    internal void SetOptions([NotNull]SubtitleOptions subtitleOptions)
    {
        Background = subtitleOptions.Background;
        BackgroundColor = subtitleOptions.BackgroundColor;
        StateHasChanged();
    }
}
