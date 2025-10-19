using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Login;

public class ResetPasswordEventArgsTests
{
    [Fact]
    public void Ctor_Sets_All_Properties()
    {
        // Arrange
        var email = "user@example.com";
        var code = "code123";
        var password = "P@ssw0rd";
        var confirm = "P@ssw0rd";

        // Act
        var args = new ResetPasswordEventArgs(email, code, password, confirm);

        // Assert
        Assert.Equal(email, args.Email);
        Assert.Equal(code, args.Code);
        Assert.Equal(password, args.Password);
        Assert.Equal(confirm, args.ConfirmPassword);
    }

    [Fact]
    public void Errors_Initially_Empty_And_IsSuccessful_True()
    {
        // Act
        var args = new ResetPasswordEventArgs("a@b.c", "c", "p", "p");

        // Assert
        Assert.NotNull(args.Errors);
        Assert.Empty(args.Errors);
        Assert.True(args.IsSuccessful);
    }

    [Fact]
    public void IsSuccessful_False_When_Errors_Populated()
    {
        // Arrange
        var args = new ResetPasswordEventArgs("a@b.c", "c", "p", "p");

        // Act
        args.Errors.Add("Invalid code");
        args.Errors.Add("Password too weak");

        // Assert
        Assert.False(args.IsSuccessful);
        Assert.Equal(2, args.Errors.Count);
        Assert.Contains("Invalid code", args.Errors);
        Assert.Contains("Password too weak", args.Errors);
    }

    [Fact]
    public void IsSuccessful_False_When_Errors_Contains_EmptyString()
    {
        // Arrange
        var args = new ResetPasswordEventArgs("a@b.c", "c", "p", "p");

        // Act
        args.Errors.Add(string.Empty);

        // Assert
        Assert.False(args.IsSuccessful);
        Assert.Single(args.Errors);
    }

    [Fact]
    public void Errors_Is_List_Instance_CanBe_Enumerated()
    {
        // Arrange
        var args = new ResetPasswordEventArgs("a@b.c", "c", "p", "p");
        var expected = new List<string> { "E1", "E2" };

        // Act
        foreach (var e in expected)
        {
            args.Errors.Add(e);
        }

        // Assert
        Assert.Equal(expected, args.Errors);
    }
}
