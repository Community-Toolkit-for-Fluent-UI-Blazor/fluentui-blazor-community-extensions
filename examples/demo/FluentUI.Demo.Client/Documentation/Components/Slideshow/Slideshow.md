---
title: Slideshow
icon: SlideTransition
route: /Slideshow
---

# Slideshow

The slideshow component allows you to create a slideshow of images with various transition effects.
You can use video content as well.

## Examples

### Default Slideshow

This example demonstrates how to implement a slideshow component using Fluent UI in a Blazor application.
By default, we will use the <code>ChildContent</code> property.

If <code>LoopMode</code> is set to <code>Infinite</code>, the indicators will not be rendered, as they are not applicable in this mode.
The <code>Orientation</code> property works only when <code>ShowIndicators</code> is set to false. In other modes, the orientation will be set by the <code>IndicatorPosition</code> property.

{{ SlideshowExample }}

### Slideshow with Caption

This example demonstrates how to implement a slideshow component using Fluent UI in a Blazor application.
Here, we will use the <code>ItemTemplate</code> property to define the content of each slide, and the <code>Items</code> property to provide the data for the slideshow.

{{ SlideshowWithCaptionExample }}

## API Resizer

{{ API Type=Slideshow }}
