using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Signature;
public class SignatureToolPositionTests
{
    [Fact]
    public void Enum_Should_Have_All_Expected_Values()
    {
        Assert.Equal(0, (int)SignatureToolPosition.Top);
        Assert.Equal(1, (int)SignatureToolPosition.Bottom);
        Assert.Equal(2, (int)SignatureToolPosition.Left);
        Assert.Equal(3, (int)SignatureToolPosition.Right);
    }

    [Theory]
    [InlineData(SignatureToolPosition.Top, "Top")]
    [InlineData(SignatureToolPosition.Bottom, "Bottom")]
    [InlineData(SignatureToolPosition.Left, "Left")]
    [InlineData(SignatureToolPosition.Right, "Right")]
    public void Enum_ToString_Should_Return_Correct_Name(SignatureToolPosition position, string expected)
    {
        Assert.Equal(expected, position.ToString());
    }

    [Theory]
    [InlineData("Top", SignatureToolPosition.Top)]
    [InlineData("Bottom", SignatureToolPosition.Bottom)]
    [InlineData("Left", SignatureToolPosition.Left)]
    [InlineData("Right", SignatureToolPosition.Right)]
    public void Parse_Should_Return_Correct_Enum(string value, SignatureToolPosition expected)
    {
        var result = Enum.Parse<SignatureToolPosition>(value);
        Assert.Equal(expected, result);
    }
}
