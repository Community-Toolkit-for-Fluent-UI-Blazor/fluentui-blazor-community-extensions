using Microsoft.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents an interface for the <see cref="Internal.FluentCxDropZone{TItem}"/> component.
/// </summary>
/// <typeparam name="TItem"></typeparam>
public interface IDropZoneComponent<TItem>
    : IItemValue<TItem>
{
    /// <summary>
    /// Gets the identifier of the drop zone.
    /// </summary>
    string? Id { get; }

    /// <summary>
    /// Gets the component to render inside the drop zone.
    /// </summary>
    RenderFragment? Component { get; }
}
