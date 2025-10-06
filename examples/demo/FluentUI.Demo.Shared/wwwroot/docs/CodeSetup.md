# Code Setup

## Getting Started

To start using the **Fluent UI Community Extension** library and Razor components from scratch, you first need to install the main [NuGet package](https://www.nuget.org/packages/FluentUI.Blazor.Community.Components/) in the project you want to use the library and its components.
You can use the NuGet package manager in your IDE or use the following command when using a CLI:

```shell
dotnet add package FluentUI.Blazor.Community.Components
```

### Register Services
Add the following in `Program.cs`

```csharp
builder.Services.AddFluentCxUIComponents();
```

If you're running your application on **Blazor Server**, make sure a default `HttpClient` is registered before the `AddFluentCxUIComponents` method.

```csharp
builder.Services.AddHttpClient();
```

## Usage
With the package installed, you can begin using the **Fluent UI Community Extension** Razor components in the same way as any other Razor component. 

### Add Imports

After the package is added, you need to add the following in your  `_Imports.razor`

```razor
@using FluentUI.Blazor.Community.Components
```

### Quick Start
This is literally all you need in your views to use Fluent UI Community Extension library components in your Blazor application.

```razor
<FluentCxResizer>
    <FluentCard>
        Hello World
    </FluentCard>
</FluentCxResizer>
```

## Blazor Hybrid
We haven't tested the library with Blazor Hybrid apps, but it should work as expected.
