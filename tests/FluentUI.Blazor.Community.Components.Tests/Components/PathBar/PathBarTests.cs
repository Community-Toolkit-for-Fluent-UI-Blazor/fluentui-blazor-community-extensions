using Bunit;
using FluentUI.Blazor.Community.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Icons.Regular;
using Microsoft.FluentUI.AspNetCore.Components.Tests;
using Moq;

namespace FluentUI.Blazor.Community.Components.Tests.Components;

public class PathBarTests : TestBase
{
    public PathBarTests()
    {
        JSInterop.Mode = Bunit.JSRuntimeMode.Loose;
        Services.AddSingleton(UnitTestLibraryConfiguration);
        Services.AddScoped<FileManagerState>();
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
        Services.AddScoped<DeviceInfoState>();
        var cut = RenderPathBar();

        Assert.Contains("stack-horizontal", cut.Markup);
        Assert.DoesNotContain("fluent-anchor", cut.Markup);
    }

    [Theory]
    [InlineData("Home\\FluentUI\\FluentCxBar", 1)]
    [InlineData("Home\\Benchmarks\\FluentUI\\Extensions\\", 2)]
    [InlineData("Home\\Images\\Travels\\2025\\Paris", 3)]
    public void FluentCxPathBar_Overflow(string path, int maxVisibleItems)
    {
        Services.AddScoped<DeviceInfoState>();

        var segments = PathHelper.GetSegments(path);
        var root = Build(segments);
        var cut = RenderPathBar(a =>
        {
            a.Add(p => p.Root, root);
            a.Add(p => p.Path, path);
          //  a.Add(p => p.MaxVisibleItems, maxVisibleItems);
        });

        cut.InvokeAsync(() => { });

        Assert.Contains("stack-horizontal", cut.Markup);
        Assert.Contains("fluent-anchor", cut.Markup);
        Assert.Contains("fluent-button", cut.Markup);
        Assert.Equal(maxVisibleItems + 1, cut.FindAll("fluent-anchor").Count);
    }

    [Fact]
    public void FluentCxPathBar_Icon_Home()
    {
        Services.AddScoped<DeviceInfoState>();

        var root = Build(["Home"]);

        var cut = RenderPathBar(a =>
        {
            a.Add(p => p.Root, root);
        });

        cut.InvokeAsync(() => { });

        Assert.Contains(GetIcon(new Size24.Home()), cut.Markup);
        
    }

    [Fact]
    public void FluentCxPathBar_Icon_Desktop()
    {
        var deviceInfoState = new DeviceInfoState()
        {
            DeviceInfo = new DeviceInfo()
            {
                Mobile = Mobile.NotMobileDevice
            }
        };

        Services.AddScoped(_ => deviceInfoState);

        var root = Build(["Home"]);

        var cut = RenderPathBar(a =>
        {
            a.Add(p => p.Root, root);
        });

        cut.InvokeAsync(() => { });

        Assert.Contains(GetIcon(new Size24.Desktop()), cut.Markup);
    }

    [Fact]
    public void FluentCxPathBar_Icon_Tablet()
    {
        var deviceInfoState = new DeviceInfoState()
        {
            DeviceInfo = new DeviceInfo()
            {
                IsTablet = true
            }
        };

        Services.AddScoped(_ => deviceInfoState);
        var root = Build(["Home"]);

        var cut = RenderPathBar(a =>
        {
            a.Add(p => p.Root, root);
        });

        cut.InvokeAsync(() => { });

        Assert.Contains(GetIcon(new Size24.Tablet()), cut.Markup);

    }

    [Fact]
    public void FluentCxPathBar_Icon_Smartphone()
    {
        var deviceInfoState = new DeviceInfoState()
        {
            DeviceInfo = new DeviceInfo()
            {
                Mobile = Mobile.Android
            }
        };

        Services.AddScoped(_ => deviceInfoState);
        var root = Build(["Home"]);

        var cut = RenderPathBar(a =>
        {
            a.Add(p => p.Root, root);
        });

        cut.InvokeAsync(() => { });

        Assert.Contains(GetIcon(new Size24.Phone()), cut.Markup);

    }

    [Theory]
    [InlineData("Home\\FluentUI\\FluentCxBar")]
    [InlineData("Home\\Benchmarks\\FluentUI\\Extensions\\")]
    [InlineData("Home\\Images\\Travels\\2025\\Paris")]
    [InlineData("Home")]
    public void FluentCxPathBar_With_Root_And_Path(string value)
    {
        Services.AddScoped<DeviceInfoState>();

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
    }

    private static string GetIcon(Icon icon)
    {
        var iconString = icon.ToMarkup().ToString();
        var index = iconString.IndexOf("<path");
        var lastIndex = iconString.IndexOf("/>", index);

        return iconString[index..lastIndex];
    }

    static IPathBarItem Build(string[] segments)
    {
        PathBarItem item = new()
        {
            Label = segments[0],
            Id = Identifier.NewId(),
            Items = Get(segments.Skip(1))
        };

        return item;
    }

    static IEnumerable<IPathBarItem> Get(IEnumerable<string> values)
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
