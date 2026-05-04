namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Defines a layer that encapsulates a payload of a specified type.
/// </summary>
/// <typeparam name="TPayload">The type of payload associated with the layer. Must implement the ILayerPayload interface.</typeparam>
public interface ILayer<TPayload> : ILayer where TPayload : ILayerPayload
{
}
