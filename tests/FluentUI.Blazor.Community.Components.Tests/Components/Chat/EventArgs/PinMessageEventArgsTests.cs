using Moq;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Chat;

public class PinMessageEventArgsTests
{
    [Fact]
    public void Constructor_SetsPropertiesCorrectly()
    {
        // Arrange
        var messageMock = new Mock<IChatMessage>();
        var isPinned = true;

        // Act
        var args = new PinMessageEventArgs(messageMock.Object, isPinned);

        // Assert
        Assert.Equal(messageMock.Object, args.Message);
        Assert.True(args.Pin);
    }

    [Fact]
    public void MessageId_Getter_ReturnsExpectedValue()
    {
        // Arrange
        var expected = new Mock<IChatMessage>();
        var args = new PinMessageEventArgs(expected.Object, false);

        // Act & Assert
        Assert.Equal(expected.Object, args.Message);
    }

    [Fact]
    public void IsPinned_Getter_ReturnsExpectedValue()
    {
        // Arrange
        var expected = new Mock<IChatMessage>();
        var args = new PinMessageEventArgs(expected.Object, true);

        // Act & Assert
        Assert.True(args.Pin);
    }
}
