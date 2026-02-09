namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the status of a chapter in media content.
/// </summary>
public enum ChapterStatus
{
    /// <summary>
    /// Represents an unspecified state.
    /// </summary>
    Unspecified,

    /// <summary>
    /// Represents the state of a chapter that has not yet started.
    /// </summary>
    NotStarted,

    /// <summary>
    /// Represents the state of a chapter that is currently in progress.
    /// </summary>
    Current,

    /// <summary>
    /// Represents the state of a chapter that has been completed.
    /// </summary>
    Completed,
}
