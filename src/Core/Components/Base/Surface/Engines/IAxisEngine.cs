using FluentUI.Blazor.Community.Components.Surface.Payloads;

namespace FluentUI.Blazor.Community.Components.Surface.Engines;

/// <summary>
/// Represents an engine responsible for building the axes payload based on the provided view and options. 
/// </summary>
/// <typeparam name="TView">The type of the view or context from which the axes payload will be constructed.</typeparam>
/// <typeparam name="TOptions">The options or configuration settings that influence how the axes payload is built.</typeparam>
public interface IAxisEngine<TView, TOptions>
{
    /// <summary>
    /// Builds an axes payload based on the specified view and options.
    /// </summary>
    /// <param name="view">The view instance that provides the context or data for constructing the axes payload. Cannot be null.</param>
    /// <param name="options">The options that configure how the axes payload is built. Cannot be null.</param>
    /// <returns>An instance of AxesPayload representing the constructed axes data, or null if the payload could not be built.</returns>
    AxisPayload? Build(TView view, TOptions options);
}
