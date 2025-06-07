// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System.ComponentModel;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the handler for the resizer component.
/// </summary>
public enum ResizerHandler
{
    /// <summary>
    /// Resizes the item horizontally.
    /// </summary>
    [Description("ew")]
    Horizontally,

    /// <summary>
    /// Resizes the item vertically.
    /// </summary>
    [Description("ns")]
    Vertically,

    /// <summary>
    /// Resizes the item vertically and horizontally.
    /// </summary>
    [Description("nwse")]
    Both
}
