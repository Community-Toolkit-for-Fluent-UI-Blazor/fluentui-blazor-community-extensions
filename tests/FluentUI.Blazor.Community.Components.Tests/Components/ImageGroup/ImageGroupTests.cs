using Bunit;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Components.ImageGroup;

public class ImageGroupTests : TestBase
{
    public ImageGroupTests()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
        Services.AddSingleton(UnitTestLibraryConfiguration);
    }

    [Fact]
    public void FluentCxImage_Default()
    {

    }
}
