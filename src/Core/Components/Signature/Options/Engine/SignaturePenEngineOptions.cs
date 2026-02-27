namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents configuration options for customizing the appearance and behavior of the pen used in a signature drawing
/// component.
/// </summary>
/// <remarks>Use this class to specify pen attributes such as color, width, dash style, smoothing, blending, and
/// shadow effects. These options control how strokes are rendered when capturing or displaying signatures. Changing
/// properties affects subsequent strokes; to revert to defaults, call the Reset method. This class is typically used in
/// conjunction with signature input or rendering components.</remarks>
public class SignaturePenEngineOptions
{
    /// <summary>
    /// Represents the base width used for rendering or layout calculations, in device-independent units.
    /// </summary>
    private double _baseWidth = 2.0;

    /// <summary>
    /// Represents the minimum width of the pen.
    /// </summary>
    private double _minWidth = 0.5;

    /// <summary>
    /// Represents the maximum width of the pen.
    /// </summary>
    private double _maxWidth = 4.0;

    /// <summary>
    /// Represents how the width of the pen is computed based on input parameters such as pressure or velocity.
    /// </summary>
    private StrokeWidthMode _strokeWidthMode = StrokeWidthMode.Linear;

    /// <summary>
    /// Represents the exponent used for calculating width in Power mode.
    /// </summary>
    private double _widthPower = 1.0;

    /// <summary>
    /// Represents the maximum velocity value used to determine width calculations.
    /// </summary>
    private double _maxVelocityForWidth = 2.0;

    /// <summary>
    /// Occurs when the options associated with this instance have changed.
    /// </summary>
    public event EventHandler? OptionsChanged;

    /// <summary>
    /// Gets or sets the base width used for rendering or layout calculations.
    /// </summary>
    public double BaseWidth
    {
        get { return _baseWidth; }
        set
        {
            if (_baseWidth != value)
            {
                _baseWidth = value;
                Notify();
            }
        }
    }

    /// <summary>
    /// Gets or sets the minimum width, in device-independent units, that the component can be resized to.
    /// </summary>
    /// <remarks>The value must be greater than zero. Setting a value less than or equal to zero may result in
    /// unexpected layout behavior.</remarks>
    public double MinWidth
    {
        get => _minWidth;
        set
        {
            if (_minWidth != value)
            {
                _minWidth = value;
                Notify();
            }
        }
    }

    /// <summary>
    /// Gets or sets the maximum width allowed for the content, in device-independent units.
    /// </summary>
    /// <remarks>The value should be greater than zero. Setting a value less than or equal to zero may result
    /// in undefined layout behavior.</remarks>
    public double MaxWidth
    {
        get => _maxWidth;
        set
        {
            if (_maxWidth != value)
            {
                _maxWidth = value;
                Notify();
            }
        }
    }

    /// <summary>
    /// Gets or sets how width is computed
    /// </summary>
    public StrokeWidthMode WidthMode
    {
        get => _strokeWidthMode;
        set
        {
            if (_strokeWidthMode != value)
            {
                _strokeWidthMode = value;
                Notify();
            }
        }
    }

    /// <summary>
    /// Gets or sets the exponent used for Power width mode.
    /// </summary>
    public double WidthPower
    {
        get => _widthPower;
        set
        {
            if (_widthPower != value)
            {
                _widthPower = value;
                Notify();
            }
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether pressure sensitivity is enabled.
    /// </summary>
    public double MaxVelocityForWidth
    {
        get => _maxVelocityForWidth;
        set
        {
            if (value != _maxVelocityForWidth)
            {
                _maxVelocityForWidth = value;
                Notify();
            }
        }
    }

    /// <summary>
    /// Resets all stroke settings to their default values.
    /// </summary>
    /// <remarks>Use this method to restore the stroke configuration to its initial state. This is useful when
    /// you want to clear any customizations and revert to the standard settings for drawing operations.</remarks>
    public void Reset()
    {
        BaseWidth = 2.0;
        MinWidth = 0.5;
        MaxWidth = 4.0;
        MaxVelocityForWidth = 2.0;
        WidthPower = 1.0;
        WidthMode = StrokeWidthMode.Linear;
    }

    /// <summary>
    /// Creates a new copy of the current SignaturePenOptions instance with the same property values.
    /// </summary>
    /// <remarks>The returned object is a deep copy for reference-type properties, ensuring that changes to
    /// the clone do not affect the original instance.</remarks>
    /// <returns>A new SignaturePenOptions object that is a copy of the current instance.</returns>
    public SignaturePenEngineOptions Clone()
    {
        return new SignaturePenEngineOptions
        {
            BaseWidth = _baseWidth,
            MinWidth = _minWidth,
            MaxWidth = _maxWidth,
            WidthMode = _strokeWidthMode,
            WidthPower = _widthPower,
            MaxVelocityForWidth = _maxVelocityForWidth
        };
    }

    private void Notify()
    {
        OptionsChanged?.Invoke(this, EventArgs.Empty);
    }
}

