using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Tests;
using Moq;

namespace FluentUI.Blazor.Community.Components.Tests.Components.ImageGroup;

public class ImageGroupItemTests : TestBase
{
    public ImageGroupItemTests()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
        Services.AddSingleton(UnitTestLibraryConfiguration);
        Services.AddFluentUIComponents();
    }

    [Fact]
    public async Task DisposeAsync_CallsParentRemove_AndSuppressFinalize()
    {
        var cut = RenderComponent<FluentCxImageGroup>(parameters =>
        {
            parameters.AddChildContent(builder =>
            {
                builder.OpenComponent<FluentCxImageGroupItem>(0);
                builder.CloseComponent();
            });
        });

        var list = typeof(FluentCxImageGroup).GetField("_children", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .GetValue(cut.Instance) as List<FluentCxImageGroupItem>;

        Assert.NotNull(list);
        Assert.Single(list);

        var item = cut.FindComponent<FluentCxImageGroupItem>().Instance;
        await item.DisposeAsync();

        list = typeof(FluentCxImageGroup).GetField("_children", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .GetValue(cut.Instance) as List<FluentCxImageGroupItem>;

        Assert.NotNull(list);
        Assert.Empty(list);
    }

    [Fact]
    public void OnInitialized_ThrowsIfParentNull()
    {
        Assert.Throws<InvalidOperationException>(()=>
        {
            RenderComponent<FluentCxSleekDial>(parameters =>
            {
                parameters.AddChildContent(builder =>
                {
                    builder.OpenComponent<FluentCxImageGroupItem>(0);
                    builder.CloseComponent();
                });
            });
        });
    }

    [Fact]
    public void Check_SetParametersAsync_Value_Changed()
    {
        var comp = RenderComponent<FluentCxImageGroup>(parameters =>
        {
            parameters.AddChildContent(builder =>
            {
                builder.OpenComponent<FluentCxImageGroupItem>(0);
                builder.AddComponentParameter(1, "Source", "img.png");
                builder.AddComponentParameter(2, "Alt", "alt");
                builder.AddComponentParameter(3, "Class", "my-class");
                builder.AddComponentParameter(4, "Title", "title");
                builder.CloseComponent();
            });
        });

        var item = comp.FindComponent<FluentCxImageGroupItem>();
        Assert.NotNull(item);

        var hasChanged = (bool)typeof(FluentCxImageGroupItem)
            .GetField("_hasParameterChanged", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!
            .GetValue(item.Instance)!;

        Assert.True(hasChanged);
    }
}
