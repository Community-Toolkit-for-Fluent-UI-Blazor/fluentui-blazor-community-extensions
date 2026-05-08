namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents configuration settings for the linear layout of a SleekDial component, including item spacing and the gap
/// between the floating action button (FAB) and the popup.
/// </summary>
public class SleekDialLinearSettings
{
    /// <summary>
    /// Represents the offset of the items from the FAB and the gap between the FAB and the popup.
    /// </summary>
    private string _itemOffset = "12px";

    /// <summary>
    /// Represents the default gap value used for spacing elements, specified in pixels.
    /// </summary>
    private string _gap = "4px";

    /// <summary>
    /// Occurs when a settings value changes, providing the name and new value of the changed setting.
    /// </summary>
    /// <remarks>The event provides a tuple containing the setting name and its updated value. Subscribers can
    /// use this information to respond to specific setting changes.</remarks>
    internal event EventHandler<(string, string)>? OnSettingsChanged;

    /// <summary>
    /// Gets or sets the offset of the items from the FAB.
    /// </summary>
    /// <remarks>This value is set to 12px by default.</remarks>
    public string ItemOffset
    {
        get => _itemOffset;
        set
        {
            if (_itemOffset != value)
            {
                _itemOffset = value;
                OnSettingsChanged?.Invoke(this, (nameof(ItemOffset), _itemOffset));
            }
        }
    }

    /// <summary>
    /// Gets or sets the gap between the FAB and the popup.
    /// </summary>
    /// <remarks>This value is set to 4px by default.</remarks>
    public string Gap
    {
        get => _gap;
        set
        {
            if (_gap != value)
            {
                _gap = value;
                OnSettingsChanged?.Invoke(this, (nameof(Gap), _gap));
            }
        }
    }
}

