using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentUI.Blazor.Community.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Tests;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Scheduler;

public class SchedulerCanvasTests : TestBase
{
    public SchedulerCanvasTests()
    {
        JSInterop.Mode = Bunit.JSRuntimeMode.Loose;
        Services.AddSingleton(UnitTestLibraryConfiguration);
        Services.AddFluentUIComponents();
        Services.AddFluentCxUIComponents();
    }

    [Fact]
    public void Render_DoesNotThrow_With_CascadingParent()
    {
        // Arrange
        var parent = RenderComponent<FluentCxScheduler<string>>();

        var cut = RenderComponent<SchedulerCanvas<string>>(p => p
            .AddCascadingValue(parent.Instance)
        );

        Assert.NotNull(cut.Instance);
    }

    [Fact]
    public async Task OnPointerUpAsync_Invokes_DisabledCells_Callback_When_ItemExists()
    {
        // Arrange
        var parent = RenderComponent<FluentCxScheduler<string>>();

        var item = new SchedulerItem<string>
        {
            Id = 123,
            Title = "Test",
            Start = DateTime.Today.AddHours(9),
            End = DateTime.Today.AddHours(10),
            Exceptions = new List<DateTime>()
        };

        bool? disabled = null;
        var disabledCallback = EventCallback.Factory.Create(this, (bool v) => disabled = v);

        var cut = RenderComponent<SchedulerCanvas<string>>(p => p
            .AddCascadingValue(parent.Instance)
            .Add(param => param.Items, new List<SchedulerItem<string>> { item })
            .Add(param => param.DisabledCells, disabledCallback)
        );

        // Act
        await cut.Instance.OnPointerUpAsync(item.Id, 0, "mouse");

        Assert.False(disabled);
    }

    [Fact]
    public async Task OnPointerUpAsync_DoesNotInvoke_DisabledCells_When_ItemNotFound()
    {
        // Arrange
        var parent = RenderComponent<FluentCxScheduler<string>>();

        var called = false;
        var disabledCallback = EventCallback.Factory.Create<bool>(this, (bool v) => called = true);

        var cut = RenderComponent<SchedulerCanvas<string>>(p => p
            .AddCascadingValue(parent.Instance)
            .Add(param => param.Items, new List<SchedulerItem<string>>())
            .Add(param => param.DisabledCells, disabledCallback)
        );

        // Act
        await cut.Instance.OnPointerUpAsync(9999, 0, "mouse");

        // Assert - callback should not be called because item id not found
        Assert.False(called);
    }

    [Fact]
    public async Task DisposeAsync_DoesNotThrow()
    {
        // Arrange
        var parent = RenderComponent<FluentCxScheduler<string>>();

        var cut = RenderComponent<SchedulerCanvas<string>>(p => p
            .AddCascadingValue(parent.Instance)
        );

        // Act / Assert - DisposeAsync should complete without exceptions
        await cut.Instance.DisposeAsync();
    }
}
