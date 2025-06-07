// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using FluentUI.Blazor.Community.Components;
using Microsoft.Extensions.DependencyInjection;

namespace FluentUI.Blazor.Community.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddFluentCxUIComponents(this IServiceCollection services)
    {
        return services.AddScoped(typeof(DropZoneState<>))
                       .AddScoped<FileManagerSortState>()
                       ;
    }
}
