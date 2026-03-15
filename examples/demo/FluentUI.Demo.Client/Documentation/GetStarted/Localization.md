---
title: Localization
order: 0005
category: 10|Get Started
route: /localization
icon: LocalLanguage
---

# Localization

Localization allows the text in some components to be translated. The FluentUI Blazor Community components use the native translation support provided by the official components.

## Explanation

The **FluentUI Blazor Community Components** have their own custom implementation of `IFluentLocalizer`. Every community resource has the prefix `CX` to distinguish it from the official **FluentUI Blazor** translations.

If you are only using the default translations for **FluentUI Blazor**, you only need to register the `FluentCxLocalizer` in your `Program.cs`:

```csharp
builder.Services.AddFluentUIComponents(config =>
{
    config.Localizer = new FluentCxLocalizer();
});
```
If you want to provide custom translations for either the official components or the community components, you must inherit from `FluentCxLocalizer` and override the corresponding logic to include your translations. You can find the current source code for `FluentCxLocalizer` in our [GitHub Repository](https://github.com/Community-Toolkit-for-Fluent-UI-Blazor/fluentui-blazor-community-extensions/blob/dev-v5/src/Core/Localization/FluentCxLocalizer.cs)

> [!NOTE]   
> The list of keys can be found in the `Core\Localization\LanguageResource.resx` file.  
> Or you can use a constant from the `FluentUI.Blazor.Community.Components.Localization.LanguageResource` class.
> Example: `Localization.LanguageResource.CX_ColorPalette_CustomGradient_Error`.
