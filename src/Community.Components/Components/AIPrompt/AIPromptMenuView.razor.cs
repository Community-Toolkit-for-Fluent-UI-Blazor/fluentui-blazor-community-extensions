using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

public partial class AIPromptMenuView
    : FluentComponentBase
{
    private readonly RenderFragment _renderMenus;
    private readonly RenderFragment<IEnumerable<AIPromptMenu>?> _renderItems;
    private readonly RenderFragment<AIPromptMenu> _renderMenu;

    [CascadingParameter]
    public FluentCxAIPrompt? Parent { get; set; }

    [Parameter]
    public required string AnchorId { get; set; }

    [Parameter]
    public bool IsMenuOpen { get; set; }

    private async Task OnHandleMenuClickAsync(AIPromptMenu menu)
    {
        if (Parent is not null)
        {
            await Parent.HandleMenuClickAsync(menu);
        }
    }
}
