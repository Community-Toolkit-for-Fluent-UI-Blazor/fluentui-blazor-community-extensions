//namespace FluentUI.Blazor.Community.Components;

///// <summary>
///// Enriches console output with information from the current HTTP context.
///// </summary>
///// <remarks>This class implements the IConsoleEnricher interface to add HTTP context details, such as request or
///// user information, to console logs. It is intended for use in ASP.NET Core applications where HTTP context is
///// available.</remarks>
///// <param name="httpContextAccessor">The accessor used to retrieve the current HTTP context, enabling extraction of HTTP-specific data for console
///// enrichment. Cannot be null.</param>
//internal sealed class HttpContextEnricher(IHttpContextAccessor httpContextAccessor)
//    : IConsoleEnricher
//{
//    /// <inheritdoc />
//    public void Enrich(ConsoleEnrichmentBag bag)
//    {
//        var context = httpContextAccessor.HttpContext;

//        if (context == null)
//        {
//            return;
//        }

//        var request = context.Request;
//        var user = context.User;

//        bag.Properties["Http.Method"] = request.Method;
//        bag.Properties["Http.Path"] = request.Path;
//        bag.Properties["Http.QueryString"] = request.QueryString.ToString();
//        bag.Properties["Http.Scheme"] = request.Scheme;
//        bag.Properties["Http.Host"] = request.Host.ToString();
//        bag.Properties["Http.TraceId"] = context.TraceIdentifier;
//        var corr = request.Headers["X-Correlation-ID"].FirstOrDefault();

//        if (!string.IsNullOrEmpty(corr))
//        {
//            bag.CorrelationId = corr;
//        }

//        if (user?.Identity?.IsAuthenticated == true)
//        {
//            bag.Properties["User.Identity.Name"] = user.Identity.Name;
//            bag.Properties["User.Identity.AuthenticationType"] = user.Identity.AuthenticationType;
//        }

//        var ip = context.Connection.RemoteIpAddress?.ToString();

//        if (!string.IsNullOrEmpty(ip))
//        {
//            bag.Properties["Http.ClientIp"] = ip;
//        }
//    }
//}
