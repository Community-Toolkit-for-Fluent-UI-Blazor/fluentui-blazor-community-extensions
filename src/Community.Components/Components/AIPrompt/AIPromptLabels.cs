namespace FluentUI.Blazor.Community.Components;

public record AIPromptLabels
{
    public static AIPromptLabels Default { get; } = new AIPromptLabels();

    public static AIPromptLabels French { get; } = new AIPromptLabels()
    {
        SystemPrompt = "Je suis un robot de discussion conçu pour assister les utilisateurs. Mon objectif est de fournir des informations utiles, précises et appropriées au contexte de manière claire et concise. Les sujets nuisibles, illégaux ou inappropriés sont interdits.",
        OutputResultTitle = "Généré par AI :",
        Generate = "Générer",
        CopyLabel = "Copier",
        RetryLabel = "Réessayer",
        PlaceholderLabel = "Bonjour ! Comment puis-je vous aider, aujourd'hui ?",
        CopiedText = "Le texte a été copié",
        ResultLabel = "Résultats",
        AskAILabel = "Demandez-moi"
    };

    public string SystemPrompt { get; set; } = "I'm a chat bot design to assist users. My goal is to provide helpful, accurate, and contextually appropriate information in a clear and concise manner. Harmful, illegal, or inappropriate topics are prohibited.";

    public string OutputResultTitle { get; set; } = "Generated with AI :";

    public string Generate { get; set; } = "Generate";

    public string PlaceholderLabel { get; set; } = "Hello ! How can I help you, today ?";

    public string CopyLabel { get; set; } = "Copy";

    public string RetryLabel { get; set; } = "Retry";

    public string CopiedText { get; set; } = "The text was successfully copied";

    public string AskAILabel { get; set; } = "Ask me";

    public string ResultLabel { get; set; } = "Results";
}
