namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides options for configuring the quiet zone around a barcode.
/// </summary>
/// <remarks>A quiet zone is the blank margin surrounding a barcode that helps scanners distinguish the barcode
/// from its surroundings. Adjust these options to control whether the quiet zone is applied and to specify its
/// size.</remarks>
public sealed class BarcodeQuietZoneOptions
{
    /// <summary>
    /// Gets or sets a value indicating whether the component is enabled.
    /// </summary>
    public bool Enabled { get; set; } = true;

    /// <summary>
    /// Gets or sets the padding inside the component's content area.
    /// </summary>
    public Thickness Padding { get; set; } = new Thickness(4);
}

