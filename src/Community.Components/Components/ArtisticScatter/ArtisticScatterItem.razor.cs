using System.Globalization;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;
using Microsoft.JSInterop;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the image inside a <see cref="FluentCxArtisticScatter"/>.
/// </summary>
public partial class ArtisticScatterItem
    : FluentComponentBase, IDisposable, IAsyncDisposable
{
    #region Fields

    /// <summary>
    /// Represents the top position of the image.
    /// </summary>
    private int _top;

    /// <summary>
    /// Represents the left position of the image.
    /// </summary>
    private int _left;

    /// <summary>
    /// Represents the rotation angle of the image.
    /// </summary>
    private int _rotation;

    /// <summary>
    /// Represents the rotation angle of the image in the mobile.
    /// </summary>
    private int _rotationMobile;

    /// <summary>
    /// Represents the opacity of the image.
    /// </summary>
    private double _opacity;

    /// <summary>
    /// Represents the z-index of the image.
    /// </summary>
    private int _zIndex;

    /// <summary>
    /// Represents the animation delay of the image.
    /// </summary>
    private double _animationDelay;

    /// <summary>
    /// Represents the invariant culture for formatting.
    /// </summary>
    private static readonly CultureInfo InvariantCulture = CultureInfo.InvariantCulture;

    /// <summary>
    /// Represents the translation offset of the image.
    /// </summary>
    private int _translate;

    /// <summary>
    /// Represents whether the animation has completed.
    /// </summary>
    private bool _animationCompleted;

    /// <summary>
    /// Represents a reference to the current instance for JavaScript interop.
    /// </summary>
    private readonly DotNetObjectReference<ArtisticScatterItem> _dotNetRef;

    /// <summary>
    /// Represents the JavaScript module for interop.
    /// </summary>
    private IJSObjectReference? _module;

    #endregion Fields

    #region Properties

    /// <summary>
    /// Gets or sets the internal renderer.
    /// </summary>
    internal RenderFragment InternalRenderer { get; private set; }

    /// <summary>
    /// Gets or sets the parent of the component.
    /// </summary>
    [CascadingParameter]
    private FluentCxArtisticScatter? Parent { get; set; }

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
    /// Gets or sets the width of the image.
    /// </summary>
    [Parameter]
    public string? Width { get; set; }

    /// <summary>
    /// Gets or sets the height of the image.
    /// </summary>
    [Parameter]
    public string? Height { get; set; }

    /// <summary>
    /// Gets or sets the border radius of the image.
    /// </summary>
    [Parameter]
    public string? BorderRadius { get; set; }

    /// <summary>
    /// Gets the css for the image.
    /// </summary>
    private string? InternalCss => new CssBuilder(Class)
        .AddClass("fluent-cx-artistic-scatter-item")
        .Build();

    /// <summary>
    /// Gets the style for the image.
    /// </summary>
    private string? InternalStyle => new StyleBuilder(Style)
        .AddStyle("--artistic-scatter-item-width", Width, !string.IsNullOrEmpty(Width))
        .AddStyle("--artistic-scatter-item-height", Height, !string.IsNullOrEmpty(Height))
        .AddStyle("--artistic-scatter-item-border-radius", BorderRadius, !string.IsNullOrEmpty(BorderRadius))
        .AddStyle("position", "absolute", !Parent!.IsMobile)
        .AddStyle("top", $"{_top}px", !Parent!.IsMobile)
        .AddStyle("left", $"{_left}px", !Parent!.IsMobile)
        .AddStyle("transform", $"rotate({_rotation}deg) !important", !Parent.IsMobile && _animationCompleted)
        .AddStyle("--artistic-scatter-item-opacity", $"{(!Parent!.IsMobile ? _opacity.ToString(InvariantCulture) : 1.0)}")
        .AddStyle("z-index", $"{_zIndex}")
        .AddStyle("animation-delay", $"{_animationDelay}s")
        .AddStyle("transform", $"rotate({_rotationMobile}deg) translateY({_translate}px) !important", Parent!.IsMobile && _animationCompleted)
        .Build();

    /// <summary>
    /// Gets or sets the JavaScript runtime.
    /// </summary>
    [Inject]
    private IJSRuntime Runtime { get; set; } = default!;

    #endregion Properties

    #region Methods

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        base.OnInitialized();

        if (Parent is null)
        {
            throw new InvalidOperationException("The ArtisticScatterItem can only be used only inside a FluentCxArtisticScatter component");
        }

        Parent.Add(this);

        var index = Parent.IndexOf(this);
        _rotationMobile = index * 20 - 60;
        _rotation = (index % 2 == 0 ? 1 : -1) * (5 + (index * 3));
        _opacity = 1.0 - (1.0 / Parent.DisplayedItemCount * index);
        _zIndex = 100 - index;
        _top = !Parent.IsMobile ? 20 + index * 12 : index * 10;
        _left = !Parent.IsMobile ? 20 + index * 18 : index * 10;
        _translate = index * 10;
        _animationDelay = index * 0.2;
    }

    /// <inheritdoc />
    public void Dispose()
    {
        Parent?.Remove(this);

        GC.SuppressFinalize(this);
    }

    /// <inheritdoc />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _module = await Runtime.InvokeAsync<IJSObjectReference>("import", "./_content/FluentUI.Blazor.Community.Components/Components/ArtisticScatter/ArtisticScatterItem.razor.js");
            await _module.InvokeVoidAsync("initialize", Element, _dotNetRef);
        }
    }

    /// <summary>
    /// Occurs when the animation is completed.
    /// </summary>
    [JSInvokable]
    public void OnAnimationCompleted()
    {
        _animationCompleted = true;
        Parent!.InvalidateRender();
    }

    /// <inheritdoc />
    public async ValueTask DisposeAsync()
    {
        try
        {
            if (_module is not null)
            {
                await _module.InvokeVoidAsync("dispose", Element);
                await _module.DisposeAsync();
            }
        }
        catch (JSDisconnectedException) { }

        GC.SuppressFinalize(this);
    }

    #endregion Methods
}
