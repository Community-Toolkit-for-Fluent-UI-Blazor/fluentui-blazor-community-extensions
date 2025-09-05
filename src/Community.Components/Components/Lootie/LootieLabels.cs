namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the labels used in the Lootie component for localization.
/// </summary>
public record LootieLabels
{
    /// <summary>
    /// Gets the default English labels.
    /// </summary>
    public static LootieLabels Default { get; } = new();

    /// <summary>
    /// Gets the French labels.
    /// </summary>
    public static LootieLabels French { get; } = new()
    {
        Pause = "Pause",
        Play = "Lecture",
        Stop = "Arrêter",
        Loop = "Boucle",
        Speed = "Vitesse",
        Direction = "Direction"
    };

    /// <summary>
    /// Gets or sets the label for the "Pause" action.
    /// </summary>
    public string Pause { get; init; } = "Pause";

    /// <summary>
    /// Gets or sets the label for the "Play" action.
    /// </summary>
    public string Play { get; init; } = "Play";

    /// <summary>
    /// Gets or sets the label for the "Stop" action.
    /// </summary>
    public string Stop { get; init; } = "Stop";

    /// <summary>
    /// Gets or sets the label for the "Loop" action.
    /// </summary>
    public string Loop { get; init; } = "Loop";

    /// <summary>
    /// Gets or sets the label for the "Speed" action.
    /// </summary>
    public string Speed { get; init; } = "Speed";

    /// <summary>
    /// Gets or sets the label for the "Direction" action.
    /// </summary>
    public string Direction { get; init; } = "Direction";
}
