// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.Extensions.AI;

namespace FluentUI.Blazor.Community.Components;

public record AIPromptResult(string? Title, string? Subtitle, string? Prompt, string? CommandId, ChatResponse? Result, bool Retried)
{
    public AIPromptRating Rating { get; set; }
}
