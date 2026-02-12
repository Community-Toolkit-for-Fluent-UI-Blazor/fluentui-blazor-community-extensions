namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides static methods for calculating and configuring the layout of items within a sleek dial control, supporting
/// both linear and radial arrangements with customizable spacing, visibility, and animation settings.
/// </summary>
internal class SleekDialLayoutEngine
{
    /// <summary>
    /// Returns the default start and end angles, in degrees, associated with the specified floating position.
    /// </summary>
    /// <remarks>The returned angles define the angular range typically used for rendering or positioning
    /// elements based on the given floating position. If the position is not recognized, the method returns the full
    /// circle range (0 to 360 degrees).</remarks>
    /// <param name="position">The floating position for which to retrieve the default angle range.</param>
    /// <returns>A tuple containing the start and end angles, in degrees, that correspond to the specified floating position.</returns>
    private static (double Start, double End) GetDefaultAngles(FloatingPosition position)
    {
        return position switch
        {
            FloatingPosition.TopLeft => (0, 90),
            FloatingPosition.TopCenter => (0, 180),
            FloatingPosition.TopRight => (90, 180),

            FloatingPosition.MiddleLeft => (270, 90),
            FloatingPosition.MiddleCenter => (0, 360),
            FloatingPosition.MiddleRight => (90, 270),

            FloatingPosition.BottomLeft => (270, 360),
            FloatingPosition.BottomCenter => (180, 360),
            FloatingPosition.BottomRight => (180, 270),

            _ => (0, 360)
        };
    }

    /// <summary>
    /// Determines the linear direction for a dial based on the specified floating position.
    /// </summary>
    /// <param name="position">The floating position for which to determine the corresponding dial direction.</param>
    /// <returns>A value of type SleekDialLinearDirection that represents the direction associated with the given floating
    /// position.</returns>
    private static SleekDialLinearDirection GetDirection(FloatingPosition position)
    {
        return position switch
        {
            FloatingPosition.TopLeft or FloatingPosition.TopCenter or FloatingPosition.TopRight => SleekDialLinearDirection.Down,
            FloatingPosition.MiddleLeft => SleekDialLinearDirection.Right,
            FloatingPosition.MiddleCenter => SleekDialLinearDirection.Up,
            FloatingPosition.MiddleRight => SleekDialLinearDirection.Left,
            FloatingPosition.BottomLeft or FloatingPosition.BottomCenter or FloatingPosition.BottomRight => SleekDialLinearDirection.Up,

            _ => SleekDialLinearDirection.Up
        };
    }

    /// <summary>
    /// Calculates the layout for a set of items arranged in a linear fashion based on the specified direction, item size,
    /// settings, and container dimensions. The method determines the position of each item and the overall dimensions
    /// of the popup that contains them, taking into account spacing, gaps, and offsets defined in the settings.
    /// </summary>
    /// <param name="count">Number of items to place.</param>
    /// <param name="direction">The linear direction in which to arrange the items. If set to Default, the direction is determined based on the floating position.</param>
    /// <param name="itemSize">The size of each item in pixels.</param>
    /// <param name="settings">The settings that define spacing, gaps, and offsets for the linear layout.</param>
    /// <param name="position">The floating position that may influence the default direction and layout of the items.</param>
    /// <param name="width">The width of the container in which the items are arranged. If greater than 0, it overrides automatic width calculation.</param>
    /// <param name="height">The height of the container in which the items are arranged. If greater than 0, it overrides automatic height calculation.</param>
    /// <param name="fabWidth">The width of the floating action button (FAB) that may be used as a reference for layout calculations. If greater than 0, it influences popup dimensions.</param>
    /// <param name="fabHeight">The height of the floating action button (FAB) that may be used as a reference for layout calculations. If greater than 0, it influences popup dimensions.</param>
    /// <returns>A SleekDialLayoutResult containing the calculated positions for each item and the overall dimensions of the popup.</returns>
    private static SleekDialLayoutResult ComputeLinearLayout(
        int count,
        SleekDialLinearDirection direction,
        int itemSize,
        SleekDialLinearSettings settings,
        FloatingPosition position,
        int width,
        int height,
        int fabWidth,
        int fabHeight)
    {
        var spacing = ParseOffset(settings.ItemOffset);
        var layouts = new List<SleekDialItemLayout>(count);
        var realDirection = direction == SleekDialLinearDirection.Default ? GetDirection(position) : direction;
        var step = itemSize + spacing;
        var gap = ParseOffset(settings.Gap);

        int popupWidth;
        int popupHeight;

        if (width > 0 || height > 0)
        {
            popupWidth = width;
            popupHeight = height;
        }
        else
        {
            var itemsExtent = count > 0 ? count * itemSize + (count - 1) * spacing : 0;

            switch (realDirection)
            {
                case SleekDialLinearDirection.Up:
                case SleekDialLinearDirection.Down:
                    popupWidth = fabWidth > 0 ? fabWidth : itemSize;
                    popupHeight = itemsExtent + gap;
                    break;

                case SleekDialLinearDirection.Left:
                case SleekDialLinearDirection.Right:
                    popupWidth = itemsExtent + gap;
                    popupHeight = fabHeight > 0 ? fabHeight : itemSize;
                    break;

                default:
                    popupWidth = itemsExtent;
                    popupHeight = itemsExtent;
                    break;
            }
        }

        for (var i = 0; i < count; i++)
        {
            var layout = new SleekDialItemLayout();

            switch (realDirection)
            {
                case SleekDialLinearDirection.Up:
                case SleekDialLinearDirection.Down:
                    layout.X = (popupWidth - itemSize) / 2.0;
                    layout.Y = i * step;
                    break;

                case SleekDialLinearDirection.Left:
                case SleekDialLinearDirection.Right:
                    layout.X = i * step;
                    layout.Y = (popupHeight - itemSize) / 2.0;
                    break;
            }

            layouts.Add(layout);
        }

        return new SleekDialLayoutResult
        {
            Layouts = layouts,
            PopupWidth = popupWidth,
            PopupHeight = popupHeight
        };
    }

