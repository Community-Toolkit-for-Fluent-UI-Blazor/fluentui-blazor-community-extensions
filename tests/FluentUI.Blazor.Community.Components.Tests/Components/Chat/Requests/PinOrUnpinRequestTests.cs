namespace FluentUI.Blazor.Community.Components.Tests.Components.Chat;

public class PinOrUnpinRequestTests
{
    [Fact]
    public void Constructor_SetsProperties()
    {
        var message = new ChatMessage();
        var request = new PinOrUnpinRequest(42, message, true);

        Assert.Equal(42, request.RoomId);
        Assert.Equal(message, request.Message);
        Assert.True(request.Pin);
    }

    [Fact]
    public void Equality_WorksForSameValues()
    {
        var message = new ChatMessage();
        var req1 = new PinOrUnpinRequest(1, message, false);
        var req2 = new PinOrUnpinRequest(1, message, false);

        Assert.Equal(req1, req2);
        Assert.True(req1 == req2);
        Assert.False(req1 != req2);
        Assert.Equal(req1.GetHashCode(), req2.GetHashCode());
    }

    [Fact]
    public void Inequality_WorksForDifferentValues()
    {
        var message = new ChatMessage();
        var req1 = new PinOrUnpinRequest(1, message, false);
        var req2 = new PinOrUnpinRequest(2, message, false);

        Assert.NotEqual(req1, req2);
        Assert.False(req1 == req2);
        Assert.True(req1 != req2);
    }

    [Fact]
    public void Deconstruct_Works()
    {
        var message = new ChatMessage();
        var request = new PinOrUnpinRequest(5, message, true);

        var (roomId, msg, pin) = request;

        Assert.Equal(5, roomId);
        Assert.Equal(message, msg);
        Assert.True(pin);
    }
}
