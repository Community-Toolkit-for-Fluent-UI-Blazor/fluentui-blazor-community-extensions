// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System.Text;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.JSInterop;

namespace FluentUI.Blazor.Community.Components;

public partial class AIPromptResultView
    : FluentComponentBase, IAsyncDisposable
{
    private readonly DotNetObjectReference<AIPromptResultView> _aiPromptResultView;
    private IJSObjectReference? _module;
    private const string JavascriptFile = "./_content/FluentUI.Blazor.Community.Components/js/clipboard.js";

    public AIPromptResultView()
    {
        _aiPromptResultView = DotNetObjectReference.Create(this);
    }

    [Inject]
    public required IJSRuntime JSRuntime { get; set; }

    [Inject]
    public required IToastService ToastService { get; set; }

    [CascadingParameter]
    private FluentCxAIPrompt? Parent { get; set; }

    private static string Format(string? text)
    {
        if (string.IsNullOrEmpty(text))
        {
            return string.Empty;
        }

        StringBuilder stringBuilder = new(text);
        stringBuilder.Replace("\r", string.Empty);
        stringBuilder.Replace("\n", "<br />");

        return stringBuilder.ToString();
    }

    private async Task OnCopyAsync(AIPromptResult value)
    {
        if (_module is not null)
        {
            await _module.InvokeVoidAsync("copyToClipboard", _aiPromptResultView, value.Result?.Text);
        }
    }

    private async Task OnChangeRatingAsync(AIPromptResult result, AIPromptRating value)
    {
        result.Rating = value;

        if (Parent?.OnRateChanged.HasDelegate ?? false)
        {
            await Parent.OnRateChanged.InvokeAsync(result);
        }
    }

    private async Task OnRetryAsync(AIPromptResult value)
    {
        if (Parent is not null)
        {
            if (!string.IsNullOrEmpty(value.Prompt))
            {
                await Parent.HandleRequestAsync(value);
            }
            else if (!string.IsNullOrEmpty(value.CommandId))
            {
                var menuCommand = AIPromptMenu.Find(Parent.Menus, value.CommandId);

                if (menuCommand is null)
                {
                    return;
                }

                await Parent.HandleMenuClickAsync(menuCommand, value);
            }
        }
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (firstRender)
        {
            _module = await JSRuntime.InvokeAsync<IJSObjectReference>("import", JavascriptFile);
        }
    }

    [JSInvokable]
    public void OnCopied()
    {
        var value = Parent?.Labels.CopiedText;

        if (!string.IsNullOrEmpty(value))
        {
            ToastService.ShowSuccess(value);
        }
    }

    public async ValueTask DisposeAsync()
    {
        try
        {
            if (_module is not null)
            {
                await _module.DisposeAsync();
                _module = null;
            }
        }
        catch (JSDisconnectedException)
        {

        }
    }

    public void Refresh()
    {
        StateHasChanged();
    }

    public async ValueTask RefreshAsync()
    {
        await InvokeAsync(StateHasChanged);
    }
}
