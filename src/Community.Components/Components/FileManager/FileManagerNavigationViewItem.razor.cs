// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

public partial class FileManagerNavigationViewItem
    : FluentComponentBase
{
    [Parameter]
    public string? Text { get; set; }

    [Parameter]
    public EventCallback<FileManagerNavigationViewItem> OnClick { get; set; }

    private async Task OnClickItemAsync()
    {
        if (OnClick.HasDelegate)
        {
            await OnClick.InvokeAsync(this);
        }
    }
}
