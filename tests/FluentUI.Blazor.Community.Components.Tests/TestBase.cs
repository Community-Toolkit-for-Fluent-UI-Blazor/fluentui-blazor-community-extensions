using Bunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests;

public class TestBase : TestContext
{

    protected LibraryConfiguration UnitTestLibraryConfiguration { get; } = new LibraryConfiguration
    {
        CollocatedJavaScriptQueryString = null,
    };
}
