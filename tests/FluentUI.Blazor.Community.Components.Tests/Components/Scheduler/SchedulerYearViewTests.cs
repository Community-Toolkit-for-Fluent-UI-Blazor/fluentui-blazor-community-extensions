using Bunit;
using FluentUI.Blazor.Community.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Tests;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Scheduler;

public class SchedulerYearViewTests : TestBase
{
    public SchedulerYearViewTests()
    {
        JSInterop.Mode = Bunit.JSRuntimeMode.Loose;
        Services.AddSingleton(UnitTestLibraryConfiguration);
        Services.AddFluentUIComponents();
        Services.AddFluentCxUIComponents();
    }

    [Fact]
    public void Does_not_render_slot_when_exception_date_present()
    {
        // Arrange
        var eventDate = new DateTime(2025, 7, 10);
        var items = new List<SchedulerItem<string>>
        {
            new SchedulerItem<string>
            {
                Title = "EventB",
                Start = eventDate,
                End = eventDate,
                Exceptions = new List<DateTime> { eventDate }
            }
        };

        // Act
        var cut = RenderComponent<SchedulerYearView<string>>(
            parameters => parameters
                .Add(p => p.CurrentDate, new DateTime(2025, 1, 1))
                .Add(p => p.Items, items)
        );

        // Assert
        var slotElements = cut.FindAll(".scheduler-item");
        Assert.False(slotElements.Any(e => e.TextContent.Contains("EventB")), "Did not expect to find 'EventB' because the date is listed as an exception.");
    }
}
