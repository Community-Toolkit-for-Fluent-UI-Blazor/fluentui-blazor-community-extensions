using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

public class AIPromptMenu
{
    public string? Id { get; set; }

    public string? Label { get; set; }

    public Icon? Icon { get; set; }

    internal List<AIPromptMenu> Items { get; set; } = [];

    public AIPromptMenu? Parent { get; private set; }

    public void AddRange(params AIPromptMenu[] items)
    {
        foreach (var item in items)
        {
            item.Parent = this;
        }

        Items.AddRange(items);
    }

    internal static AIPromptMenu? Find(IEnumerable<AIPromptMenu> menus, string commandId)
    {
        foreach (var menu in menus)
        {
            if (string.Equals(menu.Id, commandId, StringComparison.OrdinalIgnoreCase))
            {
                return menu;
            }

            var subMenu = Find(menu.Items, commandId);

            if (subMenu is not null)
            {
                return subMenu;
            }
        }

        return null;
    }
}
