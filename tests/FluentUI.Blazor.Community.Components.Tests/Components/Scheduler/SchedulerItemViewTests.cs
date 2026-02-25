using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bunit;
using FluentUI.Blazor.Community.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Tests;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Scheduler;

public class SchedulerItemViewTests : TestBase
{
    public SchedulerItemViewTests()
    {
        JSInterop.Mode = Bunit.JSRuntimeMode.Loose;
        Services.AddSingleton(UnitTestLibraryConfiguration);
        Services.AddFluentUIComponents();
        Services.AddFluentCxUIComponents();
    }

    private static SchedulerItem<string> CreateItem(string title, DateTime start)
    {
        return new SchedulerItem<string>
        {
            Id = 1,
            Title = title,
            Start = start,
            End = start.AddHours(1),
            Exceptions = new List<DateTime>()
        };
    }

    [Fact]
    public void Renders_default_content_when_no_template()
    {
        // Arrange
        var item = CreateItem("MyTitle", new DateTime(2025, 6, 10));
        var position = new RectangleF(10f, 20f, 120f, 40f);

        // Act
        var cut = RenderComponent<SchedulerItemView<string>>(parameters => parameters
            .Add(p => p.Item, item)
            .Add(p => p.Position, position)
        );

        // Assert
        var root = cut.Find(".scheduler-item");
        Assert.NotNull(root);
        Assert.Contains("MyTitle", root.TextContent);
    }

    [Fact]
    public void Renders_item_template_when_provided()
    {
        // Arrange
        var item = CreateItem("TplTitle", new DateTime(2025, 6, 11));
        var position = new RectangleF(5f, 5f, 80f, 30f);

        RenderFragment<SchedulerItem<string>> template = s => __builder =>
        {
            __builder.AddMarkupContent(0, $"<div class='tpl'>TEMPLATE-{s.Title}</div>");
        };

        // Act
        var cut = RenderComponent<SchedulerItemView<string>>(parameters => parameters
            .Add(p => p.Item, item)
            .Add(p => p.Position, position)
            .Add(p => p.ItemTemplate, template)
        );

        // Assert
        var tpl = cut.FindAll(".tpl").Select(e => e.TextContent.Trim());
        Assert.Contains("TEMPLATE-TplTitle", tpl);
    }

    [Fact]
    public void Double_click_invokes_OnDoubleClick_callback_with_item_and_eventargs()
    {
        // Arrange
        var item = CreateItem("Double", new DateTime(2025, 6, 12));
        var position = new RectangleF(0f, 0f, 50f, 20f);

        Tuple<SchedulerItem<string>, MouseEventArgs>? received = null;
        var callback = EventCallback.Factory.Create<Tuple<SchedulerItem<string>, MouseEventArgs>>(this, (Tuple<SchedulerItem<string>, MouseEventArgs> t) => received = t);

        var cut = RenderComponent<SchedulerItemView<string>>(parameters => parameters
            .Add(p => p.Item, item)
            .Add(p => p.Position, position)
            .Add(p => p.OnDoubleClick, callback)
        );

        var root = cut.Find(".scheduler-item");

        // Act
        root.TriggerEvent("ondblclick", new MouseEventArgs { ClientX = 10, ClientY = 20 });

        // Assert
        Assert.NotNull(received);
        Assert.Equal(item, received!.Item1);
        Assert.Equal(10.0, received.Item2.ClientX);
        Assert.Equal(20.0, received.Item2.ClientY);
    }

    [Fact]
    public void Drag_events_invoke_callbacks()
    {
        // Arrange
        var item = CreateItem("DragMe", new DateTime(2025, 6, 13));
        var position = new RectangleF(1f, 2f, 10f, 10f);

        DragEventArgs? dragStartArgs = null;
        SchedulerItem<string>? dragEndItem = null;

        var dragStartCallback = EventCallback.Factory.Create<DragEventArgs>(this, (DragEventArgs e) => dragStartArgs = e);
        var dragEndCallback = EventCallback.Factory.Create<SchedulerItem<string>>(this, (SchedulerItem<string> s) => dragEndItem = s);

        var cut = RenderComponent<SchedulerItemView<string>>(parameters => parameters
            .Add(p => p.Item, item)
            .Add(p => p.Position, position)
            .Add(p => p.OnDragStarted, dragStartCallback)
            .Add(p => p.OnDragEnded, dragEndCallback)
        );

        var root = cut.Find(".scheduler-item");

        // Act
        root.TriggerEvent("ondragstart", new DragEventArgs());
        root.TriggerEvent("ondragend", new DragEventArgs());

        // Assert
        Assert.NotNull(dragStartArgs);
        Assert.Equal(item, dragEndItem);
    }

    [Fact]
    public void Resize_anchor_triggers_OnResizeStarted_with_direction_and_coordinates()
    {
        // Arrange
        var item = CreateItem("ResizeMe", new DateTime(2025, 6, 14));
        var position = new RectangleF(2f, 3f, 30f, 20f);

        SchedulerResizeEventArgs<string>? received = null;
        var resizeCallback = EventCallback.Factory.Create(this, (SchedulerResizeEventArgs<string> args) => received = args);

        var cut = RenderComponent<SchedulerItemView<string>>(parameters => parameters
            .Add(p => p.Item, item)
            .Add(p => p.Position, position)
            .Add(p => p.ShowTopAnchor, true)
            .Add(p => p.OnResizeStarted, resizeCallback)
        );

        var topAnchor = cut.Find(".scheduler-resize-anchor.top");

        // Act
        topAnchor.TriggerEvent("onmousedown", new MouseEventArgs { ClientX = 123, ClientY = 456 });

        // Assert
        Assert.NotNull(received);
        Assert.Equal(item, received!.Item);
        Assert.Equal(ResizeDirection.Top, received.Direction);
        Assert.Equal(123f, received.X);
        Assert.Equal(456f, received.Y);
    }

    [Fact]
    public void Close_button_presence_and_click_behavior()
    {
        // Arrange
        var item = CreateItem("Closable", new DateTime(2025, 6, 15));
        var position = new RectangleF(0f, 0f, 10f, 10f);

        var cutWithClose = RenderComponent<SchedulerItemView<string>>(parameters => parameters
            .Add(p => p.Item, item)
            .Add(p => p.Position, position)
            .Add(p => p.CanClose, true)
        );

        var closeButton = cutWithClose.FindAll(".scheduler-item-close").FirstOrDefault();
        Assert.NotNull(closeButton);

        // Act & Assert: clicking the button should not throw (Parent may be null in tests)
        closeButton!.Click();

        // When CanClose = false the button should not be rendered
        var cutWithoutClose = RenderComponent<SchedulerItemView<string>>(parameters => parameters
            .Add(p => p.Item, item)
            .Add(p => p.Position, position)
            .Add(p => p.CanClose, false)
        );

        var absent = cutWithoutClose.FindAll(".scheduler-item-close");
        Assert.Empty(absent);
    }
}
