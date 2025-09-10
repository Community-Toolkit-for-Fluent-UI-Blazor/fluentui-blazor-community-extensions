using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Signature;
public class SignatureCapabilitiesTests
{
    [Fact]
    public void DefaultValues_ShouldBeTrue()
    {
        var capabilities = new SignatureCapabilities();

        Assert.True(capabilities.CanUndo);
        Assert.True(capabilities.CanRedo);
        Assert.True(capabilities.CanClear);
        Assert.True(capabilities.CanExport);
        Assert.True(capabilities.CanErase);
    }

    [Fact]
    public void CanSetProperties()
    {
        var capabilities = new SignatureCapabilities
        {
            CanUndo = false,
            CanRedo = false,
            CanClear = false,
            CanExport = false,
            CanErase = false
        };

        Assert.False(capabilities.CanUndo);
        Assert.False(capabilities.CanRedo);
        Assert.False(capabilities.CanClear);
        Assert.False(capabilities.CanExport);
        Assert.False(capabilities.CanErase);
    }
}
