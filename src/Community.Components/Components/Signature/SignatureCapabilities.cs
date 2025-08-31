using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

public sealed class SignatureCapabilities
    : FluentComponentBase
{
    [Parameter]
    public bool CanUndo { get; set; } = true;

    [Parameter]
    public bool CanRedo { get; set; } = true;

    [Parameter]
    public bool CanClear { get; set; } = true;

    [Parameter]
    public bool CanExport { get; set; } = true;

    [Parameter]
    public bool CanErase { get; set; } = true;

    [Parameter]
    public bool CanMaximize { get; set; } = true;
}
