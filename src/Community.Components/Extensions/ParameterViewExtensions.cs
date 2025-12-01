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

    /// <summary>
    /// Determines whether the value of a specified enumerable parameter in the given ParameterView has changed compared
    /// to a provided enumerable value.
    /// </summary>
    /// <remarks>Comparison is performed by checking for null values, count differences, and sequence
    /// equality. This method is useful for detecting changes in enumerable parameters when implementing component
    /// parameter change logic.</remarks>
    /// <typeparam name="T">The type of elements contained in the enumerable parameter.</typeparam>
    /// <param name="parameterView">The ParameterView instance containing the parameters to compare.</param>
    /// <param name="parameterName">The name of the enumerable parameter to check for changes.</param>
    /// <param name="value">The previous value of the enumerable parameter to compare against. Can be null.</param>
    /// <returns>true if the enumerable parameter value in the ParameterView differs from the provided value; otherwise, false.</returns>
    public static bool HasEnumerableValueChanged<T>(
        this ParameterView parameterView,
        string parameterName,
        IEnumerable<T>? value)
    {
        if (parameterView.TryGetValue(parameterName, out IEnumerable<T>? newValue))
        {
            if (value is null && newValue is null)
            {
                return false;
            }

            if (value is null || newValue is null)
            {
                return true;
            }

            if (value.Count() != newValue.Count())
            {
                return true;
            }

            return !value.SequenceEqual(newValue);
        }

        return false;
    }
}
