using System.Text.Json;
using FluentUI.Blazor.Community.Components;
using Microsoft.JSInterop;
using Xunit;

namespace Components.Tests.Components.TileGrid;

public class TileGridLocalStorageTests
{
    [Fact]
    public async Task SaveAsync_SerializesAndStoresItems()
    {
        var jsRuntime = new FakeJsRuntime();
        var storage = new TileGridLocalStorage<TestItem>(jsRuntime);
        var items = new List<TestItem>
        {
            new() { Id = 1, Name = "First" },
            new() { Id = 2, Name = "Second" }
        };

        await storage.SaveAsync("grid", items);

        Assert.True(jsRuntime.Storage.TryGetValue("grid", out var stored));
        Assert.Equal(JsonSerializer.Serialize(items), stored);
    }

    [Fact]
    public async Task LoadAsync_ReturnsNull_WhenStorageIsEmpty()
    {
        var jsRuntime = new FakeJsRuntime();
        var storage = new TileGridLocalStorage<TestItem>(jsRuntime);

        var result = await storage.LoadAsync("grid");

        Assert.Null(result);
    }

    [Fact]
    public async Task LoadAsync_ReturnsDeserializedItems()
    {
        var jsRuntime = new FakeJsRuntime();
        var storage = new TileGridLocalStorage<TestItem>(jsRuntime);
        var items = new List<TestItem>
        {
            new() { Id = 3, Name = "Third" },
            new() { Id = 4, Name = "Fourth" }
        };
        jsRuntime.Storage["grid"] = JsonSerializer.Serialize(items);

        var result = await storage.LoadAsync("grid");

        Assert.NotNull(result);
        Assert.Collection(result,
            item =>
            {
                Assert.Equal(3, item.Id);
                Assert.Equal("Third", item.Name);
            },
            item =>
            {
                Assert.Equal(4, item.Id);
                Assert.Equal("Fourth", item.Name);
            });
    }

    private sealed class FakeJsRuntime : IJSRuntime
    {
        public Dictionary<string, string?> Storage { get; } = new();

        public ValueTask<TValue> InvokeAsync<TValue>(string identifier, object?[]? args)
        {
            return HandleInvocation<TValue>(identifier, args);
        }

        public ValueTask<TValue> InvokeAsync<TValue>(string identifier, CancellationToken cancellationToken, object?[]? args)
        {
            return HandleInvocation<TValue>(identifier, args);
        }

        private ValueTask<TValue> HandleInvocation<TValue>(string identifier, object?[]? args)
        {
            if (identifier == "localStorage.setItem")
            {
                var key = args?[0]?.ToString() ?? string.Empty;
                var value = args?[1]?.ToString();
                Storage[key] = value;
                return new ValueTask<TValue>(default(TValue)!);
            }

            if (identifier == "localStorage.getItem")
            {
                var key = args?[0]?.ToString() ?? string.Empty;
                Storage.TryGetValue(key, out var value);
                return new ValueTask<TValue>((TValue)(object?)value!);
            }

            throw new InvalidOperationException($"Unexpected JS identifier '{identifier}'.");
        }
    }

    private sealed class TestItem
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;
    }
}
