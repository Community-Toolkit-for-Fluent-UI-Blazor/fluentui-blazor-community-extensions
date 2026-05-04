namespace FluentUI.Blazor.Community.Components.Charts.Styles;

/// <summary>
/// Represents a utility class responsible for resolving chart visual state styles by combining values from a provided item and default styles.
/// </summary>
internal static class ChartStyleResolver
{
    /// <summary>
    /// Resolves the specified <see cref="ChartVisualStateStyle"/> by applying the values from the provided item and defaults.
    /// </summary>
    /// <param name="item">The <see cref="ChartVisualStateStyle"/> to resolve, which may contain null values.</param>
    /// <param name="defaults">The default <see cref="ChartVisualStateStyle"/> to use for any null values in the item.</param>
    /// <returns>Returns a new <see cref="ChartVisualStateStyle"/> instance with all properties resolved based on the provided item and defaults.</returns>
    internal static ChartVisualStateStyle Resolve(
        ChartVisualStateStyle? item,
        ChartVisualStateStyle? defaults)
    {
        return new ChartVisualStateStyle
        {
            Fill = item?.Fill ?? defaults?.Fill,
            Stroke = item?.Stroke ?? defaults?.Stroke,
            StrokeWidth = item?.StrokeWidth ?? defaults?.StrokeWidth,
            Opacity = item?.Opacity ?? defaults?.Opacity,
            Cursor = item?.Cursor ?? defaults?.Cursor,
            Filter = item?.Filter ?? defaults?.Filter,
            Shadow = item?.Shadow ?? defaults?.Shadow
        };
    }
}
