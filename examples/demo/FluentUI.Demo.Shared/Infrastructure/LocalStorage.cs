using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;
using Microsoft.JSInterop;

namespace FluentUI.Demo.Shared.Infrastructure;

public class LocalStorage(IJSRuntime js) : JSModule(js, "./_content/FluentUI.Demo.Shared/js/localStorage.js")
{
    public async ValueTask SetItemAsync<T>(string key, T value)
    {
        await InvokeVoidAsync("setItem", key, JsonSerializer.Serialize(value));
    }

    public async ValueTask<T?> GetItemAsync<T>(string key)
    {
        var value = await InvokeAsync<string?>("getItem", key);

        if (string.IsNullOrEmpty(value))
        {
            return default;
        }

        return JsonSerializer.Deserialize<T?>(value);
    }
    public async ValueTask RemoveItemAsync(string key)
    {
        await InvokeVoidAsync("removeItem", key);
    }

    public async ValueTask ClearAsync()
    {
        await InvokeVoidAsync("clear");
    }
}
