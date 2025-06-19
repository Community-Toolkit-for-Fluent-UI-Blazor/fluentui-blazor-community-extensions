using FluentUI.Demo.Shared.Infrastructure;

namespace FluentUI.Demo.Shared.Layout;
public partial class DemoMainLayout
{
    private string? _version;
    protected override void OnInitialized()
    {
        _version = AppVersionService.GetVersionFromAssembly();
    }
}
