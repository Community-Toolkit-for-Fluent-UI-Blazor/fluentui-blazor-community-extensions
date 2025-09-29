using Microsoft.FluentUI.AspNetCore.Components.Utilities;
using Microsoft.JSInterop;

namespace FluentUI.Demo.Shared.Infrastructure;

public class DownloadFile(IJSRuntime js) : JSModule(js, "./_content/FluentUI.Demo.Shared/js/downloadFile.js")
{
    public async ValueTask DowloadAsync(string fileName, byte[] content, string contentType)
    {
        await InvokeVoidAsync("downloadFile", fileName, content, true, "_self", contentType);
    }
}
