namespace FluentUI.Blazor.Community.Components.Tests.Components;

public class ChatMessageSectionTests
{
    [Fact]
    public void Can_Set_And_Get_Properties()
    {
        // Arrange
        var section = new ChatMessageSection
        {
            Id = 1,
            MessageId = 2,
            CultureId = 3,
            Content = "Test content",
            CreatedDate = new DateTime(2024, 1, 1)
        };

        // Assert
        Assert.Equal(1, section.Id);
        Assert.Equal(2, section.MessageId);
        Assert.Equal(3, section.CultureId);
        Assert.Equal("Test content", section.Content);
        Assert.Equal(new DateTime(2024, 1, 1), section.CreatedDate);
    }

    [Fact]
    public void Record_Equality_Works()
    {
        // Arrange
        var section1 = new ChatMessageSection
        {
            Id = 1,
            MessageId = 2,
            CultureId = 3,
            Content = "Test",
            CreatedDate = DateTime.UtcNow
        };

        var section2 = new ChatMessageSection
        {
            Id = 1,
            MessageId = 2,
            CultureId = 3,
            Content = "Test",
            CreatedDate = section1.CreatedDate
        };

        // Assert
        Assert.Equal(section1, section2);
        Assert.True(section1 == section2);
    }

    [Fact]
    public void With_Expression_Creates_Copy()
    {
        // Arrange
        var original = new ChatMessageSection
        {
            Id = 1,
            MessageId = 2,
            CultureId = 3,
            Content = "Original",
            CreatedDate = DateTime.UtcNow
        };

        // Act
        var copy = original with { Content = "Copy" };

        // Assert
        Assert.Equal(original.Id, copy.Id);
        Assert.Equal(original.MessageId, copy.MessageId);
        Assert.Equal(original.CultureId, copy.CultureId);
        Assert.Equal("Copy", copy.Content);
        Assert.Equal(original.CreatedDate, copy.CreatedDate);
        Assert.NotEqual(original, copy);
    }
}
