---
title: Migration ColorPalette
route: /Migration/ColorPalette
hidden: true
---

The component has gone through some significant changes in v5.

### New features

- The component now features all new properties from `Microsoft.FluentUI.AspNetCore.Components` v5
- Localization support has been added to the component. You can now provide translations for all text in the component via the `IFluentLocalizer` interface.
- The component now supports text colors based on the background color to ensure sufficient contrast.

### Removed features 💥

- It is no longer possible to provide CSS color names as input for the component. Please use the corresponding hex code instead. For example, instead of `Red`, use `#FF0000`.
- `ColorPaletteMode.FromImage` has been removed to remove third party dependencies. If you want to use this in v5 you'll need to provide a custom plugin via the `IColorPlugin` interface.


### Removed properties 💥

- `HarmonyLabel`
- `PresetLabel`
- `Columns` the component will now render a responsive grid by default.
- `Presets` use `ProvidedColors` instead
- `SelectedPreset`
- `ShowHarmonyPicker` provide harmory from outside of the component. Please refer to our default demo.
- `ImageDataProvider` has been removed in favour of providing the colors via `IColorPlugin` interface

