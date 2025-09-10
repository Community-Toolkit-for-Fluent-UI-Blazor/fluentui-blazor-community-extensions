using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Signature;
public class SignatureLineStyleTests
{
    [Fact]
    public void SignatureLineStyle_Should_Have_Expected_Values()
    {
        Assert.Equal(0, (int)SignatureLineStyle.Solid);
        Assert.Equal(1, (int)SignatureLineStyle.Dashed);
        Assert.Equal(2, (int)SignatureLineStyle.Dotted);
    }

    [Theory]
    [InlineData(SignatureLineStyle.Solid)]
    [InlineData(SignatureLineStyle.Dashed)]
    [InlineData(SignatureLineStyle.Dotted)]
    public void SignatureLineStyle_Should_Be_Defined(SignatureLineStyle style)
    {
        Assert.True(Enum.IsDefined(typeof(SignatureLineStyle), style));
    }

    [Fact]
    public void SignatureLineStyle_Should_Have_Three_Values()
    {
        var values = Enum.GetValues(typeof(SignatureLineStyle));
        Assert.Equal(3, values.Length);
    }
}