    /// <summary>
    /// Calculates the layout for a set of items arranged radially around a center point, based on the specified
    /// settings and position.
    /// </summary>
    /// <remarks>The method supports both full-circle and partial-arc arrangements, and accounts for clockwise
    /// or counterclockwise direction as specified in the settings. The returned layout ensures items are evenly
    /// distributed along the defined arc.</remarks>
    /// <param name="itemCount">The number of items to arrange in the radial layout. Must be zero or greater.</param>
    /// <param name="itemSize">The size, in pixels, of each item to be positioned in the layout.</param>
    /// <param name="settings">The radial layout settings that define angles, direction, and offset for item arrangement.</param>
    /// <param name="position">The reference position that determines the default start and end angles for the layout.</param>
    /// <returns>A <see cref="SleekDialLayoutResult" /> containing the calculated positions and dimensions for the radial layout.</returns>
    private static SleekDialLayoutResult ComputeRadialLayout(
        int itemCount,
        int itemSize,
        SleekDialRadialSettings settings,
        FloatingPosition position)
    {
        var list = new List<SleekDialItemLayout>(itemCount);

        var (defaultStart, defaultEnd) = GetDefaultAngles(position);
        var start = settings.StartAngle >= 0 ? settings.StartAngle : defaultStart;
        var end = settings.EndAngle >= 0 ? settings.EndAngle : defaultEnd;
        var radius = ParseOffset(settings.Offset);

        start = Norm(start);
        end = Norm(end);

        double sweep;

        if (settings.Direction == SleekDialRadialDirection.Clockwise)
        {
            sweep = (end - start + 360) % 360;
        }
        else
        {
            sweep = (start - end + 360) % 360;
        }

        var isFullCircle = Math.Abs(sweep) < 0.0001 || Math.Abs(sweep - 360) < 0.0001;

        if (isFullCircle)
        {
            sweep = 360;
        }

        double step = 0;

        if (itemCount > 0)
        {
            if (isFullCircle)
            {
                step = sweep / itemCount;
            }
            else
            {
                step = itemCount > 1 ? sweep / (itemCount - 1) : 0;
            }
        }

        for (var i = 0; i < itemCount; i++)
        {
            double angle;

            if (settings.Direction == SleekDialRadialDirection.Clockwise)
            {
                angle = isFullCircle
                    ? start + i * step
                    : start + i * step;
            }
            else
            {
                angle = isFullCircle
                    ? end - i * step
                    : end - i * step;
            }

            angle = Norm(angle);
            var rad = angle * Math.PI / 180.0;

            var layout = new SleekDialItemLayout
            {
                Angle = angle,
                Radius = radius,
                X = Math.Floor(radius + Math.Cos(rad) * radius - itemSize / 2),
                Y = Math.Floor(radius + Math.Sin(rad) * radius - itemSize / 2)
            };

            list.Add(layout);
        }

        return new()
        {
            Layouts = list,
            PopupWidth = radius * 2,
            PopupHeight = radius * 2
        };

        static double Norm(double a) => (a % 360 + 360) % 360;
    }

