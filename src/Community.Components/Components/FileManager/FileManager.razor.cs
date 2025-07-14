using System.Threading.Tasks;
using FluentUI.Blazor.Community.Extensions;
using Microsoft.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the file manager.
/// </summary>
/// <typeparam name="TItem">Type of the item.</typeparam>
public partial class FileManager<TItem>
    : IDisposable where TItem : class, new()
{
    /// <summary>
    /// Represents a value indicating if the file manager is busy.
    /// </summary>
    private bool _isBusy;

    /// <summary>
    /// Gets or sets a value indicating if the file navigation view is visible or not.
    /// </summary>
    [Parameter]
    public bool FileNavigationViewVisible { get; set; }

    /// <summary>
    /// Gets or sets a value indicating if the progess is indeterminate or not.
    /// </summary>
    [Parameter]
    public bool IsIndeterminateProgress { get; set; }

    /// <summary>
    /// Gets or sets the percentage of the progression.
    /// </summary>
    [Parameter]
    public int? ProgressPercent { get; set; }

    /// <summary>
    /// Gets or sets the label of the progress.
    /// </summary>
    [Parameter]
    public string? ProgressLabel { get; set; }

    /// <summary>
    /// Gets or sets a value indicating if the file manager runs on mobile or not.
    /// </summary>
    [Parameter]
    public bool IsMobile { get; set; }

    /// <summary>
    /// Gets or sets the template of the item.
    /// </summary>
    [Parameter]
    public RenderFragment<FileManagerEntry<TItem>>? ItemTemplate { get; set; }

    /// <summary>
    /// Gets or sets the selected items.
    /// </summary>
    [Parameter]
    public IEnumerable<FileManagerEntry<TItem>> SelectedItems { get; set; } = [];

    /// <summary>
    /// Gets or sets the event callback when an item is double clicked.
    /// </summary>
    [Parameter]
    public EventCallback<FileManagerEntryEventArgs<TItem>> OnItemDoubleClick { get; set; }

    /// <summary>
    /// Gets or sets the labels of the columns when the file manager renders the items as list or details.
    /// </summary>
    [Parameter]
    public FileListDataGridColumnLabels ColumnLabels { get; set; } = FileListDataGridColumnLabels.Default;

    /// <summary>
    /// Gets or sets the event callback when the selecteditems has changed.
    /// </summary>
    [Parameter]
    public EventCallback<IEnumerable<FileManagerEntry<TItem>>> SelectedItemsChanged { get; set; }

    /// <summary>
    /// Gets or sets the entry.
    /// </summary>
    [Parameter]
    public FileManagerEntry<TItem>? Entry { get; set; }

    /// <summary>
    /// Gets or sets the event callback when an item is downloaded.
    /// </summary>
    [Parameter]
    public EventCallback<FileManagerEntry<TItem>> Download { get; set; }

    /// <summary>
    /// Gets or sets the items of the <see cref="FluentCxPathBar" />.
    /// </summary>
    [Parameter]
    public IPathBarItem? PathRoot { get; set; }

    /// <summary>
    /// Gets or sets the path of the navigation.
    /// </summary>
    [Parameter]
    public string? Path { get; set; }

    /// <summary>
    /// Gets or sets the callback when a path changed.
    /// </summary>
    [Parameter]
    public EventCallback<string> OnPathChanged { get; set; }

    /// <summary>
    /// Gets or sets the state of the file manager.
    /// </summary>
    [Inject]
    public required FileManagerState State { get; set; }

    /// <summary>
    /// Gets or sets the maximum visible items in the navigation bar
    /// </summary>
    [Parameter]
    public int? MaxVisibleItems { get; set; }

    /// <summary>
    /// Occurs when the selected items has changed.
    /// </summary>
    /// <returns>Returns a task which contains the way to process the selecteditems propery when completed.</returns>
    private async Task OnSelectedItemsChangedAsync()
    {
        if (SelectedItemsChanged.HasDelegate)
        {
            await SelectedItemsChanged.InvokeAsync(SelectedItems);
        }
    }

    /// <summary>
    /// Occurs when the view is updated.
    /// </summary>
    /// <param name="sender">Object which invokes the method.</param>
    /// <param name="e">Events args associated to this method.</param>
    private void OnViewUpdated(object? sender, EventArgs e)
    {
        StateHasChanged();
    }

    /// <summary>
    /// Occurs when the state of the file manager is updated.
    /// </summary>
    /// <param name="sender">Object which invokes the method.</param>
    /// <param name="e">Events args associated to this method.</param>
    private void OnUpdated(object? sender, EventArgs e)
    {
        Sort();
    }

    /// <summary>
    /// Sets the entry to render in the file manager.
    /// </summary>
    /// <param name="entry">Entry to set.</param>
    private void SetEntry(FileManagerEntry<TItem> entry)
    {
        Entry = entry;
        Entry.InvalidateMerge();
        Sort();
    }

    /// <summary>
    /// Occurs when an item is double clicked.
    /// </summary>
    /// <param name="e">EventArgs which contains the item which was double clicked.</param>
    /// <returns>Returns a task which contains the way to process the double clicked item when completed.</returns>
    private async Task OnItemDoubleTappedAsync(FileManagerEntryEventArgs<TItem> e)
    {
        SelectedItems = [];

        if (e.Entry.IsDirectory)
        {
            SetEntry(e.Entry);

            if (OnItemDoubleClick.HasDelegate)
            {
                await OnItemDoubleClick.InvokeAsync(e);
            }
        }
        else if (Download.HasDelegate)
        {
            await Download.InvokeAsync(e.Entry);
        }
    }

    /// <summary>
    /// Sort the current entry.
    /// </summary>
    private void Sort()
    {
        Entry?.Sort(State.SortMode, State.SortBy);
    }

    /// <inheritdoc />
    public override async Task SetParametersAsync(ParameterView parameters)
    {
        await base.SetParametersAsync(parameters);

        if (parameters.HasValueChanged(nameof(Entry), Entry) &&
            Entry is not null)
        {
            Entry.InvalidateMerge();
            Sort();
        }
    }

    /// <summary>
    /// Sets if the file manager is busy or not.
    /// </summary>
    /// <param name="isBusy">Value indicating if the file manager is busy or not.</param>
    internal void SetBusy(bool isBusy)
    {
        _isBusy = isBusy;
    }

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        base.OnInitialized();

        State.OnSortUpdated += OnUpdated;
        State.OnViewUpdated += OnViewUpdated;
    }

    /// <inheritdoc />
    public void Dispose()
    {
        State.OnSortUpdated -= OnUpdated;
        State.OnViewUpdated -= OnViewUpdated;

        GC.SuppressFinalize(this);
    }
}

