using System.Globalization;
using FluentUI.Blazor.Community.Components.Security;
using Microsoft.AspNetCore.Http;

namespace FluentUI.Demo.Shared.Infrastructure;

public class DynamicRuleLocalization : IRuleLocalization
{
    private readonly CultureInfo _culture;

    public DynamicRuleLocalization(IHttpContextAccessor accessor)
    {
        _culture = accessor.HttpContext?.Request.Headers["Accept-Language"].ToString() switch
        {
            var lang when lang.StartsWith("fr", StringComparison.OrdinalIgnoreCase) => new CultureInfo("fr-FR"),
            var lang when lang.StartsWith("en", StringComparison.OrdinalIgnoreCase) => new CultureInfo("en-US"),
            _ => CultureInfo.InvariantCulture
        };
    }

    public string ValueCannotBeNullOrEmpty => _culture.TwoLetterISOLanguageName switch
    {
        "fr" => "La valeur ne peut pas être nulle ou vide.",
        "en" => "Value cannot be null or empty.",
        _ => "Value cannot be null or empty."
    };

    public string IsNotValidEmail => _culture.TwoLetterISOLanguageName switch
    {
        "fr" => "L'adresse e-mail n'est pas valide.",
        "en" => "The email address is not valid.",
        _ => "The email address is not valid."
    };

    public string MinimumLength(int min) => _culture.TwoLetterISOLanguageName switch
    {
        "fr" => $"{min} caractères au moins.",
        "en" => $"Password must be at least {min} characters long.",
        _ => $"Minimum length: {min}"
    };

    public string MaximumLength(int max) => _culture.TwoLetterISOLanguageName switch
    {
        "fr" => $"{max} caractères au plus.",
        "en" => $"Password must not exceed {max} characters.",
        _ => $"Maximum length: {max}"
    };

    public string RequireLowercase(int count) => _culture.TwoLetterISOLanguageName switch
    {
        "fr" => $"{count} lettre(s) minuscule(s) au moins.",
        "en" => $"At least {count} lowercase letter(s) required.",
        _ => $"Lowercase required: {count}"
    };

    public string RequireUppercase(int count) => _culture.TwoLetterISOLanguageName switch
    {
        "fr" => $"{count} lettre(s) majuscule(s) au moins.",
        "en" => $"At least {count} uppercase letter(s) required.",
        _ => $"Uppercase required: {count}"
    };

    public string RequireDigit(int count) => _culture.TwoLetterISOLanguageName switch
    {
        "fr" => $"{count} chiffre(s) au moins.",
        "en" => $"At least {count} digit(s) required.",
        _ => $"Digits required: {count}"
    };

    public string RequireSpecial(int count) => _culture.TwoLetterISOLanguageName switch
    {
        "fr" => $"{count} caractère(s) spécial(aux) au moins.",
        "en" => $"At least {count} special character(s) required.",
        _ => $"Special characters required: {count}"
    };

    public string? PasswordNotMatch(params string[] args)
    {
        return _culture.TwoLetterISOLanguageName switch
        {
            "fr" => "Les mots de passe ne correspondent pas.",
            "en" => "Passwords do not match.",
            _ => "Passwords do not match."
        };
    }
}
