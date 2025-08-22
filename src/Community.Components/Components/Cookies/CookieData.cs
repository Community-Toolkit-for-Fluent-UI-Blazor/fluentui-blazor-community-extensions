using Microsoft.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the content of the <see cref="ManageCookie"/> dialog.
/// </summary>
/// <param name="Items">Items to activate / deactivate.</param>
/// <param name="Labels">Labels to localize some part of the <see cref="ManageCookie"/> dialog.</param>
/// <param name="ItemTemplate">Template to render the items in the dialog.</param>
public record CookieData(IEnumerable<CookieItem> Items, CookieLabels Labels, RenderFragment<CookieItem>? ItemTemplate = null)
{
}
