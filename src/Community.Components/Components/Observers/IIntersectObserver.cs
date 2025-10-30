using Microsoft.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Defines methods for registering and unregistering elements for intersection observation.
/// </summary>
/// <remarks>This interface is typically used to monitor the visibility or intersection of elements within a
/// viewport or container. It supports grouping elements for more granular control over intersection
/// observation.</remarks>
public interface IIntersectObserver
{
    /// <summary>
    /// Registers an intersection observer for the specified element within a group.
    /// </summary>
    /// <param name="groupId">The unique identifier of the group to which the element belongs.</param>
    /// <param name="elementId">The unique identifier of the element to observe.</param>
    /// <param name="elementReference">A reference to the DOM element to be observed.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task RegisterIntersectAsync(string groupId, string elementId, ElementReference elementReference);

    /// <summary>
    /// Asynchronously unregisters an intersecting element from a specified group.
    /// </summary>
    /// <remarks>This method removes the specified element from the group, ensuring it is no longer considered
    /// part of the intersection. If the element or group does not exist, the method completes without making
    /// changes.</remarks>
    /// <param name="groupId">The unique identifier of the group from which the element will be unregistered. Cannot be null or empty.</param>
    /// <param name="elementId">The unique identifier of the element to be unregistered. Cannot be null or empty.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task UnregisterIntersectAsync(string groupId, string elementId);
}
