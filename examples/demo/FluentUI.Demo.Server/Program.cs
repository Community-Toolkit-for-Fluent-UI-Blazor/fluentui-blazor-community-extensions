using FluentUI.Blazor.Community.Extensions;
using FluentUI.Demo.Server.Components;
using FluentUI.Demo.Shared;
using Microsoft.FluentUI.AspNetCore.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddFluentUIComponents();
builder.Services.AddFluentCxUIComponents();
builder.Services.AddFluentUIDemoServerServices();
builder.Services.AddAzureOpenAI();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddAdditionalAssemblies([typeof(FluentUI.Demo.Shared._Imports).Assembly])
    .AddInteractiveServerRenderMode();

app.Run();
