using System.Text.Json;
using FluentUI.Blazor.Community.Components;
using Microsoft.JSInterop;

namespace FluentUI.Demo.Shared.Infrastructure;

internal sealed class SchedulerStorage(IJSRuntime js)
{
    public async ValueTask StoreAsync(List<SchedulerItem<string>> items)
    {
        await js.InvokeVoidAsync("localStorage.setItem", "FluentCxScheduler", JsonSerializer.Serialize(items));
    }

    public async ValueTask<List<SchedulerItem<string>>> RetrieveAsync()
    {
        var json = await js.InvokeAsync<string?>("localStorage.getItem", "FluentCxScheduler");

        if (string.IsNullOrWhiteSpace(json))
        {
            return [];
        }

        return JsonSerializer.Deserialize<List<SchedulerItem<string>>>(json)
               ?? [];
    }

    public async ValueTask ClearAsync()
    {
        await js.InvokeVoidAsync("localStorage.removeItem", "FluentCxScheduler");
    }
}
