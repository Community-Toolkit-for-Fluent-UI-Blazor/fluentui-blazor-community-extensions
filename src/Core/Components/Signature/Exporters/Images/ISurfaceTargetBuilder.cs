namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Defines a contract for building a surface render target using a specified payload and export options.
/// </summary>
/// <remarks>Implementations of this interface are responsible for transforming the provided payload into a
/// renderable surface using the given target and export options. This interface enables extensibility for custom
/// surface rendering scenarios.</remarks>
/// <typeparam name="TPayload">The type of the payload data used to build the surface render target.</typeparam>
public interface ISurfaceTargetBuilder<TPayload>
{
    /// <summary>
    /// Builds the visual representation of the specified payload onto the given render target using the provided export
    /// options.
    /// </summary>
    /// <param name="target">The render target where the payload will be drawn. Cannot be null.</param>
    /// <param name="payload">The payload containing the data to be rendered. Cannot be null.</param>
    /// <param name="options">The export options that control how the signature is rendered and exported. Cannot be null.</param>
    ValueTask BuildAsync(
        ISurfaceRenderTarget target,
        SurfacePayload<TPayload> payload,
        SurfaceExportOptions options);
}
