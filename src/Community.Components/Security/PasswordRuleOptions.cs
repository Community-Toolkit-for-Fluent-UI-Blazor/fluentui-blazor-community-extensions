using Microsoft.Extensions.Configuration;

namespace FluentUI.Blazor.Community.Components.Security;

/// <summary>
/// Provides access to password rule configuration options, such as minimum length and character requirements, as
/// defined in the application's security settings.
/// </summary>
/// <remarks>Password rule values are retrieved from the 'Application:Security:PasswordRules' section of the
/// provided configuration. If a specific rule is not set in the configuration, its corresponding property will return
/// null.</remarks>
/// <param name="configuration">The configuration source from which password rule options are read. Must not be null.</param>
internal sealed class PasswordRuleOptions(IConfiguration configuration)
    : IPasswordRuleOptions
{
    /// <summary>
    /// References the configuration section containing password rule settings.
    /// </summary>
    private readonly IConfigurationSection _section = configuration.GetSection("Application:Security:PasswordRules");

    /// <inheritdoc />
    public int? MinimumLength => _section.GetValue<int?>(nameof(MinimumLength), null);

    /// <inheritdoc />
    public int? MaximumLength => _section.GetValue<int?>(nameof(MaximumLength), null);

    /// <inheritdoc />
    public int? MinimumLowercaseCharacters => _section.GetValue<int?>(nameof(MinimumLowercaseCharacters), null);

    /// <inheritdoc />
    public int? MinimumUppercaseCharacters => _section.GetValue<int?>(nameof(MinimumUppercaseCharacters), null);

    /// <inheritdoc />
    public int? MinimumDigits => _section.GetValue<int?>(nameof(MinimumDigits), null);

    /// <inheritdoc />
    public int? MinimumSpecialCharacters => _section.GetValue<int?>(nameof(MinimumSpecialCharacters), null);
}
