using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Chat;

public class ChatRoomListExceptionTests
{
    [Fact]
    public void Ctor_Default_SetsDefaultProperties()
    {
        // Act
        var ex = new ChatRoomListException();

        // Assert
        Assert.NotNull(ex.Message);
        Assert.Null(ex.InnerException);
    }

    [Fact]
    public void Ctor_WithMessage_SetsMessage()
    {
        // Arrange
        var message = "Erreur de chat room";

        // Act
        var ex = new ChatRoomListException(message);

        // Assert
        Assert.Equal(message, ex.Message);
        Assert.Null(ex.InnerException);
    }

    [Fact]
    public void Ctor_WithMessageAndInnerException_SetsProperties()
    {
        // Arrange
        var message = "Erreur avec inner";
        var inner = new InvalidOperationException("Inner");

        // Act
        var ex = new ChatRoomListException(message, inner);

        // Assert
        Assert.Equal(message, ex.Message);
        Assert.Equal(inner, ex.InnerException);
    }
}
