namespace FluentUI.Blazor.Community.Components.Tests.Components.Login;

public class AccountLabelsTests
{
    [Fact]
    public void Default_Instance_Has_Default_Texts()
    {
        var labels = new AccountLabels();

        Assert.Equal("Login", labels.Login);
        Assert.Equal("Password", labels.Password);
        Assert.Equal("Email", labels.Email);
        Assert.Equal("Send Instructions", labels.SendInstructions);
        Assert.Equal("Reset Password", labels.ResetPassword);
        Assert.Equal("Invalid Credentials", labels.InvalidCredentials);
    }

    [Fact]
    public void French_Instance_Contains_French_Translations()
    {
        var french = AccountLabels.French;

        Assert.Equal("Connexion", french.Login);
        Assert.Equal("Mot de passe", french.Password);
        Assert.Equal("Adresse courriel", french.Email);
        Assert.Equal("Envoyer les instructions", french.SendInstructions); // note: key translated in file
        Assert.Equal("Réinitialiser le mot de passe", french.ResetPassword);
        Assert.Equal("Identifiants invalides", french.InvalidCredentials);
    }

    [Fact]
    public void Can_Create_Custom_Instance_With_Init()
    {
        var custom = new AccountLabels
        {
            Login = "ログインする",
            Email = "メール"
        };

        Assert.Equal("ログインする", custom.Login);
        Assert.Equal("メール", custom.Email);
    }
}
