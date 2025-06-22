using FluentUI.Blazor.Community.Extensions;
using Microsoft.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

public partial class FileManager<TItem>
    : IDisposable where TItem : class, new()
{
    private bool _isBusy;

    [Parameter]
    public bool FileNavigationViewVisible { get; set; }

    [Parameter]
    public bool IsIndeterminateProgress { get; set; }

    [Parameter]
    public int? ProgressPercent { get; set; }

    [Parameter]
    public string? ProgressLabel { get; set; }

    [Parameter]
    public bool IsMobile { get; set; }

    [Parameter]
    public RenderFragment<FileManagerEntry<TItem>>? ItemTemplate { get; set; }

    [Parameter]
    public IEnumerable<FileManagerEntry<TItem>> SelectedItems { get; set; } = [];

    [Parameter]
    public EventCallback<FileManagerEntryEventArgs<TItem>> OnItemDoubleClick { get; set; }

    [Parameter]
    public FileListDataGridColumnLabels ColumnLabels { get; set; } = new();

    [Parameter]
    public EventCallback<IEnumerable<FileManagerEntry<TItem>>> SelectedItemsChanged { get; set; }

    [Parameter]
    public FileManagerEntry<TItem>? Entry { get; set; }

    [Parameter]
    public EventCallback<FileManagerEntry<TItem>> Download { get; set; }

    [Parameter]
    public List<FileNavigationItem> NavigationItems { get; set; } = [];

    [Inject]
    public required FileManagerState State { get; set; }

    private async Task OnSelectedItemsChangedAsync()
    {
        if (SelectedItemsChanged.HasDelegate)
        {
            await SelectedItemsChanged.InvokeAsync(SelectedItems);
        }
    }

    private void OnViewUpdated(object? sender, EventArgs e)
    {
        StateHasChanged();
    }

    private void OnUpdated(object? sender, EventArgs e)
    {
        Sort();
    }

    private void SetEntry(FileManagerEntry<TItem> entry)
    {
        Entry = entry;
        Entry.InvalidateMerge();
        Sort();
    }

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

    private void Sort()
    {
        Entry?.Sort(FileManagerEntryComparer<TItem>.Default.WithSortMode(State.SortMode).WithSortBy(State.SortBy));
    }

    private static void OnClick(FileNavigationItem item)
    {
        if (item.OnClick is not null)
        {
            item.OnClick(item.Path!);
        }
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

