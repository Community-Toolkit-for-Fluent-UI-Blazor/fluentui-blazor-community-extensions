using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Login;

public class SendPasswordEventArgsTests
{
    [Fact]
    public void Ctor_Sets_Email_Property()
    {
        // Arrange
        var email = "user@example.com";

        // Act
        var args = new SendPasswordEventArgs(email);

        // Assert
        Assert.Equal(email, args.Email);
    }

    [Fact]
    public void Default_FailReason_Is_None_And_Successful_Is_True()
    {
        // Arrange
        var args = new SendPasswordEventArgs("user@example.com");

        // Assert
        Assert.Equal(SendPasswordFailReason.None, args.FailReason);
        Assert.True(args.Successful);
    }

    [Fact]
    public void Successful_Is_False_When_FailReason_Is_EmailNotFound()
    {
        // Arrange
        var args = new SendPasswordEventArgs("user@example.com")
        {
            FailReason = SendPasswordFailReason.EmailNotFound
        };

        // Assert
        Assert.Equal(SendPasswordFailReason.EmailNotFound, args.FailReason);
        Assert.False(args.Successful);
    }

    [Fact]
    public void Successful_Is_False_When_FailReason_Is_NoServerResponse()
    {
        // Arrange
        var args = new SendPasswordEventArgs("user@example.com")
        {
            FailReason = SendPasswordFailReason.NoServerResponse
        };

        // Assert
        Assert.Equal(SendPasswordFailReason.NoServerResponse, args.FailReason);
        Assert.False(args.Successful);
    }

    [Fact]
    public void FailReason_Setter_Allows_Changing_Reason()
    {
        // Arrange
        var args = new SendPasswordEventArgs("user@example.com");

        // Act
        args.FailReason = SendPasswordFailReason.EmailNotFound;
        args.FailReason = SendPasswordFailReason.NoServerResponse;

        // Assert
        Assert.Equal(SendPasswordFailReason.NoServerResponse, args.FailReason);
    }
}
