---
title: SleekDial
route: /SleekDial
icon: RadioButton
---

# SleekDial

The Sleek Dial component is a modern and visually appealing control that allows users to select a value from a range by rotating a dial. It provides an intuitive and interactive way to input values, making it ideal for scenarios such as volume control, brightness adjustment, or any other setting that benefits from a circular input method.
There is two modes : **Linear** and **Radial**.

The linear mode is a more traditional slider, while the radial mode is a circular dial that can be rotated to select a value.
The direction of the dial can be set to either clockwise or counterclockwise, depending on the desired user experience.

In **Linear** mode, you can restrict the area. This is useful if you want the dial to not overflow the parent container.
The popup has an intelligent positionning system that will try to find the best position to avoid overflowing the parent container or the window.
For example, if the dial is close to the right edge of the screen, the popup will be displayed on the left side of the dial to prevent it from overflowing the screen.

In **Radial** mode, you can set the start and end angles to define the range of the dial.
You can set the offset (radius) of the dial to control how far it is from the center point.
This allows for more customization and flexibility in how the dial is displayed and used.
You can also set the direction of the dial to either clockwise or counterclockwise, depending on the desired user experience.
And you can change the basic radial mode by changing the **SpiralMode**.


## Examples

### Default

{{ SleekDialExample }}