    /// <summary>
    /// Parses a CSS-like offset string and returns its value as an integer number of pixels.
    /// </summary>
    /// <remarks>Supported units include 'px', 'rem', 'em', 'cm', 'mm', 'in', 'pt', 'pc', and '%'. The
    /// conversion uses standard CSS unit-to-pixel ratios (e.g., 1rem = 16px, 1in = 96px). If the unit is unrecognized
    /// or omitted, the numeric value is interpreted as pixels.</remarks>
    /// <param name="offset">A string representing the offset, which may include a numeric value and an optional unit such as 'px', 'rem',
    /// 'em', 'cm', 'mm', 'in', 'pt', 'pc', or '%'. If the unit is omitted, pixels are assumed.</param>
    /// <returns>The offset value converted to pixels as an integer. Returns 0 if the input is null, empty, or cannot be parsed.</returns>
    private static int ParseOffset(string offset)
    {
        if (string.IsNullOrWhiteSpace(offset))
        {
            return 0;
        }

        offset = offset.Trim().ToLowerInvariant();

        var numberPart = new string([.. offset.TakeWhile(c => char.IsDigit(c) || c == '.' || c == '-')]);

        if (!double.TryParse(numberPart, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out var value))
        {
            return 0;
        }

        var unit = offset[numberPart.Length..].Trim();

        var result = unit switch
        {
            "" => value,
            "px" => value,
            "rem" => value * 16,
            "em" => value * 16,
            "cm" => value * 37.7952755906,
            "mm" => value * 3.77952755906,
            "in" => value * 96,
            "pt" => value * (96.0 / 72.0),
            "pc" => value * 16,
            "%" => value,
            _ => value
        };

        return (int)Math.Floor(result);
    }

    /// <summary>
    /// Applies a staggered animation delay to each item in the specified layout based on the provided animation
    /// settings.
    /// </summary>
    /// <remarks>If the stagger option in the animation settings is disabled, no changes are made to the
    /// layout items. When enabled, each item's animation delay is incremented by 40 milliseconds times its index in the
    /// list, starting from the base delay.</remarks>
    /// <param name="layout">The list of layout items to which staggered animation delays will be applied. Each item's animation delay is
    /// updated if staggering is enabled.</param>
    /// <param name="anim">The animation settings that determine whether staggering is applied and specify the base delay for the
    /// animation.</param>
    private static void ApplyStagger(List<SleekDialItemLayout> layout, SleekDialAnimationSettings anim)
    {
        if (!anim.Stagger)
        {
            return;
        }

        for (var i = 0; i < layout.Count; i++)
        {
            layout[i].AnimationDelay = anim.Delay + TimeSpan.FromMilliseconds(i * 40);
        }
    }

    /// <summary>
    /// Converts a SleekDialEasing enumeration value to its corresponding CSS easing function string.
    /// </summary>
    /// <remarks>If the specified easing value is not recognized, the method returns the CSS 'ease-out'
    /// function by default.</remarks>
    /// <param name="easing">The easing mode to convert to a CSS-compatible easing function.</param>
    /// <returns>A string representing the CSS easing function that corresponds to the specified SleekDialEasing value.</returns>
    private static string ToCssEasing(SleekDialEasing easing)
    {
        return easing switch
        {
            SleekDialEasing.Linear => "linear",
            SleekDialEasing.EaseIn => "ease-in",
            SleekDialEasing.EaseOut => "ease-out",
            SleekDialEasing.EaseInOut => "ease-in-out",
            SleekDialEasing.Spring => "cubic-bezier(0.25, 1.50, 0.5, 1.0)",
            _ => "ease-out"
        };
    }

