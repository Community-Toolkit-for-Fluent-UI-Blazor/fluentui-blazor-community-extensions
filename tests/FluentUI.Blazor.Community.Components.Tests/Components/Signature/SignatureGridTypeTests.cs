using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Signature;
public class SignatureGridTypeTests
{
    [Fact]
    public void SignatureGridType_Should_Have_Expected_Values()
    {
        Assert.Equal(0, (int)SignatureGridType.None);
        Assert.Equal(1, (int)SignatureGridType.Lines);
        Assert.Equal(2, (int)SignatureGridType.Dots);
    }

    [Theory]
    [InlineData(SignatureGridType.None, "None")]
    [InlineData(SignatureGridType.Lines, "Lines")]
    [InlineData(SignatureGridType.Dots, "Dots")]
    public void SignatureGridType_ToString_Should_Return_Expected(SignatureGridType value, string expected)
    {
        Assert.Equal(expected, value.ToString());
    }

    [Theory]
    [InlineData("None", SignatureGridType.None)]
    [InlineData("Lines", SignatureGridType.Lines)]
    [InlineData("Dots", SignatureGridType.Dots)]
    public void SignatureGridType_Parse_Should_Return_Expected(string input, SignatureGridType expected)
    {
        Assert.Equal(expected, Enum.Parse<SignatureGridType>(input));
    }
}
