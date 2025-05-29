// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using FluentUI.Blazor.Community.Geometry;

namespace FluentUI.Blazor.Community.Components;

public record FluentCxTileGridItemResizeEventArgs(
    TileGridItemResizeHandle Orientation,
    RectF Original,
    PointF MousePosition,
    SizeF NewSize,
    SizeF Parent)
{
}