    /// <summary>
    /// Applies the specified animation settings to each item in the provided layout, configuring their initial and
    /// final visual states based on the animation type and dial mode.
    /// </summary>
    /// <remarks>This method updates the transformation and opacity properties of each layout item to reflect
    /// the chosen animation. The resulting visual transitions depend on both the animation type and the current dial
    /// mode.</remarks>
    /// <param name="layout">The collection of layout items to which the animation settings will be applied. Each item's visual state is
    /// updated according to the animation parameters.</param>
    /// <param name="anim">The animation settings that define the type, duration, easing, and other properties to use when animating the
    /// layout items.</param>
    /// <param name="mode">The dial mode that determines how certain animations, such as slide or radial effects, are applied to the layout
    /// items.</param>
    /// <param name="linearDirection">The direction to use for linear animations, specifying the axis and orientation for slide effects when the dial
    /// is in linear mode.</param>
    private static void ApplyAnimation(
        List<SleekDialItemLayout> layout,
        SleekDialAnimationSettings anim,
        SleekDialMode mode,
        SleekDialLinearDirection linearDirection)
    {
        foreach (var item in layout)
        {
            var initialTransforms = new List<string>();
            var finalTransforms = new List<string>();

            item.InitialOpacity = 1;
            item.FinalOpacity = 1;

            switch (anim.Animation)
            {
                case SleekDialAnimationType.None:
                    break;

                case SleekDialAnimationType.Fade:
                    item.InitialOpacity = 0;
                    item.FinalOpacity = 1;
                    break;

                case SleekDialAnimationType.Scale:
                    initialTransforms.Add("scale(0.8)");
                    finalTransforms.Add("scale(1)");
                    break;

                case SleekDialAnimationType.Slide:
                    if (mode == SleekDialMode.Linear)
                    {
                        var slide = linearDirection switch
                        {
                            SleekDialLinearDirection.Up => "translateY(20px)",
                            SleekDialLinearDirection.Down => "translateY(-20px)",
                            SleekDialLinearDirection.Left => "translateX(20px)",
                            SleekDialLinearDirection.Right => "translateX(-20px)",
                            _ => "translateY(20px)"
                        };

                        initialTransforms.Add(slide);
                        finalTransforms.Add("translate(0,0)");
                    }

                    break;

                case SleekDialAnimationType.RadialSweep:
                    if (mode == SleekDialMode.Radial)
                    {
                        initialTransforms.Add($"rotate({item.Angle - 20}deg)");
                        finalTransforms.Add($"rotate({item.Angle}deg)");
                    }

                    break;

                case SleekDialAnimationType.Orbit:
                    if (mode == SleekDialMode.Radial)
                    {
                        initialTransforms.Add($"rotate({item.Angle + 10}deg)");
                        finalTransforms.Add($"rotate({item.Angle}deg)");
                    }

                    break;
            }

            item.Transform = string.Join(" ", initialTransforms);
            item.FinalTransform = string.Join(" ", finalTransforms);

            var easing = ToCssEasing(anim.Easing);

            item.Transition = $"all {anim.Duration.TotalMilliseconds}ms {easing} {item.AnimationDelay.TotalMilliseconds}ms";
        }
    }

    /// <summary>
    /// Calculates the layout for a collection of dial items based on the specified mode, settings, and container
    /// dimensions.
    /// </summary>
    /// <remarks>The layout calculation takes into account the selected mode and its corresponding settings.
    /// Only the relevant settings for the chosen mode are used; others are ignored. The returned result can be used to
    /// render the dial items with appropriate positioning and animation.</remarks>
    /// <param name="items">The list of dial items to arrange within the layout.</param>
    /// <param name="itemSize">The size, in pixels, of each individual dial item.</param>
    /// <param name="mode">The layout mode to use for arranging the items, such as linear or radial.</param>
    /// <param name="linearSettings">The settings that control the appearance and behavior of the linear layout mode. Ignored if the mode is not
    /// linear.</param>
    /// <param name="linearDirection">The direction in which items are arranged when using the linear layout mode. Ignored if the mode is not linear.</param>
    /// <param name="radialSettings">The settings that control the appearance and behavior of the radial layout mode. Ignored if the mode is not
    /// radial.</param>
    /// <param name="animation">The animation settings to apply to the layout transitions.</param>
    /// <param name="position">The position of the floating action button (FAB) within the container.</param>
    /// <param name="width">The width, in pixels, of the container in which the layout is rendered.</param>
    /// <param name="height">The height, in pixels, of the container in which the layout is rendered.</param>
    /// <param name="fabWidth">The width, in pixels, of the floating action button (FAB).</param>
    /// <param name="fabHeight">The height, in pixels, of the floating action button (FAB).</param>
    /// <returns>A result object containing the computed positions and animation data for each dial item in the layout.</returns>
    public static SleekDialLayoutResult ComputeLayout(
        List<SleekDialItem> items,
        int itemSize,
        SleekDialMode mode,
        SleekDialLinearSettings linearSettings,
        SleekDialLinearDirection linearDirection,
        SleekDialRadialSettings radialSettings,
        SleekDialAnimationSettings animation,
        FloatingPosition position,
        int width,
        int height,
        int fabWidth,
        int fabHeight)
    {
        var count = items.Count;

        var layout = mode switch
        {
            SleekDialMode.Linear => ComputeLinearLayout(
                count,
                linearDirection,
                itemSize,
                linearSettings,
                position,
                width,
                height,
                fabWidth,
                fabHeight),

            SleekDialMode.Radial => ComputeRadialLayout(
                count,
                itemSize,
                radialSettings,
                position),

            _ => new SleekDialLayoutResult()
        };

        ApplyStagger(layout.Layouts, animation);
        ApplyAnimation(layout.Layouts, animation, mode, linearDirection);

        return layout;
    }
}
