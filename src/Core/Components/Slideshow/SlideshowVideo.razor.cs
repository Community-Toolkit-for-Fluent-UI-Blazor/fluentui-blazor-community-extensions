using FluentUI.Blazor.Community.Components.States;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the video inside a <see cref="FluentCxSlideshow{TItem}"/>.
/// </summary>
public partial class SlideshowVideo<TItem> : FluentComponentBase
{
    private SlideshowVideoJS<TItem>? _jsModule;

    /// <summary>
    /// Initializes a new instance of the SlideshowVideo class using the specified library configuration.
    /// </summary>
    /// <param name="configuration">The configuration settings that determine how the slideshow video is managed and displayed. Cannot be null.</param>
    public SlideshowVideo(LibraryConfiguration configuration)
        : base(configuration)
    {
        Id = Identifier.NewId();
    }

    /// <summary>
    /// Gets or sets the source of the data used by the component.
    /// </summary>
    /// <remarks>This property can be set to a string representing the data source, which may be a URL or a
    /// local file path. If the value is null, the component will not have a data source specified.</remarks>
    [Parameter]
    public string? Source { get; set; }

    /// <summary>
    /// Gets or sets the URL of the poster image associated with the video content.
    /// </summary>
    /// <remarks>Set this property to null if no poster image is available. The URL should point to a valid
    /// and accessible image resource to ensure the poster is displayed correctly.</remarks>
    [Parameter]
    public string? Poster { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the component should be initialized lazily.
    /// </summary>
    /// <remarks>When set to <see langword="true"/>, the component defers its initialization until it is first
    /// needed. This can improve performance in scenarios where the component may not be used immediately.</remarks>
    [Parameter]
    public bool IsLazy { get; set; } = true;

    /// <summary>
    /// Gets or sets the current state of the slideshow, which manages the presentation flow and user interactions.
    /// </summary>
    /// <remarks>The state is injected and should be configured before use to ensure proper functionality of
    /// the slideshow.</remarks>
    [Inject]
    private SlideshowState State { get; set; } = default!;

    /// <summary>
    /// Gets or sets the parent slideshow item associated with the current item.
    /// </summary>
    /// <remarks>This property is used to establish a hierarchical relationship between slideshow items. It is
    /// important to ensure that the parent item is set correctly to maintain the intended structure of the
    /// slideshow.</remarks>
    [CascadingParameter]
    private SlideshowItem<TItem>? Parent { get; set; }

    /// <inheritdoc/>
    protected override void OnInitialized()
    {
        base.OnInitialized();
        Parent?.Add(this);
    }

    /// <inheritdoc/>
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

    /// <inheritdoc/>
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (firstRender)
        {
            _jsModule = new SlideshowVideoJS<TItem>(JSModule, State);
            await _jsModule.Initialize(Id);
        }
    }
}
