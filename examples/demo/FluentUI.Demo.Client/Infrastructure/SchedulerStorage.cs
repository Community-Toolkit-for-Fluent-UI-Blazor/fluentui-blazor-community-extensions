using System.Text.Json;
using FluentUI.Blazor.Community.Components;
using Microsoft.JSInterop;

namespace FluentUI.Demo.Shared.Infrastructure;

internal sealed class SchedulerStorage(IJSRuntime js)
{
    public async ValueTask InitializeAsync(string? filename)
    {
        var json = await js.InvokeAsync<string?>("localStorage.getItem", "FluentCxScheduler");

        if (string.IsNullOrEmpty(json))
        {
            await js.InvokeVoidAsync("localStorage.setItem", "FluentCxScheduler", filename);
        }
    }

    public async ValueTask StoreAsync(List<SchedulerItem<string>> items)
    {
        var json = await js.InvokeAsync<string?>("localStorage.getItem", "FluentCxScheduler");

        if (!string.IsNullOrEmpty(json))
        {
            await File.WriteAllTextAsync(json, JsonSerializer.Serialize(items));
        }
    }

    public async ValueTask<List<SchedulerItem<string>>> RetrieveAsync()
    {
        var json = await js.InvokeAsync<string?>("localStorage.getItem", "FluentCxScheduler");

        if (string.IsNullOrWhiteSpace(json))
        {
            return [];
        }

        var data = await File.ReadAllTextAsync(json);

        if (string.IsNullOrEmpty(data))
        {
            return [];
        }

        return JsonSerializer.Deserialize<List<SchedulerItem<string>>>(data)
               ?? [];
    }

    public async ValueTask ClearAsync()
    {
        await js.InvokeVoidAsync("localStorage.removeItem", "FluentCxScheduler");
    }
}
