// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace FluentUI.Blazor.Community.Components;

public sealed class FileManagerSortState
{
    private FileSortBy _sortBy;
    private FileSortMode _sortMode;

    public event EventHandler? OnUpdated;

    public FileSortBy SortBy
    {
        get => _sortBy;

        internal set
        {
            if (_sortBy != value)
            {
                _sortBy = value;
                OnUpdated?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public FileSortMode SortMode
    {
        get => _sortMode;
        internal set
        {
            if (value != _sortMode)
            {
                _sortMode = value;
                OnUpdated?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}

