using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Signature;
public class SignatureToolTests
{
    [Fact]
    public void SignatureTool_Should_Have_Expected_Enum_Values()
    {
        Assert.Equal(0, (int)SignatureTool.Pen);
        Assert.Equal(1, (int)SignatureTool.Eraser);
    }

    [Fact]
    public void SignatureTool_Should_Contain_All_Enum_Members()
    {
        var values = Enum.GetValues(typeof(SignatureTool)).Cast<SignatureTool>().ToArray();
        Assert.Contains(SignatureTool.Pen, values);
        Assert.Contains(SignatureTool.Eraser, values);
        Assert.Equal(2, values.Length);
    }

    [Theory]
    [InlineData(SignatureTool.Pen, "Pen")]
    [InlineData(SignatureTool.Eraser, "Eraser")]
    public void SignatureTool_ToString_Should_Return_Correct_Name(SignatureTool tool, string expected)
    {
        Assert.Equal(expected, tool.ToString());
    }
}
