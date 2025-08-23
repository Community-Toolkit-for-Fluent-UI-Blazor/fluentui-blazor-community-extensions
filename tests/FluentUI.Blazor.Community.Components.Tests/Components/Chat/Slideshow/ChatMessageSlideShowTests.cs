using Moq;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Chat.Slideshow;

public class ChatMessageSlideShowTests
{
    [Fact]
    public void Constructor_ShouldInitializeId()
    {
        // Act
        var slideShow = new ChatMessageSlideShow();

        // Assert
        Assert.False(string.IsNullOrEmpty(slideShow.Id));
    }

    [Fact]
    public void Owner_GetSet_Works()
    {
        var slideShow = new ChatMessageSlideShow();
        var user = new ChatUser { Id = 1, DisplayName = "Test" };

        slideShow.Owner = user;

        Assert.Equal(user, slideShow.Owner);
    }

    [Fact]
    public void Message_GetSet_Works()
    {
        var slideShow = new ChatMessageSlideShow();
        var message = new Mock<IChatMessage>().Object;

        slideShow.Message = message;

        Assert.Equal(message, slideShow.Message);
    }

    [Fact]
    public void LoadingLabel_GetSet_Works()
    {
        var slideShow = new ChatMessageSlideShow();
        slideShow.LoadingLabel = "Loading...";

        Assert.Equal("Loading...", slideShow.LoadingLabel);
    }

    [Theory]
    [InlineData(1, 1, true)]
    [InlineData(1, 0, false)]
    [InlineData(0, 2, true)]
    [InlineData(0, 1, false)]
    [InlineData(0, 0, false)]
    public void ShowControlsAndIndicators_ReturnsExpected(int sectionCount, int fileCount, bool expected)
    {
        // Arrange
        var messageMock = new Mock<IChatMessage>();
        messageMock.Setup(m => m.Sections).Returns(new List<IChatMessageSection>(new IChatMessageSection[sectionCount]));
        messageMock.Setup(m => m.Files).Returns(new List<IChatFile>(new IChatFile[fileCount]));

        var slideShow = new ChatMessageSlideShow
        {
            Message = messageMock.Object
        };

        // Utilisation de la réflexion pour accéder à la propriété privée
        var prop = typeof(ChatMessageSlideShow).GetProperty("ShowControlsAndIndicators", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var result = (bool)prop.GetValue(slideShow);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void ShowControlsAndIndicators_ReturnsFalse_WhenMessageIsNull()
    {
        var slideShow = new ChatMessageSlideShow { Message = null };

        var prop = typeof(ChatMessageSlideShow).GetProperty("ShowControlsAndIndicators", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var result = (bool)prop.GetValue(slideShow);

        Assert.False(result);
    }
}
