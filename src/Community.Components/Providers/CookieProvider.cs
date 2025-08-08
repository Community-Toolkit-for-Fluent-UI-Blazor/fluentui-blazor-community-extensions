using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace FluentUI.Blazor.Community.Providers;

internal sealed class CookieProvider(
    IHttpContextAccessor httpContextAccessor,
    IConfiguration configuration)
    : ICookieProvider
{
    public string? Cookie { get; } = httpContextAccessor.HttpContext?.Request.Cookies[configuration.GetSection("Application").GetSection("Azure:SignalR").GetValue("CookieName", string.Empty)];
}
