using FluentUI.Blazor.Community.Geometry;

namespace FluentUI.Blazor.Community.Components;

public record ResizedEventArgs
{
    public string? Id { get; set; }

    public ResizerHandler Orientation { get; set; }

    public SizeF OriginalSize { get; set; }

    public SizeF NewSize { get; set; }
}
