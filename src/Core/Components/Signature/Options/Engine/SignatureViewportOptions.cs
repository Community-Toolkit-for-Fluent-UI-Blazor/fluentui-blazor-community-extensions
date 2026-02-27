namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents configuration options for the viewport of a signature surface, including zoom and pan settings.
/// </summary>
/// <remarks>Use this class to control the visual presentation of a signature area, such as scaling and
/// positioning. The options can be reset to their default values using the Reset method.</remarks>
public class SignatureViewportOptions
{
    /// <summary>
    /// Gets or sets the zoom level applied to the content.
    /// </summary>
    /// <remarks>The zoom level determines the scaling factor for the content. A value of 1.0 represents the
    /// default scale; values greater than 1.0 increase the size, while values less than 1.0 decrease it.</remarks>
    public double Zoom { get; set; } = 1.0;

    /// <summary>
    /// Gets or sets the horizontal pan offset applied to the content.
    /// </summary>
    public double PanX { get; set; }

    /// <summary>
    /// Gets or sets the vertical offset for panning the content.
    /// </summary>
    public double PanY { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the component's content scales proportionally with the surface area.
    /// </summary>
    /// <remarks>Set this property to <see langword="true"/> to ensure that the content automatically adjusts
    /// its size based on the surface. If <see langword="false"/>, the content maintains its original scale regardless
    /// of surface changes.</remarks>
    public bool ScaleWithSurface { get; set; } = true;

    /// <summary>
    /// Resets the zoom and pan values to their default states.
    /// </summary>
    /// <remarks>This method sets the zoom to 1.0 and both pan coordinates to 0.0. Use this method to restore
    /// the view to its original position and scale.</remarks>
    public void Reset()
    {
        Zoom = 1.0;
        PanX = 0.0;
        PanY = 0.0;
    }
}

