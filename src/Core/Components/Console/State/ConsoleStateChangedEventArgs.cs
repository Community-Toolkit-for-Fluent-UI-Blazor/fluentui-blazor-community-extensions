namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the event arguments for a console state change event, providing information about the type of change that occurred in the console state.
/// </summary>
/// <param name="Kind">Type of change of the console state.</param>
public sealed record ConsoleStateChangedEventArgs(ConsoleChangeKind Kind)
{
}
