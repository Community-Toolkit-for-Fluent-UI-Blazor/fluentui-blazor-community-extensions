// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace FluentUI.Blazor.Community.Components;

public interface IGridSettings
{
    DropZoneDisplay Display { get; }

    string? Width { get; }

    string? Height { get; }
}
