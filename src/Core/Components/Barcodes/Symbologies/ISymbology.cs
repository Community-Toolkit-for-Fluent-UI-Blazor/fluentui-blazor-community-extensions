using FluentUI.Blazor.Community.Components.Enums;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Defines a contract for types that provide symbology information.
/// </summary>
/// <remarks>Implement this interface to expose a specific symbology type through the Symbology property. This is
/// typically used in scenarios where different symbology standards or formats need to be represented in a consistent
/// manner.</remarks>
internal interface ISymbology
{
    /// <summary>
    /// Gets the symbology used by the component.
    /// </summary>
    Symbology Symbology { get; }
}
