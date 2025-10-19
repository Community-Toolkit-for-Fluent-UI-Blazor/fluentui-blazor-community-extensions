using Microsoft.Extensions.DependencyInjection;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Tests;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Login;

public class LoginOverlayTests : TestBase
{
    public LoginOverlayTests()
    {
        JSInterop.Mode = Bunit.JSRuntimeMode.Loose;
        Services.AddSingleton(UnitTestLibraryConfiguration);
        Services.AddFluentUIComponents();
    }

    [Fact]
    public void LoginOverlay_DefaultInstance_Renders()
    {
        // Act
        var cut = RenderComponent<LoginOverlay>();

        // Assert
        Assert.NotNull(cut.Instance);
    }

    [Fact]
    public void LoginOverlay_Renders_ChildContent()
    {
        // Act
        var cut = RenderComponent<LoginOverlay>(parameters => parameters
            .AddChildContent("<div id=\"child\">contenu</div>")
        );

        // Assert - vérifie que le ChildContent est bien rendu
        Assert.Contains("id=\"child\"", cut.Markup);
        Assert.Contains("contenu", cut.Markup);
    }
}
