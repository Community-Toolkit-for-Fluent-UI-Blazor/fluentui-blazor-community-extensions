using System.Text.Json.Serialization;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a cookie item.
/// </summary>
public record CookieItem
{
    /// <summary>
    /// Gets or sets the name of the item.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the title of the item.
    /// </summary>
    [JsonIgnore]
    public string? Title { get; set; }

    /// <summary>
    /// Gets or sets the description of the item.
    /// </summary>
    [JsonIgnore]
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets a value indicating if the item is active.
    /// </summary>
    public bool? IsActive { get; set; }

    /// <summary>
    /// Gets or sets a value indicating that the item is disabled.
    /// </summary>
    /// <remarks>
    /// A disabled cookie item defines a mandatory item.
    /// </remarks>
    [JsonIgnore]
    public bool Disabled { get; set; }

    /// <summary>
    /// Gets the cookie item for the google analytics view.
    /// </summary>
    /// <param name="description">Description of the cookie.</param>
    /// <returns>Returns the cookie.</returns>
    internal static CookieItem CreateGoogleAnalyticsCookie(string description)
    {
        return new()
        {
            Disabled = true,
            IsActive = true,
            Title = "Google Analytics",
            Description = description,
            Name = FluentCxCookie.GoogleAnalytics
        };
    }
}
