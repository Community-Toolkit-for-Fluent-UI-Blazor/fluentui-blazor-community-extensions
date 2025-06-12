// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using FluentUI.Blazor.Community.Helpers;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.AI;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

public partial class FluentCxAIPrompt
    : FluentComponentBase
{
    private readonly List<AIPromptResult> _results = [];
    private string? _activeView;
    private AIPromptView? _aiPromptView;
    private AIPromptResultView? _aiPromptResultView;

    public FluentCxAIPrompt()
    {
        Id = StringHelper.GenerateId();
        _activeView = $"aiprompt-view-{Id}";
    }

    [Inject]
    public required IChatClient ChatClient { get; set; }

    [Parameter]
    public AIPromptLabels Labels { get; set; } = AIPromptLabels.Default;

    [Parameter]
    public string? Width { get; set; }

    [Parameter]
    public string? Height { get; set; }

    [Parameter]
    public EventCallback<AIPromptMenuEventArgs> OnMenuSelected { get; set; }

    [Parameter]
    public EventCallback<AIPromptResult> OnRateChanged { get; set; }

    [Parameter]
    public EventCallback<AIPromptResult> OnResponseGenerated { get; set; }

    [Parameter]
    public bool IsSuggestionExpanded { get; set; } = true;

    [Parameter]
    public bool IsRatingVisible { get; set; }

    [Parameter]
    public string? Prompt { get; set; }

    [Parameter]
    public EventCallback<string?> PromptChanged { get; set; }

    [Parameter]
    public IEnumerable<string> Suggestions { get; set; } = [];

    [Parameter]
    public RenderFragment<string>? SuggestionItemTemplate { get; set; }

    [Parameter]
    public IEnumerable<AIPromptMenu> Menus { get; set; } = [];

    [Parameter]
    public RenderFragment<AIPromptResult>? ResultItemTemplate { get; set; }

    internal bool IsRequestProcessing { get; set; }

    internal List<AIPromptResult> Results => _results;

    internal bool IsMenuOpen {  get; set; }

    internal async Task HandleMenuClickAsync(AIPromptMenu menu, AIPromptResult? result = null)
    {
        var e = new AIPromptMenuEventArgs()
        {
            Command = menu,
            Result = result,
        };

        if (OnMenuSelected.HasDelegate)
        {
            await OnMenuSelected.InvokeAsync(e);
        }

        if (!e.Cancel)
        {
            var title = menu.Label;

            if (menu.Parent is not null)
            {
                title = $"{menu.Parent.Label}: {menu.Label}";
            }

            Results.Insert(0, new AIPromptResult(title, null, null, menu.Id, e.Result?.Result, result is not null));
        }
    }

    internal async Task HandlePromptChangedAsync(string? value)
    {
        Prompt = value;

        if (PromptChanged.HasDelegate)
        {
            await PromptChanged.InvokeAsync(value);
        }
    }

    public async Task GetResponseAsync(string prompt)
    {
        await InternalResponseAsync(prompt);
    }

    internal async Task HandleRequestAsync(AIPromptResult? result = null)
    {
        await InternalResponseAsync(null, result);
    }

    private async Task InternalResponseAsync(string? prompt, AIPromptResult? result = null)
    {
        if (ChatClient is null)
        {
            throw new InvalidOperationException("An instance of IChatClient should be provided to the component.");
        }

        IsRequestProcessing = true;
        _activeView = $"aiprompt-result-view-{Id}";
        await InvokeAsync(StateHasChanged);
        await RefreshChildrenAsync();

        var promptToUse = prompt ?? result?.Prompt ?? Prompt;
        List<ChatMessage> messages = [];
        messages.Add(new ChatMessage()
        {
            Role = ChatRole.System,
            Contents = [
                new TextContent(Labels.SystemPrompt)
                ]
        });

        messages.Add(new ChatMessage()
        {
            Role = ChatRole.User,
            Contents = [
                new TextContent(promptToUse)
            ]
        });

        if (result is not null)
        {
            messages.Add(new ChatMessage()
            {
                Role = ChatRole.Assistant,
                RawRepresentation = result.Result
            });

            messages.Add(new ChatMessage()
            {
                Role = ChatRole.User,
                Contents = [
                    new TextContent(promptToUse)
                ]
            });
        }

        var res = await ChatClient.GetResponseAsync(messages);

        AIPromptResult outputRes = new(Labels.OutputResultTitle, Prompt, promptToUse, null, res, result is not null);
        _results.Insert(0, outputRes);
        Prompt = null;

        IsRequestProcessing = false;
        await RefreshChildrenAsync();
        
        if (OnResponseGenerated.HasDelegate)
        {
            await OnResponseGenerated.InvokeAsync(outputRes);
        }
    }

    private async Task RefreshChildrenAsync()
    {
        if (_activeView == $"aiprompt-result-view-{Id}" &&
            _aiPromptResultView is not null)
        {
            await _aiPromptResultView.RefreshAsync();
        }
        else if (_activeView == $"aiprompt-view-{Id}" &&
                 _aiPromptView is not null)
        {
            await _aiPromptView.RefreshAsync();
        }
    }
}
