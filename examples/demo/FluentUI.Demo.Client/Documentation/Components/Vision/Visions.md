---
title: Vision
icon: Eye
route: /Visions
---

# Visions

The ColorImageTransform class allows you to apply a color transformation to an image,
such as changing the vision of the image to simulate different types of color blindness.
The sample use the Microsoft SkiaSharp library to extract the color byte array from the image,
and then apply the color transformation to the byte array before creating a new image with the transformed colors.

In the sample, you can change the IccProfile and the ColorVisionType to see how the image is transformed based on different color vision types, such as Protanopia, Deuteranopia, and Tritanopia.

## Examples

{{ VisionExample }}



