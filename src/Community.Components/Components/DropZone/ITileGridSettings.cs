// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace FluentUI.Blazor.Community.Components;

public interface ITileGridSettings
    : IGridSettings
{
    string ColumnWidth { get; }

    string? MinimumColumnWidth { get; }

    int? Columns { get; }

    string RowHeight { get; }
}
