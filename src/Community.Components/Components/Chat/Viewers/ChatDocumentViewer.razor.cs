using System.Linq.Expressions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the viewer for the documents.
/// </summary>
public partial class ChatDocumentViewer
    : FluentComponentBase
{
    /// <summary>
    /// Gets or sets the filter of the documents.
    /// </summary>
    [Parameter]
    public Expression<Func<IChatFile, bool>>? Filter { get; set; }

    /// <summary>
    /// Gets or sets the items provider.
    /// </summary>
    [Parameter]
    public ItemsProviderDelegate<IChatFile>? ItemsProvider { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    private async Task OnItemSelectedAsync(IChatFile item)
    {
        await Task.CompletedTask;
    }
}
