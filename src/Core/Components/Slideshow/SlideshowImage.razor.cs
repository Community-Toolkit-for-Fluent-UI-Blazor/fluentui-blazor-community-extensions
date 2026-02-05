using FluentUI.Blazor.Community.Components.States;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the image inside a <see cref="FluentCxSlideshow{TItem}"/>.
/// </summary>
/// <typeparam name="TItem">Type of the item.</typeparam>
public partial class SlideshowImage<TItem>
    : FluentComponentBase
{
    /// <summary>
    /// Holds a reference to the JavaScript module that manages slideshow image functionality for the specified item
    /// type.
    /// </summary>
    /// <remarks>This field is nullable, indicating that the JavaScript module may not be initialized. It is
    /// intended for internal use to facilitate interaction between the .NET component and its associated JavaScript
    /// logic.</remarks>
    private SlideshowImageJS<TItem>? _jsModule;

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="SlideshowImage{TItem}"/> class.
    /// </summary>
    public SlideshowImage(LibraryConfiguration configuration)
        : base(configuration)
    {
        Id = Identifier.NewId();
    }

    #endregion Constructors

    #region Properties

    /// <summary>
    /// Gets or sets the source of the image.
    /// </summary>
    [Parameter]
    public string? Source { get; set; }

    /// <summary>
    /// Gets or sets the alternate text of the image.
    /// </summary>
    [Parameter]
    public string? Alt { get; set; }

    /// <summary>
    /// Gets or sets the title of the image.
    /// </summary>
    [Parameter]
    public string? Title { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the image should be loaded lazily.
    /// </summary>
    [Parameter]
    public bool IsLazy { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether the image has high fetch priority.
    /// </summary>
    [Parameter]
    public bool FetchPriorityHigh { get; set; } = false;

    /// <summary>
    /// Gets or sets the current state of the slideshow image.
    /// </summary>
    [Inject]
    private SlideshowState State { get; set; } = default!;

    /// <summary>
    /// Gets or sets the parent slideshow item in the cascading parameter hierarchy.
    /// </summary>
    [CascadingParameter]
    private SlideshowItem<TItem>? Parent { get; set; }

    #endregion Properties

    #region Methods

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        base.OnInitialized();

        Parent?.Add(this);
    }

    /// <inheritdoc />
    public override async ValueTask DisposeAsync()
    {
        State?.RemoveSize(Id);

        if (_jsModule != null)
        {
            await _jsModule.DisposeAsync(Id);
        }

        Parent?.Remove(this);

        await base.DisposeAsync();
    }

    /// <inheritdoc />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (firstRender)
        {
            _jsModule = new SlideshowImageJS<TItem>(JSModule, State);
            await _jsModule.Initialize(Id);
        }
    }

    #endregion Methods
}
