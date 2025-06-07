// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Extensions;

public static class ParameterViewExtensions
{
    public static bool HasValueChanged<T>(
        this ParameterView parameterView,
        string parameterName,
        T? value)
    {
        return parameterView.TryGetValue(parameterName, out T? newValue) && newValue is not null && EqualityComparer<T?>.Default.Equals(newValue, value);
    }
}
