using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Login;

public class ForgotPasswordModelTests
{
    [Fact]
    public void Default_Email_Is_Null()
    {
        var model = new ForgotPasswordModel();
        Assert.Null(model.Email);
    }

    [Fact]
    public void Email_Property_CanBe_Set_And_Get()
    {
        var model = new ForgotPasswordModel();
        model.Email = "user@example.com";
        Assert.Equal("user@example.com", model.Email);
    }

    [Fact]
    public void Email_Has_RequiredAttribute()
    {
        var prop = typeof(ForgotPasswordModel).GetProperty(nameof(ForgotPasswordModel.Email),
            BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
        Assert.NotNull(prop);
        Assert.True(prop.GetCustomAttributes(typeof(RequiredAttribute), true).Any());
    }
}
