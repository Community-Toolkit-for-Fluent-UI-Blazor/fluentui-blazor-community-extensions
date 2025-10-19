using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Login;

public class ExternalProviderRegisterEventArgsTests
{
    [Fact]
    public void Ctor_Sets_Email_Property()
    {
        // Arrange
        var email = "user@example.com";

        // Act
        var args = new ExternalProviderRegisterEventArgs(email);

        // Assert
        Assert.Equal(email, args.Email);
    }

    [Fact]
    public void Errors_Is_Empty_List_By_Default_And_IsSuccessful_True_When_NoConfirmationRequired()
    {
        // Arrange & Act
        var args = new ExternalProviderRegisterEventArgs("a@b.c");

        // Assert
        Assert.NotNull(args.Errors);
        Assert.Empty(args.Errors);
        Assert.False(args.RequireConfirmedAccount);
        Assert.True(args.IsSuccessful);
    }

    [Fact]
    public void IsSuccessful_False_When_RequireConfirmedAccount_True()
    {
        // Arrange
        var args = new ExternalProviderRegisterEventArgs("a@b.c")
        {
            RequireConfirmedAccount = true
        };

        // Act & Assert
        Assert.False(args.IsSuccessful);
    }

    [Fact]
    public void IsSuccessful_False_When_Errors_Not_Empty()
    {
        // Arrange
        var args = new ExternalProviderRegisterEventArgs("a@b.c");
        args.Errors.Add("Some error");

        // Act & Assert
        Assert.False(args.IsSuccessful);
        Assert.Single(args.Errors);
        Assert.Contains("Some error", args.Errors);
    }

    [Fact]
    public void RequireConfirmedAccount_Can_Be_Toggled_And_Affects_IsSuccessful()
    {
        // Arrange
        var args = new ExternalProviderRegisterEventArgs("a@b.c");

        // Initially successful
        Assert.True(args.IsSuccessful);

        // Require confirmation -> not successful
        args.RequireConfirmedAccount = true;
        Assert.False(args.IsSuccessful);

        // Remove requirement -> successful again (errors still empty)
        args.RequireConfirmedAccount = false;
        Assert.True(args.IsSuccessful);
    }

    [Fact]
    public void Errors_List_Is_Mutable_But_Property_Is_ReadOnly()
    {
        // Arrange
        var args = new ExternalProviderRegisterEventArgs("a@b.c");

        // Act
        args.Errors.Add("E1");
        args.Errors.Add("E2");

        // Assert
        Assert.Equal(2, args.Errors.Count);
        Assert.Equal(new[] { "E1", "E2" }, args.Errors.ToArray());
    }
}
