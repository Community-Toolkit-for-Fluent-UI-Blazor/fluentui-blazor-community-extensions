using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace FluentUI.Blazor.Community.Components.Security;

/// <summary>
/// Specifies that a property value must match the value of another property, typically used to confirm password fields
/// during validation.
/// </summary>
/// <remarks>Apply this attribute to a property to ensure its value is equal to the value of a specified
/// comparison property, such as confirming a password entry. The comparison is performed using the values of the two
/// properties at validation time. This attribute is commonly used in data models for user registration or password
/// reset scenarios to enforce that the confirmation password matches the original password. The property specified by
/// the constructor must exist on the validated object and should not be an indexer property. If the referenced property
/// is trimmed during linking, ensure it is preserved to avoid runtime errors.</remarks>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public sealed class PasswordCompareAttribute
    : ValidationAttribute
{
    /// <summary>
    /// Initializes a new instance of the PasswordCompareAttribute class using the specified property to compare
    /// against.
    /// </summary>
    /// <remarks>This constructor is typically used to specify the property that holds the original password
    /// value when performing a comparison for validation purposes. The referenced property must be preserved if code
    /// trimming is enabled, as it may be removed during trimming.</remarks>
    /// <param name="otherProperty">The name of the property to compare the password value with. Cannot be null.</param>
    [RequiresUnreferencedCode("The property referenced by 'otherProperty' may be trimmed. Ensure it is preserved.")]
    public PasswordCompareAttribute(string otherProperty) : base()
    {
        ArgumentNullException.ThrowIfNull(otherProperty);

        OtherProperty = otherProperty;
    }

    /// <summary>
    /// Gets the value of the other property.
    /// </summary>
    public string OtherProperty { get; }

    /// <summary>
    /// Gets the display name associated with another property.
    /// </summary>
    public string? OtherPropertyDisplayName { get; internal set; }

    /// <inheritdoc/>
    public override bool RequiresValidationContext => true;

    /// <inheritdoc/>
    [UnconditionalSuppressMessage("ReflectionAnalysis", "IL2072:UnrecognizedReflectionPattern",
           Justification = "The ctor is marked with RequiresUnreferencedCode informing the caller to preserve the other property.")]
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var otherPropertyInfo = validationContext.ObjectType.GetRuntimeProperty(OtherProperty);
        var textLocalization = validationContext.GetRequiredService<IRuleLocalization>();

        if (otherPropertyInfo == null)
        {
            throw new ArgumentException($"The property '{OtherProperty}' was not found on type '{validationContext.ObjectType.FullName}'.", nameof(OtherProperty));
        }

        if (otherPropertyInfo.GetIndexParameters().Length > 0)
        {
            throw new ArgumentException("The property was not found");
        }

        var otherPropertyValue = otherPropertyInfo.GetValue(validationContext.ObjectInstance, null);
        
        if (!Equals(value, otherPropertyValue))
        {
            OtherPropertyDisplayName ??= GetDisplayNameForProperty(otherPropertyInfo);

            string[]? memberNames = validationContext.MemberName != null
               ? [validationContext.MemberName]
               : null;

            return new ValidationResult(textLocalization.PasswordNotMatch(validationContext.DisplayName, OtherPropertyDisplayName ?? OtherProperty), memberNames);
        }

        return null;
    }

    /// <summary>
    /// Retrieves the display name for the specified property, using the <see cref="DisplayAttribute"/> if present.
    /// </summary>
    /// <remarks>If the property does not have a <see cref="DisplayAttribute"/>, the method returns a fallback
    /// value, which may be a default or alternative display name. This method does not validate whether the property is
    /// null; callers should ensure a valid <see cref="PropertyInfo"/> instance is provided.</remarks>
    /// <param name="property">The property for which to obtain the display name. Must not be null.</param>
    /// <returns>The display name defined by the <see cref="DisplayAttribute"/> if present; otherwise, a fallback value.</returns>
    private string? GetDisplayNameForProperty(PropertyInfo property)
    {
        var attributes = CustomAttributeExtensions.GetCustomAttributes(property, true);

        foreach (var attribute in attributes)
        {
            if (attribute is DisplayAttribute display)
            {
                return display.GetName();
            }
        }

        return OtherProperty;
    }
}
