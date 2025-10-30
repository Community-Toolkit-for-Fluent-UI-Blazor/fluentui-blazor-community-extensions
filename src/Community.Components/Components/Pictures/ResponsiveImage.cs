using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a responsive image component.
/// </summary>
public class ResponsiveImage
    : FluentComponentBase, IDisposable
{
    [CascadingParameter]
    private FluentCxPicture Parent { get; set; }

    [Parameter]
    public string? Media { get; set; }

    [Parameter]
    public string? Source { get; set; }

    [Parameter]
    public string? ContentType { get; set; }

    protected override void OnInitialized()
    {
        base.OnInitialized();

        Parent?.AddSource(this);
    }

    public void Dispose()
    {
        Parent?.RemoveSource(this);
    }
}
