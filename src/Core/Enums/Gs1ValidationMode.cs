namespace FluentUI.Blazor.Community.Components.Enums;

/// <summary>
/// Specifies the available modes for validating GS1 barcodes or data according to different levels of strictness.
/// </summary>
public enum Gs1ValidationMode
{
    /// <summary>
    /// Specifies a strict mode for validating GS1 data.
    /// </summary>
    Strict,

    /// <summary>
    /// Specifies a permissive mode.
    /// </summary>
    Permissive,

    /// <summary>
    /// Represents a hybrid mode.
    /// </summary>
    Hybrid
}
