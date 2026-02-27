namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides factory methods for creating predefined signature theme definitions based on the specified theme type.
/// </summary>
/// <remarks>Use this class to obtain a configured instance of a signature theme definition for supported themes
/// such as Blueprint, Engineering, Notebook, DottedNotebook, and DarkGrid. The returned theme definition contains
/// visual settings appropriate for the selected theme. This class is intended for scenarios where consistent theme
/// configuration is required across signature components.</remarks>
internal static class SignatureThemeFactory
{
    /// <summary>
    /// Creates a new signature theme definition based on the specified theme.
    /// </summary>
    /// <remarks>Use this method to obtain a preconfigured theme definition for rendering signature grids with
    /// consistent appearance. Each theme provides distinct colors, grid settings, and display modes suitable for
    /// different signature scenarios.</remarks>
    /// <param name="theme">The theme to use for generating the signature theme definition. Determines the visual style and grid
    /// configuration applied.</param>
    /// <returns>A signature theme definition configured according to the specified theme. If the theme is not recognized, a
    /// default definition is returned.</returns>
    public static SignatureThemeDefinition Create(SignatureTheme theme)
    => theme switch
    {
        SignatureTheme.Blueprint => new SignatureThemeDefinition
        {
            Background = new SignatureBackgroundTheme
            {
                Color = "#0a1a2f",
                Opacity = 1.0
            },

            Grid = new SignatureGridTheme
            {
                Color = "#4da6ff",
                Opacity = 0.8,
                CellSize = 25,
                BoldEvery = 5,
                StrokeWidth = 1.2
            },

            Axes = new SignatureAxesTheme
            {
                Color = "#ffffff",
                Opacity = 0.9,
                StrokeWidth = 2,
                DashArray = null
            }
        },

        SignatureTheme.Engineering => new SignatureThemeDefinition
        {
            Background = new SignatureBackgroundTheme
            {
                Color = "white",
                Opacity = 1.0
            },

            Grid = new SignatureGridTheme
            {
                Color = "#d0d0d0",
                Opacity = 1.0,
                CellSize = 10,
                BoldEvery = 10,
                StrokeWidth = 0.8
            },

            Axes = new SignatureAxesTheme
            {
                Color = "#000000",
                Opacity = 1.0,
                StrokeWidth = 1.5,
                DashArray = "4,2"
            }
        },

        SignatureTheme.Notebook => new SignatureThemeDefinition
        {
            Background = new SignatureBackgroundTheme
            {
                Color = "white",
                Opacity = 1.0
            },

            Grid = new SignatureGridTheme
            {
                Color = "#b0c4de",
                Opacity = 0.7,
                CellSize = 20,
                BoldEvery = 0,
                DashArray = "4,2"
            },

            Axes = new SignatureAxesTheme
            {
                Color = "#000000",
                Opacity = 1.0,
                StrokeWidth = 2,
                DashArray = null
            }
        },

        SignatureTheme.DottedNotebook => new SignatureThemeDefinition
        {
            Background = new SignatureBackgroundTheme
            {
                Color = "white",
                Opacity = 1.0
            },

            Grid = new SignatureGridTheme
            {
                Color = "#bbbbbb",
                Opacity = 0.9,
                CellSize = 20,
                DisplayMode = GridDisplayMode.Dots,
                PointRadius = 1.2
            },

            Axes = new SignatureAxesTheme
            {
                Color = "#000000",
                Opacity = 1.0,
                StrokeWidth = 2,
                DashArray = null
            }
        },

        SignatureTheme.DarkGrid => new SignatureThemeDefinition
        {
            Background = new SignatureBackgroundTheme
            {
                Color = "#1e1e1e",
                Opacity = 1.0
            },

            Grid = new SignatureGridTheme
            {
                Color = "#444444",
                Opacity = 0.6,
                CellSize = 20,
                BoldEvery = 5,
                StrokeWidth = 1
            },

            Axes = new SignatureAxesTheme
            {
                Color = "#ffffff",
                Opacity = 0.8,
                StrokeWidth = 2,
                DashArray = null
            }
        },

        SignatureTheme.BlueprintDark => new SignatureThemeDefinition
        {
            Background = new SignatureBackgroundTheme
            {
                Color = "#071423",
                Opacity = 1.0
            },

            Grid = new SignatureGridTheme
            {
                Color = "#3c8dd9",
                Opacity = 0.7,
                CellSize = 25,
                BoldEvery = 5,
                StrokeWidth = 1.2
            },

            Axes = new SignatureAxesTheme
            {
                Color = "#ffffff",
                Opacity = 0.9,
                StrokeWidth = 2,
                DashArray = null
            }
        },

        SignatureTheme.EngineeringPaper => new SignatureThemeDefinition
        {
            Background = new SignatureBackgroundTheme
            {
                Color = "#fdfdfd",
                Opacity = 1.0
            },

            Grid = new SignatureGridTheme
            {
                Color = "#c8e0c8",
                Opacity = 1.0,
                CellSize = 5,
                BoldEvery = 10,
                StrokeWidth = 0.6
            },

            Axes = new SignatureAxesTheme
            {
                Color = "#006600",
                Opacity = 1.0,
                StrokeWidth = 1.5,
                DashArray = null
            }
        },

        SignatureTheme.FrenchNotebook => new SignatureThemeDefinition
        {
            Background = new SignatureBackgroundTheme
            {
                Color = "white",
                Opacity = 1.0
            },

            Grid = new SignatureGridTheme
            {
                Color = "#7aa0d8",
                Opacity = 0.8,
                CellSize = 20,
                BoldEvery = 0,
                StrokeWidth = 1
            },

            Axes = new SignatureAxesTheme
            {
                Color = "#d9534f", // rouge cahier
                Opacity = 1.0,
                StrokeWidth = 2,
                DashArray = null
            }
        },

        SignatureTheme.GraphPaper => new SignatureThemeDefinition
        {
            Background = new SignatureBackgroundTheme
            {
                Color = "white",
                Opacity = 1.0
            },

            Grid = new SignatureGridTheme
            {
                Color = "#a0a0a0",
                Opacity = 0.6,
                CellSize = 10,
                BoldEvery = 5,
                StrokeWidth = 0.8
            },

            Axes = new SignatureAxesTheme
            {
                Color = "#000000",
                Opacity = 1.0,
                StrokeWidth = 1.5,
                DashArray = null
            }
        },

        SignatureTheme.DarkBlueprintNeon => new SignatureThemeDefinition
        {
            Background = new SignatureBackgroundTheme
            {
                Color = "#0b0b0b",
                Opacity = 1.0
            },

            Grid = new SignatureGridTheme
            {
                Color = "#00eaff",
                Opacity = 0.4,
                CellSize = 20,
                BoldEvery = 4,
                StrokeWidth = 1
            },

            Axes = new SignatureAxesTheme
            {
                Color = "#ff00ff",
                Opacity = 1.0,
                StrokeWidth = 2,
                DashArray = null
            }
        },

        SignatureTheme.Minimalist => new SignatureThemeDefinition
        {
            Background = new SignatureBackgroundTheme
            {
                Color = "white",
                Opacity = 1.0
            },

            Grid = new SignatureGridTheme
            {
                Color = "#e0e0e0",
                Opacity = 0.3,
                CellSize = 25,
                BoldEvery = 0,
                StrokeWidth = 0.5
            },

            Axes = new SignatureAxesTheme
            {
                Color = "#000000",
                Opacity = 0.2,
                StrokeWidth = 1,
                DashArray = null
            }
        },
        _ => new SignatureThemeDefinition()
    };

}
