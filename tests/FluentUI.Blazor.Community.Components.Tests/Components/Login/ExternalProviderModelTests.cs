using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Login;

public class ExternalProviderModelTests
{
    [Fact]
    public void Email_Default_Is_Null_And_CanBe_Set()
    {
        var model = new ExternalProviderModel();
        Assert.Null(model.Email);

        model.Email = "external@example.com";
        Assert.Equal("external@example.com", model.Email);
    }

    [Fact]
    public void Email_Has_RequiredAttribute()
    {
        var prop = typeof(ExternalProviderModel).GetProperty(nameof(ExternalProviderModel.Email),
            BindingFlags.Public | BindingFlags.Instance);
        Assert.NotNull(prop);
        Assert.True(prop.GetCustomAttributes(typeof(RequiredAttribute), true).Any());
    }
}
