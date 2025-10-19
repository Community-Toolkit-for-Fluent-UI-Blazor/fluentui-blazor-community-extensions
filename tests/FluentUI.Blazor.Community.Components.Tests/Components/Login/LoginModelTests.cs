using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Login;

public class LoginModelTests
{
    [Fact]
    public void Default_Values_Are_Null_And_RememberMe_False()
    {
        var model = new LoginModel();
        Assert.Null(model.Email);
        Assert.Null(model.Password);
        Assert.False(model.RememberMe);
    }

    [Fact]
    public void Properties_Setters_And_Getters_Work()
    {
        var model = new LoginModel
        {
            Email = "user",
            Password = "pwd",
            RememberMe = true
        };

        Assert.Equal("user", model.Email);
        Assert.Equal("pwd", model.Password);
        Assert.True(model.RememberMe);
    }

    [Fact]
    public void UserName_And_Password_Have_Required_Attribute()
    {
        var type = typeof(LoginModel);
        var userProp = type.GetProperty(nameof(LoginModel.Email));
        var pwdProp = type.GetProperty(nameof(LoginModel.Password));

        Assert.NotNull(userProp);
        Assert.NotNull(pwdProp);

        Assert.True(userProp.GetCustomAttributes(typeof(RequiredAttribute), true).Any());
        Assert.True(pwdProp.GetCustomAttributes(typeof(RequiredAttribute), true).Any());
    }
}
