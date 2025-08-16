using System.Globalization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;
using Microsoft.JSInterop;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a slideshow for a chat message. 
/// </summary>
public partial class ChatMessageSlideShow
    : FluentComponentBase, IAsyncDisposable
{
    private bool _showContent;
    private const string JavaScriptFileName = "./_content/FluentUI.Blazor.Community.Components/Components/Chat/Slideshow/ChatMessageSlideShow.razor.js";
    private IJSObjectReference? _module;
    private string? _width;
    private string? _height;
    private DotNetObjectReference<ChatMessageSlideShow>? _dotNetReference;

    /// <summary>
    /// Initializes a new instance of the <see cref="ChatMessageSlideShow"/> class.
    /// </summary>
    public ChatMessageSlideShow()
    {
        Id = Identifier.NewId();
        _dotNetReference = DotNetObjectReference.Create(this);
    }

    /// <summary>
    /// Gets or sets the owner of the view.
    /// </summary>
    [Parameter]
    public ChatUser? Owner { get; set; }

    /// <summary>
    /// Gets or sets the message to render.
    /// </summary>
    [Parameter]
    public IChatMessage? Message { get; set; }

    /// <summary>
    /// Gets or sets the loading label.
    /// </summary>
    [Parameter]
    public string? LoadingLabel { get; set; }

    [Inject]
    private IJSRuntime Runtime { get; set; } = default!;

    private bool ShowControlsAndIndicators => Message is not null && (Message.Sections.Count > 0 ? (Message.Files.Count > 0 ? true : false) :
                                              Message.Files.Count > 1);

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (firstRender)
        {
            _module = await Runtime.InvokeAsync<IJSObjectReference>("import", JavaScriptFileName);
            await _module.InvokeVoidAsync("updateSize", Id, _dotNetReference);
        }
    }

    [JSInvokable("getParentSize")]
    public void GetParentSize(double width, double height)
    {
        _width = $"{Math.Floor(width).ToString(CultureInfo.InvariantCulture)}px";
        _height = $"{Math.Floor(height).ToString(CultureInfo.InvariantCulture)}px";
        _showContent= true;
        StateHasChanged();
    }

    public async ValueTask DisposeAsync()
    {
        try
        {
            if (_module is not null)
            {
                await _module.DisposeAsync();
            }

            _dotNetReference?.Dispose();
        }
        catch (JSDisconnectedException)
        { }
    }
}
