namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides methods to apply a signature theme definition to signature rendering options.
/// </summary>
/// <remarks>This static class centralizes the logic for transferring theme settings to rendering options,
/// ensuring consistent application of visual styles such as background, grid, axes, and watermark across signature
/// components. It is intended for use when updating or initializing signature rendering based on a specified
/// theme.</remarks>
public static class SignatureThemeApplier
{
    /// <summary>
    /// Applies the specified rendering options to the given signature theme definition.
    /// </summary>
    /// <param name="theme">The signature theme definition to which the rendering options will be applied. Cannot be null.</param>
    /// <param name="options">The rendering options that define how the signature theme should be configured. Cannot be null.</param>
    public static void Apply(SignatureThemeDefinition theme, SignatureRenderingOptions options)
    {
        ApplyBackground(theme, options.Background);
        ApplyGrid(theme, options.Grid);
        ApplyAxes(theme, options.Axes);
        ApplyWatermark(theme, options.Watermark);
    }

    /// <summary>
    /// Applies the background color and opacity from the specified theme to the provided background options.
    /// </summary>
    /// <param name="theme">The theme definition containing the background color and opacity to apply.</param>
    /// <param name="options">The background options to which the theme's background settings will be applied. This object is modified
    /// by the method.</param>
    private static void ApplyBackground(SignatureThemeDefinition theme, SignatureBackgroundOptions options)
    {
        options.Color = theme.Background.Color;
        options.Opacity = theme.Background.Opacity;
    }

    /// <summary>
    /// Applies the grid settings from the specified theme to the provided grid options.
    /// </summary>
    /// <param name="theme">The theme definition containing the grid settings to apply. Cannot be null.</param>
    /// <param name="options">The grid options to which the theme's grid settings will be applied. Cannot be null.</param>
    private static void ApplyGrid(SignatureThemeDefinition theme, SignatureGridOptions options)
    {
        var t = theme.Grid;
        options.DisplayMode = t.DisplayMode;
        options.Color = t.Color;
        options.Opacity = t.Opacity;
        options.CellSize = t.CellSize;
        options.BoldEvery = t.BoldEvery;
        options.StrokeWidth = t.StrokeWidth;
        options.DashArray = t.DashArray;
        options.PointRadius = t.PointRadius;
    }

    /// <summary>
    /// Applies axis-related visual properties from the specified theme to the provided axes options.
    /// </summary>
    /// <param name="theme">The theme definition containing axis visual properties to apply. Cannot be null.</param>
    /// <param name="options">The axes options to which the theme's axis properties will be applied. Cannot be null.</param>
    private static void ApplyAxes(SignatureThemeDefinition theme, SignatureAxesOptions options)
    {
        var t = theme.Axes;
        options.Color = t.Color;
        options.Opacity = t.Opacity;
        options.StrokeWidth = t.StrokeWidth;
        options.DashArray = t.DashArray;
    }

    /// <summary>
    /// Applies the watermark settings from the specified theme to the provided watermark options.
    /// </summary>
    /// <remarks>This method updates the properties of the <paramref name="opt"/> parameter to match the
    /// watermark configuration defined in <paramref name="theme"/>. Existing values in <paramref name="opt"/> will be
    /// overwritten.</remarks>
    /// <param name="theme">The signature theme definition containing the watermark settings to apply. Cannot be null.</param>
    /// <param name="opt">The watermark options to which the theme's watermark settings will be applied. Cannot be null.</param>
    private static void ApplyWatermark(SignatureThemeDefinition theme, SignatureWatermarkOptions opt)
    {
        var t = theme.Watermark;
        opt.Color = t.Color;
        opt.Opacity = t.Opacity;
        opt.FontSize = t.FontSize;
        opt.FontFamily = t.FontFamily;
        opt.FontWeight = t.FontWeight;
    }
}

