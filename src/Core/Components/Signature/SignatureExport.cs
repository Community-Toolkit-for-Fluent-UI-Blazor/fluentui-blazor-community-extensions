using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the signature exporter.
/// </summary>
public sealed class SignatureExport
    : FluentComponentBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SignatureExport"/> class.
    /// </summary>
    /// <param name="configuration"></param>
    public SignatureExport(LibraryConfiguration configuration)
        : base(configuration)
    {
        Id = Identifier.NewId();
    }
}
