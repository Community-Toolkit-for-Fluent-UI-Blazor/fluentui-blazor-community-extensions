using FluentUI.Blazor.Community.Components.Charts.Payloads;
using FluentUI.Blazor.Community.Components.Enums;

namespace FluentUI.Blazor.Community.Components.Components.Charts.Layers;

internal class ClipPathLayer(ClipPathPayload payload) : ILayer<ClipPathPayload>
{
    /// <inheritdoc />
    public string Key => "clip-path";

    /// <inheritdoc />
    public LayerOrder Order => LayerOrder.Background;

    /// <inheritdoc />
    public int Priority => 0;

    /// <inheritdoc />
    public ILayerPayload LayerPayload => payload;
}
