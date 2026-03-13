using FluentUI.Blazor.Community.Components;
using Xunit;

namespace Components.Tests.Components.Animations.Engine;

public class MotionPayloadTests
{
    [Fact]
    public void SetAndGet_ReturnsStoredValue()
    {
        var payload = new MotionPayload();

        payload.Set("x", 42);

        Assert.True(payload.Contains("x"));
        Assert.Equal(42, payload.Get<int>("x"));
    }

    [Fact]
    public void Clone_CopiesValues()
    {
        var payload = new MotionPayload();
        payload.Set("x", 5);

        var clone = payload.Clone();
        payload.Clear();

        Assert.Equal(5, clone.Get<int>("x"));
        Assert.False(payload.Contains("x"));
    }
}
