using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Bunit;
using FluentUI.Blazor.Community.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.DataGrid.Infrastructure;
using Microsoft.JSInterop;
using Xunit;

namespace FluentUI.Blazor.Community.Tests.Components.Base;

public class ComponentBaseTests : BunitContext
{
    /// <summary>
    /// List of components to exclude from the test.
    /// </summary>
    private static readonly Type[] Excluded =
    [
        typeof(FluentUI.Blazor.Community.Components._Imports),
        typeof(FluentUI.Blazor.Community.Components.FluentCxDeviceDetector), // Renders no HTML output
        typeof(CookiePolicyEntryView),               // Renders no HTML output
        typeof(ManageCookie),                        // Renders no HTML output,
        typeof(FluentCxCookie),                             // Renders no HTML output,
    ];

    /// <summary>
    /// List of customized actions to initialize the component with a specific type and optional required parameters.
    /// </summary>
    private static readonly Dictionary<Type, Loader> ComponentInitializer = new()
    {
        //{ typeof(FluentTimePicker<>), Loader.MakeGenericType(typeof(DateTime))},
        //{ typeof(FluentOverlay), Loader.Default.WithRequiredParameter("Visible", true)}
    };

    /// <summary />
    public ComponentBaseTests(ITestOutputHelper testOutputHelper)
    {
        Output = testOutputHelper;
        Services.AddFluentUIComponents();
    }

    /// <summary>
    /// Gets the test output helper.
    /// </summary>
    public ITestOutputHelper Output { get; }

    /// <summary>
    /// Test to verify that all FluentUI components implement the default properties (Id, Class, Style)
    /// from <see cref="FluentComponentBase"/>.
    ///
    /// ⚠️ DO NOT CHANGE THE FOLLOWING TEST.
    /// </summary>
    /// <param name="blazor">Blazor Component property name/value</param>
    /// <param name="html">Expected HTML attribute name/value</param>
    [Theory]
    [InlineData("Id='id='My-Specific-ID'", "id='My-Specific-ID'")]
    [InlineData("Class='My-Specific-Item'", "class='My-Specific-Item'")]
    [InlineData("Style='My-Specific-Style'", "style='My-Specific-Style'")]
    [InlineData("Margin='10px'", "style='margin: 10px;*'")]                                                 // `*` is required to accept `;` in the Regex
    [InlineData("Padding='10px'", "style='padding: 10px;*'")]                                               // `*` is required to accept `;` in the Regex
    [InlineData("Margin='my-margin'", "class='my-margin'")]
    [InlineData("Padding='my-padding'", "class='my-padding'")]
    [InlineData("extra-attribute='My-Specific-Attribute'", "extra-attribute='My-Specific-Attribute'")]      // AdditionalAttributes
    public void ComponentBase_DefaultProperties(string blazor, string html)
    {
        var errors = new StringBuilder();
        var blazorAttribute = ParseHtmlAttribute(blazor);
        var htmlAttribute = ParseHtmlAttribute(html);

        using var context = new DateTimeProviderContext(DateTime.Now);
        JSInterop.Mode = JSRuntimeMode.Loose;

        foreach (var componentType in BaseHelpers.GetDerivedTypes<IFluentComponentBase>(except: Excluded))
        {
            // Convert to generic type if needed
            var type = ComponentInitializer.TryGetValue(componentType, out var value)
                     ? value.ComponentType(componentType)
                     : componentType;

            // Arrange and Act
            try
            {
                var renderedComponent = Render<DynamicComponent>(parameters =>
                {
                    parameters.Add(p => p.Type, type);

                    // Required parameters
                    parameters.Add(p => p.Parameters, DictionaryExtensions.Union(
                        new Dictionary<string, object>
                        {
                            { blazorAttribute.Name, blazorAttribute.Value }
                        },
                        ComponentInitializer.TryGetValue(componentType, out var valueRequired) ? valueRequired.RequiredParameters : null
                    ));

                    // Cascading values
                    if (ComponentInitializer.TryGetValue(componentType, out var valueCascading))
                    {
                        foreach (var (Name, Value) in valueCascading.CascadingValues)
                        {
                            if (string.IsNullOrEmpty(Name))
                            {
                                parameters.AddCascadingValue(Value);
                            }
                            else
                            {
                                parameters.AddCascadingValue(Name, Value);
                            }
                        }
                    }
                });

                // Assert
                var isMatch = renderedComponent.Markup.ContainsAttribute(htmlAttribute.Name, htmlAttribute.Value);

                Output.WriteLine($"{(isMatch ? "✅" : "❌")} {componentType.Name}");

                if (!isMatch)
                {
                    var error = $"\"{componentType.Name}\" does not use the \"{blazorAttribute.Name}\" property/attribute (missing HTML attribute {htmlAttribute.Name}=\"{htmlAttribute.Value}\").";
                    errors.AppendLine(error);
                }
            }
            catch (Exception ex)
            {
                var error = $"Error rendering component {componentType?.Name}. Update the `ComponentInitializer` dictionary: {Environment.NewLine}{Environment.NewLine}{ex.Message}{Environment.NewLine}{Environment.NewLine}{ex.InnerException?.Message}";
                errors.AppendLine(error);
            }
        }

        Assert.True(errors.Length == 0, errors.ToString());
    }

    // Helper method to parse HTML attributes
    private static (string Name, string Value) ParseHtmlAttribute(string attributeString)
    {
        var parts = attributeString.Split(['='], 2);
        if (parts.Length != 2)
        {
            throw new ArgumentException("Invalid attribute string format.", nameof(attributeString));
        }

        var name = parts[0].Trim();
        var value = parts[1].Trim(' ', '"', '\'');

        return (name, value);
    }
    private class Loader
    {
        public static Loader MakeGenericType(Type type) => new()
        {
            ComponentType = t => t.MakeGenericType(type)
        };

        public static Loader Default => new();

        private Loader() { }

        public Func<Type, Type> ComponentType { get; private set; } = t => t;

        public Dictionary<string, object> RequiredParameters { get; } = [];

        public List<(string? Name, object Value)> CascadingValues { get; } = [];

        public Loader WithRequiredParameter(string key, object value)
        {
            RequiredParameters.Add(key, value);
            return this;
        }

        public Loader WithCascadingValue<TValue>(string? name, TValue cascadingValue) where TValue : notnull
        {
            CascadingValues.Add((name, cascadingValue));
            return this;
        }

        public Loader WithCascadingValue<TValue>(TValue cascadingValue) where TValue : notnull
        {
            return WithCascadingValue(null, cascadingValue);
        }
    }

    private static class DictionaryExtensions
    {
        public static Dictionary<string, object> Union(Dictionary<string, object> first, Dictionary<string, object>? second)
        {
            if (second == null)
            {
                return first;
            }

            return first.Concat(second)
                .GroupBy(kvp => kvp.Key)
                .ToDictionary(g => g.Key, g => g.Last().Value);
        }
    }
}
