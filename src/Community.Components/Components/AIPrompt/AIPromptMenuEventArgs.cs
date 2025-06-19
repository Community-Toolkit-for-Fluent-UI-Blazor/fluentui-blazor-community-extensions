using System.ComponentModel;

namespace FluentUI.Blazor.Community.Components;

public sealed class AIPromptMenuEventArgs
    : CancelEventArgs
{
    public AIPromptResult? Result { get; internal set; }

    public AIPromptMenu? Command { get; internal set; }
}
