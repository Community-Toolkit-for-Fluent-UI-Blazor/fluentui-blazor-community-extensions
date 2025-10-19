using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Login;

public class RecoveryCodeEventArgsTests
{
    [Fact]
    public void Ctor_Sets_RecoveryCode()
    {
        var code = "RC-XYZ";
        var args = new RecoveryCodeEventArgs(code);
        Assert.Equal(code, args.RecoveryCode);
    }

    [Fact]
    public void Default_FailReason_None_And_IsSuccessful_True()
    {
        var args = new RecoveryCodeEventArgs("x");
        Assert.Equal(RecoveryFailReason.None, args.FailReason);
        Assert.True(args.IsSuccessful);
    }

    [Fact]
    public void IsSuccessful_False_When_FailReason_Set()
    {
        var args = new RecoveryCodeEventArgs("x");
        var alt = Enum.GetValues<RecoveryFailReason>().FirstOrDefault(v => v != RecoveryFailReason.None);
        args.FailReason = alt;
        Assert.Equal(alt, args.FailReason);
        Assert.False(args.IsSuccessful);
    }
}
