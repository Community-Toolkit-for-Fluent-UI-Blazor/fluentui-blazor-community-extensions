using FluentUI.Blazor.Community.Components;

namespace FluentUI.Demo.Client.Infrastructure;

internal sealed class DefaultFileProviderCapabilities
    : IFileProviderCapabilities
{
    public bool CanDelete => true;

    public bool CanRename => true;

    public bool CanMove => true;

    public bool CanCreateDirectory => true;

    public bool CanUpload => true;
}
