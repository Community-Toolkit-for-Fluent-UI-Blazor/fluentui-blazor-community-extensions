using Moq;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Chat;

public class ChatMessageReactEventArgsTests
{
    [Fact]
    public void Constructor_SetsPropertiesCorrectly()
    {
        // Arrange
        var messageMock = new Mock<IChatMessage>();
        var reaction = "👍";

        // Act
        var args = new ChatMessageReactEventArgs(messageMock.Object, reaction);

        // Assert
        Assert.Equal(messageMock.Object, args.Message);
        Assert.Equal(reaction, args.Reaction);
    }

    [Fact]
    public void Equality_TwoInstancesWithSameValues_AreEqual()
    {
        // Arrange
        var messageMock = new Mock<IChatMessage>();
        var reaction = "😀";

        var args1 = new ChatMessageReactEventArgs(messageMock.Object, reaction);
        var args2 = new ChatMessageReactEventArgs(messageMock.Object, reaction);

        // Act & Assert
        Assert.Equal(args1, args2);
        Assert.True(args1 == args2);
        Assert.False(args1 != args2);
    }

    [Fact]
    public void Equality_TwoInstancesWithDifferentValues_AreNotEqual()
    {
        // Arrange
        var messageMock1 = new Mock<IChatMessage>();
        var messageMock2 = new Mock<IChatMessage>();
        var reaction1 = "😀";
        var reaction2 = "😢";

        var args1 = new ChatMessageReactEventArgs(messageMock1.Object, reaction1);
        var args2 = new ChatMessageReactEventArgs(messageMock2.Object, reaction2);

        // Act & Assert
        Assert.NotEqual(args1, args2);
        Assert.True(args1 != args2);
    }

    [Fact]
    public void CanDeconstructRecord()
    {
        // Arrange
        var messageMock = new Mock<IChatMessage>();
        var reaction = "🔥";
        var args = new ChatMessageReactEventArgs(messageMock.Object, reaction);

        // Act
        var (message, reac) = args;

        // Assert
        Assert.Equal(messageMock.Object, message);
        Assert.Equal(reaction, reac);
    }
}
