using Microsoft.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Defines an interface for observing and managing DOM mutations on specified elements within a group context.
/// </summary>
/// <remarks>Implementations of this interface allow clients to register and unregister mutation observers for
/// elements, typically to react to changes in the DOM such as attribute modifications, child list updates, or subtree
/// changes. This is commonly used in scenarios where dynamic UI updates or synchronization with client-side state are
/// required. Thread safety and observer lifecycle management depend on the specific implementation.</remarks>
public interface IMutationObserver
{
    /// <summary>
    /// Registers a mutation observer for the specified DOM element, enabling asynchronous tracking of changes according
    /// to the provided options.
    /// </summary>
    /// <remarks>If an observer is already registered for the specified element and group, this method may
    /// overwrite the previous registration. This method does not begin observing until registration is
    /// complete.</remarks>
    /// <param name="groupId">The identifier for the observer group. Used to associate the mutation observer with a logical group for
    /// management or disposal.</param>
    /// <param name="elementId">The unique identifier of the DOM element to observe for mutations.</param>
    /// <param name="elementReference">A reference to the DOM element that will be observed for mutations. Must not be null.</param>
    /// <param name="options">The configuration options that specify which types of mutations to observe, such as attribute changes, child
    /// list modifications, or subtree changes. Cannot be null.</param>
    /// <returns>A task that represents the asynchronous registration operation. The task completes when the mutation observer
    /// has been successfully registered.</returns>
    Task RegisterMutationAsync(string groupId, string elementId, ElementReference elementReference, MutationObserverOptions options);

    /// <summary>
    /// Asynchronously unregisters a mutation identified by the specified group and element IDs.
    /// </summary>
    /// <param name="groupId">The unique identifier of the mutation group containing the mutation to unregister. Cannot be null or empty.</param>
    /// <param name="elementId">The unique identifier of the mutation element to unregister within the specified group. Cannot be null or empty.</param>
    /// <returns>A task that represents the asynchronous unregister operation.</returns>
    Task UnregisterMutationAsync(string groupId, string elementId);
}
