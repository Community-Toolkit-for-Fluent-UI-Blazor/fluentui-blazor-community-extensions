using Microsoft.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Defines the contract for a signature tool used within a component, providing access to its type, button
/// representation, and configuration options.
/// </summary>
/// <remarks>Implementations of this interface represent different types of signature tools that can be integrated
/// into a UI. The interface exposes properties for rendering the tool's button and configuration options, allowing for
/// both default and custom option sets. This enables flexible integration and customization of signature tools in user
/// interfaces.</remarks>
public interface ISignatureTool
{
    /// <summary>
    /// Gets a value indicating whether the current instance is active.
    /// </summary>
    bool IsActive { get; }

    /// <summary>
    /// Gets a custom render fragment that defines additional options to display.
    /// </summary>
    RenderFragment? CustomOptions { get; }

    /// <summary>
    /// Gets the default set of options to render within the component.
    /// </summary>
    RenderFragment DefaultOptions { get; }

    /// <summary>
    /// Activates the current instance, enabling it to perform its intended operations.
    /// </summary>
    void Activate();
}
