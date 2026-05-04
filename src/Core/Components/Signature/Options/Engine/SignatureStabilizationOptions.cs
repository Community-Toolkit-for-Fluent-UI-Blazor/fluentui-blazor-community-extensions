namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides configuration options for real-time signature stroke stabilization.
/// </summary>
public sealed class SignatureStabilizationOptions
{
    /// <summary>
    /// Enables or disables real-time stroke stabilization.
    /// </summary>
    public bool Enabled { get; set; }

    /// <summary>
    /// Stabilization factor between 0 and 1.
    /// 0 = no stabilization, 1 = very strong smoothing.
    /// </summary>
    public double Factor { get; set; } = 0.5;

    /// <summary>
    /// Resets the option to its default values.
    /// </summary>
    public void Reset()
    {
        Enabled = false;
        Factor = 0.5;
    }

    /// <summary>
    /// Clones the current instance of <see cref="SignatureStabilizationOptions"/>, creating a new object with the same property values.
    /// </summary>
    /// <returns>Returns the cloned instance.</returns>
    public SignatureStabilizationOptions Clone()
    {
        return new SignatureStabilizationOptions()
        {
            Enabled = Enabled,
            Factor = Factor
        };
    }
}

