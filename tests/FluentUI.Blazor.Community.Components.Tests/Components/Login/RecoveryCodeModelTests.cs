using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Login;

public class RecoveryCodeModelTests
{
    [Fact]
    public void Property_Default_Is_Null_And_Can_Set()
    {
        var model = new RecoveryCodeModel();
        Assert.Null(model.RecoveryCode);

        model.RecoveryCode = "RC-123";
        Assert.Equal("RC-123", model.RecoveryCode);
    }

    [Fact]
    public void RecoveryCode_Has_Required_And_DataType_Text_Attributes()
    {
        var prop = typeof(RecoveryCodeModel).GetProperty(nameof(RecoveryCodeModel.RecoveryCode),
            BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);

        Assert.NotNull(prop);
        Assert.True(prop.GetCustomAttributes(typeof(RequiredAttribute), true).Any());
        var dt = prop.GetCustomAttributes(typeof(DataTypeAttribute), true).Cast<DataTypeAttribute>().FirstOrDefault();
        Assert.NotNull(dt);
        Assert.Equal(DataType.Text, dt.DataType);
    }
}
