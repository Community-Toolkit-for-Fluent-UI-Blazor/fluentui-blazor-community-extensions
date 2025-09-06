namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the state of a Lootie component.
/// </summary>
public class LootieState
{
    /// <summary>
    /// Gets a value indicating whether the component is registered.
    /// </summary>
    public bool IsRegister { get; private set; }

    /// <summary>
    /// Registers the component.
    /// </summary>
    /// <exception cref="InvalidOperationException">Occurs when the component is already registered.</exception>
    internal void Register()
    {
        if (IsRegister)
        {
            throw new InvalidOperationException("The component is already registered.");
        }

        IsRegister = true;
    }

    /// <summary>
    /// Unregisters the component.
    /// </summary>
    internal void Unregister() => IsRegister = false;
}
