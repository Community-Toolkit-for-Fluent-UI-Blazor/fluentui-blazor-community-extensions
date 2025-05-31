// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace FluentUI.Blazor.Community.Components;

public record FileManagerContent(
    string? Label,
    string? Placeholder,
    string? Name,
    bool IsDirectory,
    bool IsRenaming
    )
{
    public string? Name { get; set; } = Name;
}
