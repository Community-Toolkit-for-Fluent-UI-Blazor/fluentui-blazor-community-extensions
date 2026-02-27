namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents configuration options for smoothing strokes in a signature input.
/// </summary>
/// <remarks>Use this class to control whether stroke smoothing is applied and to configure the smoothing window
/// size. Smoothing can help produce more natural-looking signature lines by averaging input points.</remarks>
public sealed class SignatureSmoothingOptions
{
    /// <summary>
    /// Enables or disables stroke smoothing.
    /// </summary>
    public bool Enabled { get; set; }

    /// <summary>
    /// Size of the smoothing window (number of points).
    /// </summary>
    public int WindowSize { get; set; } = 3;

    /// <summary>
    /// Resets the component to its default state by disabling it and restoring the default window size.
    /// </summary>
    public void Reset()
    {
        Enabled = false;
        WindowSize = 3;
    }

    /// <summary>
    /// Clones the current instance of <see cref="SignatureSmoothingOptions"/>, creating a new object with the same property values.
    /// </summary>
    /// <returns>Returns the cloned instance.</returns>
    public SignatureSmoothingOptions Clone()
    {
        return new SignatureSmoothingOptions()
        {
            Enabled = Enabled,
            WindowSize = WindowSize
        };
    }
}

