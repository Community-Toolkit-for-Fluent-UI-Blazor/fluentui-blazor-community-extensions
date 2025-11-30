using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentUI.Blazor.Community.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;
using Microsoft.JSInterop;

namespace FluentUI.Demo.Shared.Infrastructure;

internal class SchedulerStorage(IJSRuntime js) : JSModule(js, "./_content/FluentUI.Demo.Shared/js/scheduler-storage.js")
{
    public async ValueTask StoreAsync(List<SchedulerItem<string>> items)
    {
        await InvokeVoidAsync("store", items);
    }

    public async ValueTask<List<SchedulerItem<string>>> RetrieveAsync()
    {
        var items = await InvokeAsync<List<SchedulerItem<string>>>("retrieve");

        return items;
    }
}
