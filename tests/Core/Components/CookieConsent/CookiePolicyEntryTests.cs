using System.Text.Json;
using FluentUI.Blazor.Community.Components;
using Xunit;

namespace Components.Tests.Components.CookieConsent;

public class CookiePolicyEntryTests
{
    [Fact]
    public void CookiePolicyEntry_DefaultValues()
    {
        var entry = new CookiePolicyEntry();

        Assert.Null(entry.Name);
        Assert.Null(entry.Title);
        Assert.Null(entry.Description);
        Assert.False(entry.IsActive);
        Assert.False(entry.Disabled);
    }

    [Fact]
    public void CookiePolicyEntry_AssignsProperties()
    {
        var entry = new CookiePolicyEntry
        {
            Name = "Analytics",
            Title = "Analytics Title",
            Description = "Analytics Description",
            IsActive = true,
            Disabled = true
        };

        Assert.Equal("Analytics", entry.Name);
        Assert.Equal("Analytics Title", entry.Title);
        Assert.Equal("Analytics Description", entry.Description);
        Assert.True(entry.IsActive);
        Assert.True(entry.Disabled);
    }

    [Fact]
    public void CookiePolicyEntry_SerializesExpectedProperties()
    {
        var entry = new CookiePolicyEntry
        {
            Name = "Analytics",
            Title = "Analytics Title",
            Description = "Analytics Description",
            IsActive = true,
            Disabled = true
        };

        var json = JsonSerializer.Serialize(entry);

        Assert.Contains("\"Name\":\"Analytics\"", json);
        Assert.Contains("\"IsActive\":true", json);
        Assert.DoesNotContain("Title", json);
        Assert.DoesNotContain("Description", json);
        Assert.DoesNotContain("Disabled", json);
    }
}
