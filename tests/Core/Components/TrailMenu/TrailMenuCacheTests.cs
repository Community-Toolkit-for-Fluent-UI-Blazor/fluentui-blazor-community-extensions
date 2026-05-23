using FluentUI.Blazor.Community.Components.TrailMenu;
using Xunit;

namespace Components.Tests.Components.TrailMenu;

public class TrailMenuCacheTests
{
    [Fact]
    public void SetAndTryGet_ReturnsStoredValue()
    {
        var cache = new TrailMenuCache();

        cache.Set("id", 12.5);

        Assert.True(cache.TryGet("id", out var size));
        Assert.Equal(12.5, size);
    }

    [Fact]
    public void TotalSize_SumsValues()
    {
        var cache = new TrailMenuCache();

        cache.Set("a", 10);
        cache.Set("b", 5);

        Assert.Equal(15, cache.TotalSize);
    }

    [Fact]
    public void Invalidate_ResetsSize()
    {
        var cache = new TrailMenuCache();

        cache.Set("a", 10);
        cache.Invalidate("a");

        Assert.True(cache.TryGet("a", out var size));
        Assert.Equal(0, size);
    }

    [Fact]
    public void Clear_RemovesIds()
    {
        var cache = new TrailMenuCache();

        cache.Set("a", 1);
        cache.Set("b", 2);

        cache.Clear(["a", "", null]);

        Assert.False(cache.TryGet("a", out _));
        Assert.True(cache.TryGet("b", out _));
    }
}
