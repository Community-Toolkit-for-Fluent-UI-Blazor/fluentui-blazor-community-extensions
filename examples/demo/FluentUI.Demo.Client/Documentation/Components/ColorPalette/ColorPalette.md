---
title: Color Palette
order: 0000
category: 30|Components
icon: Color
route: /ColorPalette
---

# Color Palette

The ColorPalette component is used to display a set of colors in a structured layout.
It can be useful for showcasing brand colors, theme colors, or any other color schemes in a visually appealing way.
You can provide a base color and the component will generate a harmonious color palette based on that color.
Many differents gradient strategies are available to choose from.
You can navigate through the colors using keyboard arrows and select one or multiple colors depending on the configuration.
A system of plugins is also available to extend the component's functionality.
If you use plugins, it could be very useful to use the `PaletteMode.None` mode.

## Examples

### Default

{{ ColorPaletteDefault }}

### Plugins

The component can be extended with custom plugins to provide additional color generation strategies. You can create your own plugin by implementing the `IColorPlugin` interface and provide an instance to your implementation via the `Plugins` parameter. Make sure to set the `Mode` parameter to `ColorPaletteMode.None` to prevent the default palette from being generated.

{{ ColorPalettePlugins }}

