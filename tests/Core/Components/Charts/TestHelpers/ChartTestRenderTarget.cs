using FluentUI.Blazor.Community.Components;

namespace Components.Tests.Components.Charts.TestHelpers;

internal sealed class ChartTestRenderTarget : ISurfaceRenderTarget
{
    private readonly List<ILayer> _layers = [];

    public IReadOnlyList<ILayer> Layers => _layers;

    public ViewPayload View { get; private set; } = new();

    public void SetView(ViewPayload view)
    {
        View = view ?? throw new ArgumentNullException(nameof(view));
    }

    public ValueTask FlushAsync() => ValueTask.CompletedTask;

    public object? GetNativeHandle() => null;

    public ISurfaceRenderTarget AddLayer(ILayer layer)
    {
        ArgumentNullException.ThrowIfNull(layer);
        _layers.Add(layer);
        return this;
    }
}
