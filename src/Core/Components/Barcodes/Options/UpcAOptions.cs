namespace FluentUI.Blazor.Community.Components;

internal sealed class UpcAOptions
{
    /// <summary>
    /// Largeur d’un module (barre étroite).
    /// </summary>
    public double ModuleWidth { get; set; } = 2.0;

    /// <summary>
    /// Hauteur des barres.
    /// </summary>
    public double ModuleHeight { get; set; } = 50.0;

    /// <summary>
    /// Gets the height of the guard module, in device-independent units.
    /// </summary>
    public double GuardModuleHeight => ModuleHeight + ModuleHeight * 0.1;
}
