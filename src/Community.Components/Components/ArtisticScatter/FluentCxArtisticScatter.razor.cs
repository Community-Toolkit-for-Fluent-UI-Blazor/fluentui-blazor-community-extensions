using FluentUI.Blazor.Community.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;
using Microsoft.JSInterop;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a container for a collection of images to be displayed in an artistic scatter layout.
/// </summary>
public partial class FluentCxArtisticScatter
    : FluentComponentBase, IAsyncDisposable
{
    #region Fields

    /// <summary>
    /// Represents the list of items in the scatter.
    /// </summary>
    private readonly List<ArtisticScatterItem> _items = [];

    /// <summary>
    /// Indicates whether the maximum item value has changed.
    /// </summary>
    /// <remarks>This field is used internally to track changes to the maximum item value.</remarks>
    private bool _hasMaxItemChanged;

    /// <summary>
    /// Reference to the JavaScript module for interop.
    /// </summary>
    private IJSObjectReference? _module;

    #endregion Fields

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="FluentCxArtisticScatter"/> class.
    /// </summary>
    /// <remarks>This constructor generates a new unique identifier for the instance using <see
    /// cref="Identifier.NewId"/>.</remarks>
    public FluentCxArtisticScatter()
    {
        Id = Identifier.NewId();
    }

    #endregion Constructors

    #region Properties

    /// <summary>
    /// Gets or sets the JavaScript runtime for interop.
    /// </summary>
    [Inject]
    private IJSRuntime Runtime { get; set; } = default!;

    /// <summary>
    /// Gets or sets the unique identifier for the component instance.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets the maximum number of items to display in the scatter.
    /// </summary>
    [Parameter]
    public int MaxItems { get; set; } = 5;

    /// <summary>
    /// Gets or sets a value indicating whether the component is being viewed on a mobile device.
    /// </summary>
    [Parameter]
    public bool IsMobile { get; set; }

    /// <summary>
    /// Gets or sets the callback to invoke when the scatter is clicked.
    /// </summary>
    [Parameter]
    public EventCallback OnClick { get; set; }

    /// <summary>
    /// Gets or sets the width of the scatter container.
    /// </summary>
    [Parameter]
    public string? Width { get; set; }

    /// <summary>
    /// Gets or sets the height of the scatter container.
    /// </summary>
    [Parameter]
    public string? Height { get; set; }

    /// <summary>
    /// Gets the items currently displayed in the scatter, limited to the maximum number of items specified by <see cref="MaxItems"/>.
    /// </summary>
    private IEnumerable<ArtisticScatterItem> DisplayedItems => _items.Take(MaxItems);

    /// <summary>
    /// Gets the count of items currently displayed in the scatter.
    /// </summary>
    internal int DisplayedItemCount => DisplayedItems.Count();

    /// <summary>
    /// Gets the internal CSS classes for the component.
    /// </summary>
    private string? InternalCss => new CssBuilder(Class)
        .AddClass("fluent-cx-artistic-scatter")
        .Build();

    /// <summary>
    /// Gets the internal styles for the component.
    /// </summary>
    private string? InternalStyle => new StyleBuilder(Style)
        .AddStyle("--artistic-scatter-width", Width, !string.IsNullOrWhiteSpace(Width))
        .AddStyle("--artistic-scatter-height", Height, !string.IsNullOrWhiteSpace(Height))
        .Build();

    /// <summary>
    /// Gets the internal CSS classes for the scatter container.
    /// </summary>
    private string? InternalScatterContainer => new CssBuilder()
        .AddClass("fluent-cx-artistic-scatter-container")
        .AddClass("mobile", IsMobile)
        .AddClass("default", !IsMobile)
        .Build();

    #endregion Properties

    #region Methods

    /// <summary>
    /// Asynchronously invokes the <see cref="OnClick"/> event if it has been assigned a delegate.
    /// </summary>
    /// <remarks>This method checks whether the <see cref="OnClick"/> event has a delegate before invoking it.
    /// If no delegate is assigned, the method does nothing.</remarks>
    /// <returns></returns>
    private async Task OnClickAsync()
    {
        if (OnClick.HasDelegate)
        {
            await OnClick.InvokeAsync();
        }
    }

    /// <summary>
    /// Determines the zero-based index of the specified <see cref="ArtisticScatterItem"/> in the collection.
    /// </summary>
    /// <param name="item">The <see cref="ArtisticScatterItem"/> to locate in the collection.</param>
    /// <returns>The zero-based index of the first occurrence of the specified item in the collection,  or -1 if the item is not
    /// found.</returns>
    internal int IndexOf(ArtisticScatterItem item) => _items.IndexOf(item);

    /// <summary>
    /// Adds the specified item to the collection if it is not already present.
    /// </summary>
    /// <param name="item">The <see cref="ArtisticScatterItem"/> to add to the collection. Cannot be <see langword="null"/>.</param>
    internal void Add(ArtisticScatterItem item)
    {
        ArgumentNullException.ThrowIfNull(item, nameof(item));

        if (!_items.Contains(item))
        {
            _items.Add(item);
            InvokeAsync(StateHasChanged);
        }
    }

    /// <summary>
    /// Removes the specified item from the collection.
    /// </summary>
    /// <param name="item">The item to remove from the collection. Cannot be <see langword="null"/>.</param>
    internal void Remove(ArtisticScatterItem item)
    {
        ArgumentNullException.ThrowIfNull(item, nameof(item));

        if (_items.Remove(item))
        {
            InvokeAsync(StateHasChanged);
        }
    }

    /// <inheritdoc />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (firstRender)
        {
            _module = await Runtime.InvokeAsync<IJSObjectReference>("import", "./_content/FluentUI.Blazor.Community.Components/components/ArtisticScatter/FluentCxArtisticScatter.razor.js");
            await _module.InvokeVoidAsync("fluentCxArtisticScatter.initialize", Id, Element);
            await _module.InvokeVoidAsync("fluentCxArtisticScatter.updateScatterImages", Id, DisplayedItems.Select(x => x.Id).ToList());
        }

        if (_hasMaxItemChanged)
        {
            _hasMaxItemChanged = false;

            if (_module is not null)
            {
                await _module.InvokeVoidAsync("fluentCxArtisticScatter.updateScatterImages", Id, DisplayedItems.Select(x => x.Id).ToList());
            }
        }
    }

    /// <inheritdoc />
    public override Task SetParametersAsync(ParameterView parameters)
    {
        _hasMaxItemChanged = parameters.HasValueChanged(nameof(MaxItems), MaxItems);

        return base.SetParametersAsync(parameters);
    }

    /// <inheritdoc />
    public async ValueTask DisposeAsync()
    {
        try
        {
            if (_module is not null)
            {
                await _module.InvokeVoidAsync("fluentCxArtisticScatter.dispose", Id);
                await _module.DisposeAsync();
                _module = null;
            }
        }
        catch (JSDisconnectedException) { }

        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Marks the component for re-rendering, causing the UI to be updated during the next render cycle.
    /// </summary>
    internal void InvalidateRender()
    {
        StateHasChanged();
    }

    #endregion Methods
}
