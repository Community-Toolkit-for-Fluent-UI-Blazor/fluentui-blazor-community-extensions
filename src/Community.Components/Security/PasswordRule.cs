using System.Security.Cryptography;
using System.Text;

namespace FluentUI.Blazor.Community.Components.Security;

/// <summary>
/// Represents the rules to generate a passowrd.
/// </summary>
public sealed class PasswordRule
{
    /// <summary>
    /// Type of rules allowed for the password.
    /// </summary>
    private enum PasswordRuleType
    {
        /// <summary>
        /// The password must have lowercase character.
        /// </summary>
        Lowercase,

        /// <summary>
        /// The password must have uppercase character.
        /// </summary>
        Uppercase,

        /// <summary>
        /// The password must have digit character.
        /// </summary>
        Digit,

        /// <summary>
        /// The password must have special character.
        /// </summary>
        Special
    }

    /// <summary>
    /// Represents the rules to generate a password.
    /// </summary>
    private readonly Dictionary<PasswordRuleType, string> _rules = new(EqualityComparer<PasswordRuleType>.Default)
    {
        [PasswordRuleType.Lowercase] = "abcdefghijklmnopqrstuvwxyz",
        [PasswordRuleType.Uppercase] = "ABCDEFGHIJKLMNOPQRSTUVWXYZ",
        [PasswordRuleType.Digit] = "0123456789",
        [PasswordRuleType.Special] = "!@#$%^&*()_+=[]{};:|./?,-\\\""
    };

    /// <summary>
    /// Represents the number of occurrence of each condition to respect the password predicate.
    /// </summary>
    private readonly Dictionary<PasswordRuleType, int> _ruleChars = new(EqualityComparer<PasswordRuleType>.Default);

    /// <summary>
    /// Represents the definition of the password rule.
    /// </summary>
    private readonly PasswordRuleDefinition _definition;

