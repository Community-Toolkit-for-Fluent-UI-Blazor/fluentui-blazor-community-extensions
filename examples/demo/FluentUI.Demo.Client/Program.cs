// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using FluentUI.Blazor.Community.Components;
using FluentUI.Blazor.Community.Extensions;
using FluentUI.Demo.Client;
using FluentUI.Demo.Shared.Providers;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.FluentUI.AspNetCore.Components;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddFluentUIComponents();
builder.Services.AddFluentCxUIComponents();
builder.Services.AddAzureOpenAI();
builder.Services.AddSingleton<IFileManagerItemsProvider<NoFileEntryData>, FileManagerItemsProvider>();
builder.Services.AddScoped<FileManagerState>();

await builder.Build().RunAsync();
