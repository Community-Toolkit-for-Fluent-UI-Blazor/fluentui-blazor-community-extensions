using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Scheduler;

public class SchedulerEventContentTests
{
    [Fact]
    public void DefaultValues_AreCorrect()
    {
        var sut = new SchedulerEventContent<string>();

        Assert.Equal(SchedulerLabels.Default, sut.Labels);
        Assert.Equal(CultureInfo.CurrentCulture, sut.Culture);
        Assert.Null(sut.Item);
        Assert.Null(sut.Template);
    }

    [Fact]
    public void InitProperties_CanBeSet_WithObjectInitializer()
    {
        var customLabels = SchedulerLabels.French;

        RenderFragment<string> template = context => builder =>
        {
            builder.AddContent(0, $"item:{context}");
        };

        var culture = new CultureInfo("fr-FR");

        var sut = new SchedulerEventContent<string>
        {
            Labels = customLabels,
            Template = template,
            Culture = culture
            // Note: Item is of type SchedulerItem<TItem>; avoid setting here to keep test compilation isolated.
        };

        Assert.Same(customLabels, sut.Labels);
        Assert.Same(template, sut.Template);
        Assert.Equal(culture, sut.Culture);
    }

    [Fact]
    public void Culture_IsMutable_AfterConstruction()
    {
        var sut = new SchedulerEventContent<int>();
        var newCulture = new CultureInfo("en-GB");

        sut.Culture = newCulture;

        Assert.Equal(newCulture, sut.Culture);
    }
}
