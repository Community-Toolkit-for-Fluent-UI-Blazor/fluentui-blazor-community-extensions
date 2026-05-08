namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the result of a layout calculation for a sleek dial, including item layouts and popup dimensions.
/// </summary>
/// <remarks>This class is typically used to encapsulate the layout information required to render a sleek dial
/// component, such as the positions of individual items and the overall size of the popup container.</remarks>
internal class SleekDialLayoutResult
{
    /// <summary>
    /// Gets or sets the collection of layouts used to define the arrangement of dial items.
    /// </summary>
    public List<SleekDialItemLayout> Layouts { get; init; } = [];

    /// <summary>
    /// Gets the width of the popup, in pixels.
    /// </summary>
    public int PopupWidth { get; init; }

    /// <summary>
    /// Gets the height of the popup, in pixels.
    /// </summary>
    public int PopupHeight { get; init; }
}

