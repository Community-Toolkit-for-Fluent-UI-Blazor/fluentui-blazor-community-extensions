using Bunit;
using FluentUI.Blazor.Community.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Tests;

namespace FluentUI.Blazor.Community.Components.Tests.Components;

public class PathBarTests : TestBase
{
    public PathBarTests()
    {
        JSInterop.Mode = Bunit.JSRuntimeMode.Loose;
        Services.AddSingleton(UnitTestLibraryConfiguration);
        Services.AddScoped<FileManagerState>();
        Services.AddScoped<DeviceInfoState>();
        Services.AddFluentUIComponents();
    }

    private IRenderedFragment RenderPathBar(Action<ComponentParameterCollectionBuilder<FluentCxPathBar>>? configureParameters = null)
    {
        return Render(b =>
        {
            b.OpenComponent<FluentMenuProvider>(0);
            b.CloseComponent();

            b.OpenComponent<FluentCxPathBar>(1);
            if (configureParameters != null)
            {
                var i = 2;
                var parameters = new ComponentParameterCollectionBuilder<FluentCxPathBar>();
                configureParameters(parameters);
                foreach (var param in parameters.Build())
                {
                    b.AddAttribute(i, param.Name, param.Value);
                    i++;
                }
            }

            b.CloseComponent();
        });
    }

    [Fact]
    public void FluentCxPathBar_Default()
    {
        var cut = RenderPathBar();

        Assert.Contains("stack-horizontal", cut.Markup);
        Assert.DoesNotContain("fluent-anchor", cut.Markup);
    }

    [Theory]
    [InlineData("Home\\FluentUI\\FluentCxBar")]
    [InlineData("Home\\Benchmarks\\FluentUI\\Extensions\\")]
    [InlineData("Home\\Images\\Travels\\2025\\Paris")]
    [InlineData("Home")]
    public void FluentCxPathBar_With_Root_And_Path(string value)
    {
        var segments = PathHelper.GetSegments(value);
        var root = Build(segments);
        var cut = RenderPathBar(a =>
        {
            a.Add(p => p.Root, root);
            a.Add(p => p.Path, value);
        });

        cut.InvokeAsync(() => { });

        Assert.Contains("stack-horizontal", cut.Markup);
        Assert.Contains("fluent-anchor", cut.Markup);
        Assert.Equal(segments.Length, cut.FindAll("fluent-anchor").Count);

        IPathBarItem Build(string[] segments)
        {
            PathBarItem item = new()
            {
                Label = segments[0],
                Id = Identifier.NewId(),
                Items = Get(segments.Skip(1))
            };

            return item;
        }

        IEnumerable<IPathBarItem> Get(IEnumerable<string> values)
        {
            if (values is null || !values.Any())
            {
                return [];
            }

            var item = new PathBarItem()
            {
                Label = values.ElementAt(0),
                Id = Identifier.NewId(),
                Items = Get(values.Skip(1))
            };

            return [item];
        }
    }
}
