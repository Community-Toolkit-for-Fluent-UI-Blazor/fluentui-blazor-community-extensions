using Microsoft.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Defines methods for observing and managing resize events for DOM elements.
/// </summary>
/// <remarks>This interface provides functionality to register and unregister DOM elements for resize observation.
/// It supports grouping elements by an optional group identifier, allowing for more granular control over resize event
/// handling. Use this interface to monitor size changes of specific elements in a web application.</remarks>
public interface IResizeObserver
{
    /// <summary>
    /// Registers an element for resize event tracking within a specified group.
    /// </summary>
    /// <remarks>This method enables resize event tracking for the specified element, allowing the application
    /// to respond to changes in the element's size. Ensure that the <paramref name="groupId"/> and <paramref
    /// name="elementId"/> are unique within the context of the application.</remarks>
    /// <param name="groupId">The identifier of the group to which the element belongs. This value cannot be null or empty.</param>
    /// <param name="elementId">The unique identifier of the element to be tracked. This value cannot be null or empty.</param>
    /// <param name="elementReference">A reference to the DOM element to be monitored for resize events.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task RegisterResizeAsync(string groupId, string elementId, ElementReference elementReference);

    /// <summary>
    /// Unregisters a resize listener for a specific element within a group asynchronously.
    /// </summary>
    /// <param name="groupId">The identifier of the group containing the element. Cannot be <see langword="null"/> or empty.</param>
    /// <param name="elementId">The identifier of the element to unregister. Cannot be <see langword="null"/> or empty.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task UnregisterResizeAsync(string groupId, string elementId);
}
