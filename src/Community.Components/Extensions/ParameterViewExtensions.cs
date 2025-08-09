using Microsoft.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Extensions;

/// <summary>
/// Represents extensions for <see cref="ParameterView"/> struct.
/// </summary>
public static class ParameterViewExtensions
{
    /// <summary>
    /// Gets a value indicating if the <paramref name="parameterName"/> has changed.
    /// </summary>
    /// <typeparam name="T">Type of the value.</typeparam>
    /// <param name="parameterView">Collection of parameters to use.</param>
    /// <param name="parameterName">Name of the parameter to check.</param>
    /// <param name="value">Value of the parameter.</param>
    /// <returns>Returns <see langword="true" /> the value of the parameter has changed, <see langword="false" /> otherwise.</returns>
    public static bool HasValueChanged<T>(
        this ParameterView parameterView,
        string parameterName,
        T? value)
    {
        return parameterView.TryGetValue(parameterName, out T? newValue) && newValue is not null && !EqualityComparer<T?>.Default.Equals(newValue, value);
    }

    /// <summary>
    /// Gets a value indicating if the <paramref name="parameterName"/> has changed.
    /// </summary>
    /// <typeparam name="T">Type of the value.</typeparam>
    /// <param name="parameterView">Collection of parameters to use.</param>
    /// <param name="parameterName">Name of the parameter to check.</param>
    /// <param name="value">Value of the parameter.</param>
    /// <param name="stringComparison">Type of comparison to use.</param>
    /// <returns>Returns <see langword="true" /> the value of the parameter has changed, <see langword="false" /> otherwise.</returns>
    public static bool HasValueChanged(
        this ParameterView parameterView,
        string parameterName,
        string? value,
        StringComparison stringComparison = StringComparison.Ordinal)
    {
        return parameterView.TryGetValue(parameterName, out string? newValue) && !string.Equals(value, newValue, stringComparison);
    }
}
