using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents an interface for the <see cref="InternalTrailMenuItem"/>
/// </summary>
public interface ITrailMenuItem
{
    /// <summary>
    /// Gets the label of the item.
    /// </summary>
    string? Label { get; set; }

    /// <summary>
    /// Gets or sets the parent of the item.
    /// </summary>
    ITrailMenuItem? Parent { get; set;  }

    /// <summary>
    /// Gets or sets the next item in the trail.
    /// </summary>
    ITrailMenuItem? Next { get; set; }

    /// <summary>
    /// Gets the id of the item.
    /// </summary>
    string? Id { get; }

    /// <summary>
    /// Gets the <see cref="Microsoft.FluentUI.AspNetCore.Components.Icon"/> of the item.
    /// </summary>
    Icon? Icon { get; }

    /// <summary>
    /// Gets the menu items of the item.
    /// </summary>
    IEnumerable<ITrailMenuItem> Items { get; set; }
}
