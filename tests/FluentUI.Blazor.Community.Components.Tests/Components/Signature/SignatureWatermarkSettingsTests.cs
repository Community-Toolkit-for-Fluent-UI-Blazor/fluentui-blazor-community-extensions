using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Tests;
using Moq;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Signature;

public class SignatureWatermarkSettingsTests : TestBase
{

    [Fact]
    public void OnInitialized_Throws_When_Parent_Is_Null()
    {
        var ex = Assert.Throws<InvalidOperationException>(() => RenderComponent<SignatureWatermarkSettings>());
        Assert.Contains("SignatureWatermarkSettings must be used within a FluentCxSignature component", ex.Message);
    }
}
