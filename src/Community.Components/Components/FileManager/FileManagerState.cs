// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace FluentUI.Blazor.Community.Components;

public sealed class FileManagerState
{
    private FileSortBy _sortBy;
    private FileSortMode _sortMode;
    private FileView _view;

    public event EventHandler? OnSortUpdated;
    public event EventHandler? OnViewUpdated;

    public FileSortBy SortBy
    {
        get => _sortBy;

        internal set
        {
            if (_sortBy != value)
            {
                _sortBy = value;
                OnSortUpdated?.Invoke(this, EventArgs.Empty);
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
                OnSortUpdated?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public FileView View
    {
        get => _view;
        internal set
        {
            if (_view != value)
            {
                _view = value;
                OnViewUpdated?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}

