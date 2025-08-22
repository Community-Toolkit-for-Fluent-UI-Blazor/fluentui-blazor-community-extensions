using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Chat;

public class ChatRoomContentTests
{
    [Fact]
    public void Constructor_ShouldSetProperties()
    {
        // Arrange & Act
        var content = new ChatRoomContent
        {
            Label = "Name of the room",
            Placeholder = "Enter the name",
            Name = "General"
        };

        // Assert
        Assert.Equal("Name of the room", content.Label);
        Assert.Equal("Enter the name", content.Placeholder);
        Assert.Equal("General", content.Name);
    }

    [Fact]
    public void Name_ShouldBeRequired()
    {
        // Arrange
        var content = new ChatRoomContent { Name = null };

        // Act
        var results = ValidateModel(content);

        // Assert
        Assert.Contains(results, r => r.MemberNames.Contains(nameof(ChatRoomContent.Name)) && r.ErrorMessage.Contains("required", StringComparison.OrdinalIgnoreCase));
    }

    [Theory]
    [InlineData("a")]
    [InlineData("ab")]
    public void Name_ShouldHaveMinLength3(string? invalidName)
    {
        // Arrange
        var content = new ChatRoomContent { Name = invalidName };

        // Act
        var results = ValidateModel(content);

        // Assert
        Assert.Contains(results, r => r.MemberNames.Contains(nameof(ChatRoomContent.Name)) && r.ErrorMessage.Contains("minimum", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void Name_ValidValue_ShouldPassValidation()
    {
        // Arrange
        var content = new ChatRoomContent { Name = "ValidName" };

        // Act
        var results = ValidateModel(content);

        // Assert
        Assert.Empty(results);
    }

    private static List<ValidationResult> ValidateModel(object model)
    {
        var context = new ValidationContext(model, null, null);
        var results = new List<ValidationResult>();
        Validator.TryValidateObject(model, context, results, true);
        return results;
    }
}
