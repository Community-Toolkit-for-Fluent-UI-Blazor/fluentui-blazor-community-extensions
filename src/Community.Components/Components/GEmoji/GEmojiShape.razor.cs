using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.JSInterop;

namespace FluentUI.Blazor.Community.Components;

public partial class GEmojiShape
    : FluentComponentBase
{
    private string? _value;
    private bool _executeAfterRender;
    private IJSObjectReference? _module;
    private const string JavascriptFile = "./_content/FluentUI.Blazor.Community.Components/GEmoji/GEmoji.js";

    public GEmojiShape()
    {
        Id = Identifier.NewId();
    }

    [Inject]
    private IJSRuntime JSRuntime { get; set; } = default!;

    [Parameter]
    public string? Value { get; set; }

    [JSInvokable]
    public string MarkupContent(string value)
    {
        return value.MarkupContent();
    }

    private async Task OnSetEmojiAsync()
    {
        if (_module is not null)
        {
            await _module.InvokeVoidAsync("emojify", Id, _value);
        }
    }

    public override async Task SetParametersAsync(ParameterView parameters)
    {
        await base.SetParametersAsync(parameters);

        if (parameters.TryGetValue(nameof(Value), out string? value) &&
            !string.Equals(value, _value, StringComparison.Ordinal))
        {
            _value = value;
            _executeAfterRender = true;
        }
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (firstRender)
        {
            _module = await JSRuntime.InvokeAsync<IJSObjectReference>(
                "import",
                JavascriptFile);

            await _module.InvokeVoidAsync("initialize", Id, Element, DotNetObjectReference.Create(this));
        }

        if (_executeAfterRender)
        {
            _executeAfterRender = false;
            await OnSetEmojiAsync();
        }
    }
}
