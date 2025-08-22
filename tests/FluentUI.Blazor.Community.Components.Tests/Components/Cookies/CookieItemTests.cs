using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Cookies;

public class CookieItemTests
{
    [Fact]
    public void CookieItem_CanBeInstantiatedAndPropertiesSet()
    {
        // Arrange
        var name = "essential_cookie";
        var title = "Essential Cookie";
        var description = "This is an essential cookie.";
        var isActive = true;
        var disabled = false;

        // Act
        var cookieItem = new CookieItem
        {
            Name = name,
            Title = title,
            Description = description,
            IsActive = isActive,
            Disabled = disabled
        };

        // Assert
        Assert.NotNull(cookieItem);
        Assert.Equal(cookieItem.Name, name);
        Assert.Equal(cookieItem.Title, title);
        Assert.Equal(cookieItem.Description, description);
        Assert.Equal(cookieItem.IsActive, isActive);
        Assert.Equal(cookieItem.Disabled, disabled);
    }

    [Fact]
    public void CookieItem_NullablePropertiesHandleNull()
    {
        // Act
        var cookieItem = new CookieItem
        {
            Name = null,
            Title = "Nullable Title", 
            Description = "Nullable Description",
            IsActive = null
        };

        // Assert
        Assert.NotNull(cookieItem);
        Assert.Null(cookieItem.Name);
        Assert.Null(cookieItem.IsActive);
        Assert.Equal("Nullable Title", cookieItem.Title); 
        Assert.Equal("Nullable Description", cookieItem.Description); 
    }

    [Fact]
    public void CreateGoogleAnalyticsCookie_ReturnsCorrectlyConfiguredItem()
    {
        // Arrange
        var expectedDescription = "This cookie is used for Google Analytics tracking.";

        // Act
        var cookieItem = CookieItem.CreateGoogleAnalyticsCookie(expectedDescription);

        // Assert
        Assert.NotNull(cookieItem);
        Assert.True(cookieItem.Disabled);
        Assert.True(cookieItem.IsActive);
        Assert.Equal("Google Analytics", cookieItem.Title);
        Assert.Equal(expectedDescription, cookieItem.Description);
        Assert.Equal(FluentCxCookie.GoogleAnalytics, cookieItem.Name);
    }
}
