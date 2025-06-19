using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

public partial class AIPromptView
    : FluentComponentBase
{
    [CascadingParameter]
    private FluentCxAIPrompt Parent { get; set; } = default!;

    private bool HasSuggestion => Parent?.Suggestions?.Any() ?? false;

    private string SuggestionAccordionId { get; } = Guid.NewGuid().ToString();

    private async Task OnTappedAsync(string? value)
    {
        if (!string.IsNullOrEmpty(value))
        {
            await Parent.HandlePromptChangedAsync(value);
        }
    }

    private async Task HandlePromptChangedAsync()
    {
        if (Parent is not null)
        {
            await Parent.HandlePromptChangedAsync(Parent.Prompt);
        }
    }

    private async Task OnKeyDownAsync(KeyboardEventArgs e, string value)
    {
        if (e.Key == "Enter")
        {
            await Parent.HandlePromptChangedAsync(value);
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
