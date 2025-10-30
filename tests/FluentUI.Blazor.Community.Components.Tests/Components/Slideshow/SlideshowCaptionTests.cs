using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.FluentUI.AspNetCore.Components.Tests;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Slideshow;

public class SlideshowDescriptionTests : TestBase
{
    [Fact]
    public void RendersChildContent()
    {
        // Arrange
        var childContent = "<span>Test Caption</span>";

        // Act
        var cut = RenderComponent<SlideshowCaption>(parameters => parameters
            .AddChildContent(childContent)
        );

        // Assert
        cut.Markup.Contains($@"
                <div role=""presentation"">
                    {childContent}
                </div>
            ");
    }
}
