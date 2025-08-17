using FluentUI.Blazor.Community.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;
using Microsoft.JSInterop;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a reply message.
/// </summary>
public partial class ChatMessageReply
    : FluentComponentBase
{
    /// <summary>
    /// Value indicating if the text part of the reply must be refreshed.
    /// </summary>
    private bool _refreshText;

    /// <summary>
    /// Javascript file to use.
    /// </summary>
    private const string JAVASCRIPT_FILE = "./_content/FluentUI.Blazor.Community.Components/Components/Chat/ChatMessageReply.razor.js";

    /// <summary>
    /// Javascript module.
    /// </summary>
    private IJSObjectReference? _module;

    /// <summary>
    /// Initializes a new instance of the <see cref="ChatMessageReply"/>.
    /// </summary>
    public ChatMessageReply()
    {
        Id = Identifier.NewId();
    }

    /// <summary>
    /// Gets or sets the javascript runtime.
    /// </summary>
    [Inject]
    private IJSRuntime JSRuntime { get; set; } = default!;

    /// <summary>
    /// Gets the css to use.
    /// </summary>
    private string? ClassValue => new CssBuilder(Class)
        .AddClass("chat-reply")
        .AddClass("chat-reply-in-card", InsideCard)
        .Build();

    /// <summary>
    /// Gets the style to use.
    /// </summary>
    private string? StyleValue => new StyleBuilder(Style).Build();

    /// <summary>
    /// Gets or sets the text of the reply.
    /// </summary>
    [Parameter]
    public string? Text { get; set; }

    /// <summary>
    /// Gets or sets a value indicating if the reply can be dismissed.
    /// </summary>
    [Parameter]
    public bool AllowDismiss { get; set; }

    /// <summary>
    /// Gets or sets the callback to raise when the reply is dismissed.
    /// </summary>
    [Parameter]
    public EventCallback OnDismiss { get; set; }

    /// <summary>
    /// Gets or sets if the reply is inside the card.
    /// </summary>
    [Parameter]
    public bool InsideCard { get; set; }

    /// <summary>
    /// Occurs when the reply is dismissed.
    /// </summary>
    /// <returns>Returns a task which dismiss the reply when completed.</returns>
    private async Task OnDismissAsync()
    {
        if (OnDismiss.HasDelegate)
        {
            await OnDismiss.InvokeAsync();
        }
    }

    /// <inheritdoc />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (firstRender)
        {
            _module = await JSRuntime.InvokeAsync<IJSObjectReference>("import", JAVASCRIPT_FILE);
            await _module.InvokeVoidAsync("initialize", Id);
        }
    }

    /// <inheritdoc />
    protected override async Task OnParametersSetAsync()
    {
        await base.OnParametersSetAsync();

        if (_refreshText &&
            _module is not null)
        {
            _refreshText = false;
            await _module.InvokeVoidAsync("setText", Id, Text);
        }
    }

    /// <inheritdoc />
    public override async Task SetParametersAsync(ParameterView parameters)
    {
        _refreshText = parameters.HasValueChanged(nameof(Text), Text);

        await base.SetParametersAsync(parameters);
    }
}
