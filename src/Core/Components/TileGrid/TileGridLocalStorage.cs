using System.Text.Json;
using Microsoft.JSInterop;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides a tile grid storage implementation that persists items in the browser's local storage using JSON
/// serialization.
/// </summary>
/// <remarks>This class uses the JavaScript runtime to interact with the browser's local storage. Items are
/// serialized to JSON when saved and deserialized when loaded. Ensure that the item type supports JSON serialization
/// and deserialization. Data stored in local storage is scoped to the user's browser and persists across sessions
/// unless explicitly cleared.</remarks>
/// <typeparam name="TItem">The type of items to store in the grid. Must be serializable to and from JSON.</typeparam>
public sealed class TileGridLocalStorage<TItem> : ITileGridStorage<TItem>
{
    /// <summary>
    /// Represents the JavaScript runtime instance used to enable interoperability between .NET and JavaScript code.
    /// </summary>
    /// <remarks>This field is typically initialized via dependency injection and is required for invoking
    /// JavaScript functions from .NET. Ensure that the JavaScript runtime is available before making interop calls to
    /// prevent runtime errors.</remarks>
    private readonly IJSRuntime _js;

    /// <summary>
    /// Initializes a new instance of the LocalStorageTileGridStorage class with the specified JavaScript runtime.
    /// </summary>
    /// <param name="js">Javascript runtime.</param>
    public TileGridLocalStorage(IJSRuntime js)
    {
        _js = js;
    }

    /// <inheritdoc />
    public async Task SaveAsync(string gridId, IEnumerable<TItem> items)
    {
        var json = JsonSerializer.Serialize(items);
        await _js.InvokeVoidAsync("localStorage.setItem", gridId, json);
    }

    /// <inheritdoc />
    public async Task<List<TItem>?> LoadAsync(string gridId)
    {
        var json = await _js.InvokeAsync<string>("localStorage.getItem", gridId);

        if (string.IsNullOrWhiteSpace(json))
        {
            return null;
        }

        return JsonSerializer.Deserialize<List<TItem>>(json);
    }
}
