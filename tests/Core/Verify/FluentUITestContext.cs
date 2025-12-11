// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using AngleSharp.Html.Parser;
using Bunit;
using Microsoft.Extensions.DependencyInjection;

namespace FluentUI.Blazor.Community.Components.Tests.Verify;

/// <summary />
public class FluentUITestContext : Bunit.BunitContext
{
    /// <summary />
    public FluentUITestContext()
    {
        Services.AddSingleton<IHtmlParser>(new HtmlParser());
    }

    /// <summary />
    public FluentUITestContext(JSRuntimeMode mode) : this()
    {
        JSInterop.Mode = mode;
    }
}
