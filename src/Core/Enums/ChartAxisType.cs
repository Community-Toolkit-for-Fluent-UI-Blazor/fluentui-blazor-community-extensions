namespace FluentUI.Blazor.Community.Components.Enums;

/// <summary>
/// Specifies the type of axis used in a chart, such as category, linear, or time-based axes.
/// </summary>
/// <remarks>Use this enumeration to indicate how data should be interpreted and displayed along a chart
/// axis. The axis type affects scaling, labeling, and formatting of axis values.</remarks>
internal enum ChartAxisType
{
    /// <summary>
    /// Represents a categorical axis, where each value corresponds to a distinct category or group. 
    /// </summary>
    Category,

    /// <summary>
    /// Represents a numeric axis, where values are interpreted as continuous numbers and scaled accordingly.
    /// </summary>
    Numeric,

    /// <summary>
    /// Represents a specific point in time or a time value.
    /// </summary>
    Time,

    /// <summary>
    /// Represents a polar axis.
    /// </summary>
    Polar,
}
