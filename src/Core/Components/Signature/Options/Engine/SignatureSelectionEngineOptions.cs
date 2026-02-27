namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents configuration options for the signature selection engine, including selection sensitivity and enablement.
/// </summary>
public class SignatureSelectionEngineOptions
{
    /// <summary>
    /// Gets or sets the distance threshold used to detect stroke selection.
    /// </summary>
    public double Tolerance { get; set; } = 6.0;

    /// <summary>
    /// Gets or sets a value indicating whether the stroke selection is enabled.
    /// </summary>
    public bool Enabled { get; set; } = true;
}
