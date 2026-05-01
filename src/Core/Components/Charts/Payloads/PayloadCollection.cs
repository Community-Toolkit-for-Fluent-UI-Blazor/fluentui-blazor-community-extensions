namespace FluentUI.Blazor.Community.Components.Charts.Payloads;

internal sealed class PayloadCollection<T>(IReadOnlyList<T> payload) : ILayerPayload
{
    public IReadOnlyList<T> Payloads { get; } = payload;

    public required string Id { get; set; } = string.Empty;
}
