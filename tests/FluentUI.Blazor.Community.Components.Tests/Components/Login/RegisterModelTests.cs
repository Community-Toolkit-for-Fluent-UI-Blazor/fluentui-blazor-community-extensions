using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Login;

public class RegisterModelTests
{
    [Fact]
    public void DefaultValues_Are_Null_And_IsValid_False()
    {
        var model = new RegisterModel();

        Assert.Null(model.DisplayName);
        Assert.Null(model.Email);
        Assert.Null(model.Password);
        Assert.Null(model.ConfirmPassword);
        Assert.False(model.IsValid());
    }

    [Fact]
    public void IsValid_Returns_True_For_Valid_Model()
    {
        var model = new RegisterModel
        {
            DisplayName = "User",
            Email = "user@example.com",
            Password = "P@ssw0rd",
            ConfirmPassword = "P@ssw0rd"
        };

        Assert.True(model.IsValid());
    }

    [Fact]
    public void IsValid_Returns_False_When_DisplayName_Missing()
    {
        var model = new RegisterModel
        {
            DisplayName = null,
            Email = "user@example.com",
            Password = "P@ssw0rd",
            ConfirmPassword = "P@ssw0rd"
        };

        Assert.False(model.IsValid());
    }

    [Fact]
    public void IsValid_Returns_False_When_Email_Missing()
    {
        var model = new RegisterModel
        {
            DisplayName = "User",
            Email = null,
            Password = "P@ssw0rd",
            ConfirmPassword = "P@ssw0rd"
        };

        Assert.False(model.IsValid());
    }

    [Fact]
    public void IsValid_Returns_False_When_Password_Missing()
    {
        var model = new RegisterModel
        {
            DisplayName = "User",
            Email = "user@example.com",
            Password = null,
            ConfirmPassword = null
        };

        Assert.False(model.IsValid());
    }

    [Fact]
    public void IsValid_Returns_False_When_ConfirmPassword_Missing()
    {
        var model = new RegisterModel
        {
            DisplayName = "User",
            Email = "user@example.com",
            Password = "P@ssw0rd",
            ConfirmPassword = null
        };

        Assert.False(model.IsValid());
    }

    [Fact]
    public void IsValid_Returns_False_When_Passwords_Do_Not_Match()
    {
        var model = new RegisterModel
        {
            DisplayName = "User",
            Email = "user@example.com",
            Password = "P@ssw0rd",
            ConfirmPassword = "Different"
        };

        Assert.False(model.IsValid());
    }

    [Fact]
    public void IsValid_Is_CaseSensitive_For_Passwords()
    {
        var model = new RegisterModel
        {
            DisplayName = "User",
            Email = "user@example.com",
            Password = "Password",
            ConfirmPassword = "password" // different casing
        };

        Assert.False(model.IsValid());
    }

    [Fact]
    public void IsValid_Returns_False_When_Fields_Are_Whitespace()
    {
        var model = new RegisterModel
        {
            DisplayName = "   ",
            Email = "   ",
            Password = "   ",
            ConfirmPassword = "   "
        };

        Assert.False(model.IsValid());
    }

    [Fact]
    public void All_Properties_Have_RequiredAttribute()
    {
        var type = typeof(RegisterModel);

        var displayNameProp = type.GetProperty(nameof(RegisterModel.DisplayName));
        var emailProp = type.GetProperty(nameof(RegisterModel.Email));
        var passwordProp = type.GetProperty(nameof(RegisterModel.Password));
        var confirmProp = type.GetProperty(nameof(RegisterModel.ConfirmPassword));

        Assert.NotNull(displayNameProp);
        Assert.NotNull(emailProp);
        Assert.NotNull(passwordProp);
        Assert.NotNull(confirmProp);

        Assert.True(displayNameProp.GetCustomAttributes(typeof(RequiredAttribute), true).Any());
        Assert.True(emailProp.GetCustomAttributes(typeof(RequiredAttribute), true).Any());
        Assert.True(passwordProp.GetCustomAttributes(typeof(RequiredAttribute), true).Any());
        Assert.True(confirmProp.GetCustomAttributes(typeof(RequiredAttribute), true).Any());
    }
}
