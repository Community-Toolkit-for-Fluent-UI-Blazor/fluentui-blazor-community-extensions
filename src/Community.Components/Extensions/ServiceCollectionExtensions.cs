// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System.ClientModel;
using FluentUI.Blazor.Community.Components;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Azure.AI.OpenAI;
using Microsoft.Extensions.AI;

namespace FluentUI.Blazor.Community.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddFluentCxUIComponents(this IServiceCollection services)
    {
        return services.AddScoped(typeof(DropZoneState<>))
                       .AddScoped<FileManagerState>();
    }

    public static IServiceCollection AddAzureOpenAI(this IServiceCollection services)
    {
        return services.AddScoped(typeof(IChatClient), p => GetChatClient(p.GetRequiredService<IConfiguration>()));
    }

    private static IChatClient GetChatClient(IConfiguration configuration)
    {
        var section = configuration.GetSection("AI:Azure");
        var endpoint = section.GetValue<string>("Endpoint");
        var credentials = section.GetValue<string>("Credentials");
        var model = section.GetValue<string>("Model");

        ArgumentException.ThrowIfNullOrEmpty(endpoint, nameof(endpoint));
        ArgumentException.ThrowIfNullOrEmpty(credentials, nameof(credentials));
        ArgumentException.ThrowIfNullOrEmpty(model, nameof(model));
        
        return new AzureOpenAIClient(
            new(endpoint),
            new ApiKeyCredential(credentials)
        )
                .GetChatClient(model)
                .AsIChatClient();
    }
}