    /// <summary>
    /// Initializes a new instance of the <see cref="PasswordRule"/> class.
    /// </summary>
    /// <param name="minimumLength">Minimum length of the password.</param>
    /// <param name="maximumLength">Maximum length of the password.</param>
    /// <param name="minimumLowercaseCharacters">Minimum number of lowercase character in the password.</param>
    /// <param name="minimumUppercaseCharacters">Minimum number of uppercase character in the password.</param>
    /// <param name="minimumDigits">Minimum number of digits character in the password.</param>
    /// <param name="minimumSpecialCharacters">Minimum number of special character in the password.</param>
    public PasswordRule(
        int minimumLength,
        int maximumLength,
        int minimumLowercaseCharacters,
        int minimumUppercaseCharacters,
        int minimumDigits,
        int minimumSpecialCharacters)
        : this(new(minimumLength, maximumLength, minimumLowercaseCharacters, minimumUppercaseCharacters, minimumDigits, minimumSpecialCharacters))
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PasswordRule"/> class.
    /// </summary>
    /// <param name="passwordRuleDefinition">Definition of the password rule.</param>
    public PasswordRule(PasswordRuleDefinition passwordRuleDefinition)
    {
        _definition = passwordRuleDefinition;
        Reset();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PasswordRule"/> class.
    /// </summary>
    public PasswordRule()
        : this(8, 16, 1, 1, 1, 1)
    {
    }

    /// <summary>
    /// Gets the length of the password.
    /// </summary>
    /// <returns>Returns the length of the password.</returns>
    private int GetLength()
    {
        return Random.Shared.Next(_definition.MinimumLength, _definition.MaximumLength);
    }

    /// <summary>
    /// Generates a password with the defined rule.
    /// </summary>
    /// <returns>Returns the generated password.</returns>
    /// <exception cref="System.ArgumentOutOfRangeException">Exception occurs when the minimum length of the password is lower than
    ///  the sum of all conditions to match.</exception>
    public string GetPassword()
    {
        Reset();
        var length = GetLength();

        StringBuilder passwordBuilder = new();

        for (var i = 0; i < length; ++i)
        {
            if (_ruleChars.Sum(x => x.Value) == length - i)
            {
                var currentChar = _ruleChars.Where(x => x.Value > 0).Select(x => x.Key).ToArray();
                passwordBuilder.Append(GetCharFromRules(currentChar));
            }
            else
            {
                passwordBuilder.Append(GetCharFromRules());
            }
        }

        return passwordBuilder.ToString();
    }

    /// <summary>
    /// Evaluates the specified password and returns an array of errors that describe which password requirements are
    /// not met.
    /// </summary>
    /// <remarks>Use this method to determine which specific password rules are violated, such as minimum
    /// length, required character types, or other configured constraints. This can be useful for providing detailed
    /// feedback to users during password creation or update.</remarks>
    /// <param name="password">The password string to validate. Cannot be null.</param>
    /// <returns>An array of <see cref="PasswordError"/> objects representing each password rule that the specified password does
    /// not satisfy. The array is empty if the password meets all requirements.</returns>
    public PasswordError[] GetErrors(string password)
    {
        List<PasswordError> errors = [];

        if (string.IsNullOrEmpty(password) || password.Length < _definition.MinimumLength)
        {
            errors.Add(new("Password.MinimumLength", _definition.MinimumLength));
        }

        if (password.Length > _definition.MaximumLength)
        {
            errors.Add(new("Password.MaximumLength", _definition.MaximumLength));
        }

        if (_ruleChars[PasswordRuleType.Lowercase] > 0)
        {
            errors.Add(new("Password.LowerCase", _definition.MinimumLowercaseCharacters));
        }

        if (_ruleChars[PasswordRuleType.Uppercase] > 0)
        {
            errors.Add(new("Password.UpperCase", _definition.MinimumUppercaseCharacters));
        }

        if (_ruleChars[PasswordRuleType.Digit] > 0)
        {
            errors.Add(new("Password.Digit", _definition.MinimumDigits));
        }

        if (_ruleChars[PasswordRuleType.Special] > 0)
        {
            errors.Add(new("Password.Special", _definition.MinimumSpecialCharacters));
        }

        return [.. errors];
    }

    /// <summary>
    /// Determines whether the specified password meets all configured password rules.
    /// </summary>
    /// <remarks>The validation checks for the presence of digits, lowercase letters, uppercase letters, and
    /// special characters as defined by the configured rules. If the password contains any character not covered by
    /// these rules, the method returns <see langword="false"/>.</remarks>
    /// <param name="password">The password string to validate. Can be null or empty, in which case the method returns <see langword="false"/>.</param>
    /// <returns>A value indicating whether the password satisfies all required rules. Returns <see langword="true"/> if the
    /// password is valid; otherwise, <see langword="false"/>.</returns>
    public bool IsValid(string? password)
    {
        if (string.IsNullOrEmpty(password))
        {
            return false;
        }

        Reset();

        foreach (var c in password)
        {
            if (char.IsDigit(c))
            {
                DecreaseRule(PasswordRuleType.Digit);
            }
            else if (char.IsLower(c))
            {
                DecreaseRule(PasswordRuleType.Lowercase);
            }
            else if (char.IsUpper(c))
            {
                DecreaseRule(PasswordRuleType.Uppercase);
            }
            else if (_rules[PasswordRuleType.Special].Contains(c))
            {
                DecreaseRule(PasswordRuleType.Special);
            }
            else
            {
                return false;
            }
        }

        return _ruleChars.All(x => x.Value == 0) &&
               password.Length >= _definition.MinimumLength &&
               password.Length <= _definition.MaximumLength;
    }

    /// <summary>
    /// Restore the rule to the default definition.
    /// </summary>
    private void Reset()
    {
        _ruleChars[PasswordRuleType.Digit] = _definition.MinimumDigits;
        _ruleChars[PasswordRuleType.Lowercase] = _definition.MinimumLowercaseCharacters;
        _ruleChars[PasswordRuleType.Uppercase] = _definition.MinimumUppercaseCharacters;
        _ruleChars[PasswordRuleType.Special] = _definition.MinimumSpecialCharacters;
    }

    /// <summary>
    /// Gets a character from the specified rules.
    /// </summary>
    /// <param name="rules">Allowed rules to use to get a character.</param>
    /// <returns>Returns a character get from the specified rules.</returns>
    private char GetCharFromRules(params PasswordRuleType[] rules)
    {
        var filter = rules.Length == 0 ? _rules : _rules.Where(x => rules.Contains(x.Key));
        var length = filter.Sum(x => x.Value.Length);
        var index = RandomNumberGenerator.GetInt32(length);
        var offset = 0;

        foreach (var item in filter)
        {
            if (index < offset + item.Value.Length)
            {
                DecreaseRule(item.Key);
                return item.Value[index - offset];
            }

            offset += item.Value.Length;
        }

        return new char();
    }

    /// <summary>
    /// Decrease the counter of the specified rule to allow the generator
    ///  to seed good value for the remaining characters of the password.
    /// </summary>
    /// <param name="key">Key of the value to decrease.</param>
    private void DecreaseRule(PasswordRuleType key)
    {
        if (_ruleChars[key] > 0)
        {
            _ruleChars[key]--;
        }
    }
}
