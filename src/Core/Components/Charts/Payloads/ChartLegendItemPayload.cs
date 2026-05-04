namespace FluentUI.Blazor.Community.Components.Charts.Payloads;

/// <summary>
/// Represents an item in a legend, associating a label with a color index.
/// </summary>
/// <param name="Label">The text label that describes the legend item.</param>
/// <param name="ColorIndex">The index of the color associated with the legend item.</param>
public sealed record LegendItem(string? Label, int ColorIndex);
