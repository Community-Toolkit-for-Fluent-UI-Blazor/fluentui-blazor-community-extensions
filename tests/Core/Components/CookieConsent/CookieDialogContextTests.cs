using System.Collections.Generic;
using FluentUI.Blazor.Community.Components;
using Microsoft.AspNetCore.Components;
using Xunit;

namespace Components.Tests.Components.CookieConsent;

public class CookieDialogContextTests
{
    [Fact]
    public void CookieDialogContext_AssignsProperties()
    {
        var items = new List<CookiePolicyEntry>
        {
            new() { Name = "Analytics", IsActive = true },
            new() { Name = "Marketing", IsActive = false }
        };
        var labels = new CookieLabels();
        RenderFragment<CookiePolicyEntry> template = entry => builder => builder.AddContent(0, entry.Name);

        var context = new CookieDialogContext(items, labels, template);

        Assert.Same(items, context.Items);
        Assert.Same(labels, context.Labels);
        Assert.Same(template, context.ItemTemplate);
    }

    [Fact]
    public void CookieDialogContext_AllowsNullTemplate()
    {
        var items = new List<CookiePolicyEntry> { new() { Name = "Analytics" } };
        var labels = new CookieLabels();

        var context = new CookieDialogContext(items, labels);

        Assert.Same(items, context.Items);
        Assert.Same(labels, context.Labels);
        Assert.Null(context.ItemTemplate);
    }
}
