using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bunit;
using Microsoft.FluentUI.AspNetCore.Components.Tests;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Slideshow;

public class SlideshowItemTests
    : TestBase
{
    [Fact]
    public void SlideshowItem_Renders_Div_With_Expected_Attributes()
    {
        // Arrange & Act
        var cut = RenderComponent<SlideshowItem<object>>(parameters => parameters
            .AddChildContent("<span>Contenu test</span>")
            .Add(p => p.Id, "test-id")
            .Add(p => p.AriaLabel, "label")
            .Add(p => p.Style, "color:red;")
        );

        // Assert
        cut.MarkupMatches(@"
<div aria-label=""label"" id=""test-id"" class=""slideshow-item "" style=""color:red;"" role=""listitem"">
    <span>Contenu test</span>
</div>
");
    }
}
