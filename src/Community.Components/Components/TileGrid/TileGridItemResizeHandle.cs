// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System.ComponentModel;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the handle for resizing <see cref="FluentCxTileGridItem{TItem}"/>
/// </summary>
public enum TileGridItemResizeHandle
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
