using Microsoft.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

public interface IDropZoneComponent<TItem>
{
    TItem? Value { get; }

    string? Id { get; }

    RenderFragment? Component { get; }
}
