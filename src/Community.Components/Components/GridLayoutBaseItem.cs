using System.Text.Json.Serialization;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents an item of a <see cref="GridLayoutBase"/>.
/// </summary>
[JsonDerivedType(typeof(TileGridLayoutItem))]
public class GridLayoutBaseItem
{
    /// <summary>
    /// Gets or sets the index of the item.
    /// </summary>
    public int Index {  get; set; }

    /// <summary>
    /// Gets or sets the key of the item.
    /// </summary>
    public string? Key {  get; set; }
}
