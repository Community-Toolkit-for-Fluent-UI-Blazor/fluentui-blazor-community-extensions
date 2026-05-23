using FluentUI.Blazor.Community.Components;
using FluentUI.Demo.Client;
using FluentUI.Demo.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using FluentUI.Blazor.Community.Components.Localization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddHttpClient();

// Add localization services
builder.Services.AddLocalization();

// Add FluentUI services
builder.Services.AddFluentUIComponents(config =>
{
    // Set default values for FluentButton component
    // config.DefaultValues.For<FluentButton>().Set(p => p.Appearance, ButtonAppearance.Primary);
    // config.DefaultValues.For<FluentButton>().Set(p => p.Shape, ButtonShape.Circular);

    // Use a custom localizer
    config.Localizer = new FluentCxLocalizer();
});

// Add FluentUI community extension services
builder.Services.AddFluentCxUIComponents();

// Add Demo server services
builder.Services.AddFluentUIDemoServices().ForServer();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.MapStaticAssets();

app.UseAntiforgery();

// Use the localization services
app.UseRequestLocalization(new RequestLocalizationOptions().AddSupportedUICultures(["en"]));

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(FluentUI.Demo.Client._Imports).Assembly);

app.Run();
