namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the settings for the radial mode.
/// </summary>
public class SleekDialRadialSettings
{
    /// <summary>
    /// Represents the start angle of the radial arc, specified in degrees.
    /// </summary>
    private int _startAngle = -1;

    /// <summary>
    /// Represents the end angle of the radial arc, specified in degrees.
    /// </summary>
    private int _endAngle = -1;

    /// <summary>
    /// Represents the offset of the items on the arc, specified as a CSS length value.
    /// </summary>
    private string _offset = "110px";

    /// <summary>
    /// Specifies the radial direction used by the dial control.
    /// </summary>
    /// <remarks>This field determines whether the dial operates in a clockwise or counterclockwise direction.
    /// The value affects how user interactions are interpreted when rotating the dial.</remarks>
    private SleekDialRadialDirection _direction = SleekDialRadialDirection.Clockwise;

    /// <summary>
    /// Represents the event that is triggered when any of the radial settings are changed.
    /// </summary>
    internal event EventHandler? OnSettingsChanged;

    /// <summary>
    /// Gets or sets the start angle of the radial arc.
    /// </summary>
    public int StartAngle
    {
        get => _startAngle;
        set
        {
            if (_startAngle != value)
            {
                _startAngle = value;
                OnSettingsChanged?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    /// <summary>
    /// Gets or sets the end angle of the radial arc.
    /// </summary>
    public int EndAngle
    {
        get => _endAngle;
        set
        {
            if (_endAngle != value)
            {
                _endAngle = value;
                OnSettingsChanged?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    /// <summary>
    /// Gets or sets the offset of the items on the arc.
    /// </summary>
    public string Offset
    {
        get => _offset;
        set
        {
            if (_offset != value)
            {
                _offset = value;
                OnSettingsChanged?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    /// <summary>
    /// Gets or sets the direction of the items.
    /// </summary>
    public SleekDialRadialDirection Direction
    {
        get => _direction;
        set
        {
            if (_direction != value)
            {
                _direction = value;
                OnSettingsChanged?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
