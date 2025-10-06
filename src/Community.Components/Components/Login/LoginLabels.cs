namespace FluentUI.Blazor.Community.Components;

public record LoginLabels
{
    public static LoginLabels Default { get; } = new LoginLabels();

    public static LoginLabels French { get; } = new LoginLabels
    {
        Login = "Connexion",
        Logout = "Déconnexion",
        Register = "Inscription",
        UserName = "Nom d'utilisateur",
        Password = "Mot de passe",
        RememberMe = "Se souvenir de moi",
        ExternalLogin = "Ou utilisez un service externe pour vous connecter",
        ConnectWithProvider = "Se connecter avec {0}",
        SignInWithProvider = "S'inscire avec {0}"
    };

    public string Login { get; init; } = "Login";

    public string Logout { get; init; } = "Logout";

    public string Register { get; init; } = "Register";

    public string UserName { get; init; } = "User Name";

    public string Password { get; init; } = "Password";

    public string RememberMe { get; init; } = "Remember Me";

    public string ExternalLogin { get; init; } = "Or use an external service to login";

    public string ConnectWithProvider { get; init; } = "Connect with {0}";

    public string SignUpWithProvider { get; init; } = "Sign up with {0}";
}
