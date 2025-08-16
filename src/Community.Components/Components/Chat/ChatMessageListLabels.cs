namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents all the labels for the <see cref="ChatMessageListView{TItem}"/>.
/// </summary>
public record ChatMessageListLabels
{
    /// <summary>
    /// Represents the default labels.
    /// </summary>
    /// <remarks>The default labels are in english.</remarks>
    public static ChatMessageListLabels Default { get; } = new();

    /// <summary>
    /// Represents the french labels.
    /// </summary>
    public static ChatMessageListLabels French { get; } = new()
    {
        LoadingLabel = "Chargement des messages ...",
        EmptyRoomMessage = "Vous avez commencé une discussion avec {0}.<br />Les créateurs et les créatrices sont des personnes avant tout.<br />Les messages doivent être respectueux...",
        EmptyRoomWelcome = "Bienvenue de la part de {0} 😀",
        EmptyRoomLetsTalk = "Discutons",
        EnterMessage = "Entrez votre message",
        InsertMedia = "Ajouter des médias (audio, vidéo, images)",
        InsertEmojis = "Ajouter des émojis",
        InsertGift = "Ajouter des cadeaux",
        SendMessage = "Envoyer le message",
        DialogCancel = "Annuler",
        SelectFromCloudDriveLabel = "Sélectionnez les médias à partir de votre disque cloud",
        SelectFromHardDriveLabel = "Sélectionnez les médias à partir de votre disque dur",
        DialogOk = "Valider",
        DragDropFileLabel = "Faites glisser les fichiers, que vous souhaitez télécharger, ici ou <label for=\"{0}\">sélectionnez</label> les<span style=\"color: red;\">*</span>.<br /><em>Maximum de {1} fichiers autorisés.</em>",
        Completed = "Terminé",
        Progression = "Chargement {0} - {1}",
        BlockMessage = "Cette salle de discussion a été bloquée par un utilisateur. Vous ne pouvez plus communiquer avec lui tant qu'il ne vous a pas débloqué",
        MessageTextCopied = "Le texte du message a bien été copié !",
        Copy = "Copier",
        Delete = "Supprimer",
        Edit = "Modifier",
        Unread = "Non lu",
        Read = "Lu",
        ReadByEveryone = "Lu par tout le monde",
        DeleteMessage = "Êtes-vous sûr de vouloir supprimer le message ?",
        DeleteTitle = "Supprimer le message",
        DialogNo = "Non",
        DialogYes = "Oui",
        Edited = "Modifié",
        PinLabel = "Épingler",
        UnpinLabel = "Désépingler",
        FileSelectorDialogTitle = "Importer des fichiers de",
        Reply = "Répondre",
        ReplyFromGiftOnly = "Message cadeau",
        ReplyFromDocumentOnly = "Message document (photos, audio, vidéos...)",
        SendingMessageLabel = "Message en cours d'envoi ...",
        ThisMessageWasDeleted = "Ce message a été supprimé.",
        MessageTextCopyFailed = "Echec de la copie du texte.",
        OwnerBlockMessage = "Vous avez bloqué cette salle de discussion. Pour continuer à discuter, vous devez débloquer la salle.",
        MessageViewer = "Visionneuse de messages",
        LiveAudioRecording = "Enregistrer un message audio",
        AudioProcessingLabel = "Traitement du message audio..."
    };

    /// <summary>
    /// Gets or sets the loading label.
    /// </summary>
    public string LoadingLabel { get; set; } = "Loading ...";

    /// <summary>
    /// Gets or sets the welcome message when the room is empty.
    /// </summary>
    public string EmptyRoomWelcome { get; set; } = "You started a discussion with {0}.<br />Creators are people first and foremost.<br />Messages should be respectful...";

    /// <summary>
    /// Gets or sets the title message when the room is empty.
    /// </summary>
    public string EmptyRoomMessage { get; set; } = "Welcome from {0} 😀";

    /// <summary>
    /// Gets or sets the message to start talking with a person.
    /// </summary>
    public string EmptyRoomLetsTalk { get; set; } = "Let's chat";

    /// <summary>
    /// Gets or sets the placeholder to enter a message.
    /// </summary>
    public string EnterMessage { get; set; } = "Enter your message ...";

    /// <summary>
    /// Gets or sets the title on the media button.
    /// </summary>
    public string InsertMedia { get; set; } = "Add media (images, videos, audios)";

    /// <summary>
    /// Gets or sets the title on the emoji button.
    /// </summary>
    public string InsertEmojis { get; set; } = "Add emojis";

    /// <summary>
    /// Gets or sets the title on the gift button.
    /// </summary>
    public string InsertGift { get; set; } = "Add a gift";

    /// <summary>
    /// Gets or sets the title on the send message button.
    /// </summary>
    public string SendMessage { get; set; } = "Send the message";

    /// <summary>
    /// Gets or sets the label to select files from the hard drive.
    /// </summary>
    public string SelectFromHardDriveLabel { get; set; } = "Select your media from your hard drive";

    /// <summary>
    /// Gets or sets the label to select files from the cloud drive.
    /// </summary>
    public string SelectFromCloudDriveLabel { get; set; } = "Select your media from your cloud drive";

    /// <summary>
    /// Gets or sets the cancel label.
    /// </summary>
    public string DialogCancel { get; set; } = "Cancel";

    /// <summary>
    /// Gets or sets the OK label.
    /// </summary>
    public string DialogOk { get; set; } = "Ok";

    /// <summary>
    /// Gets or sets the label for the dragdrop file zone.
    /// </summary>
    public string DragDropFileLabel { get; set; } = "Drag files here you wish to upload, or <label for=\"{0}\">browse</label> for them<span style=\"color: red;\">*</span>.<br /><em>Maximum of {1} files allowed.</em>";

    /// <summary>
    /// Gets or sets the label when an operation is completed.
    /// </summary>
    public string Completed { get; set; } = "Completed";

    /// <summary>
    /// Gets or sets the label for a progressive operation.
    /// </summary>
    public string Progression { get; set; } = "Loading {0} - {1}";

    /// <summary>
    /// Gets or sets the message of a blocked room (user view).
    /// </summary>
    public string BlockMessage { get; set; } = "This chat room has been blocked by a user... You cannot communicate until the user unblock you.";

    /// <summary>
    /// Gets or sets the message of a blocked room (owner view).
    /// </summary>
    public string OwnerBlockMessage { get; set; } = "You have blocked this chat room. To continue chatting, you need to unblock the room.";

    /// <summary>
    /// Gets or sets the message when a text is successfully copied.
    /// </summary>
    public string MessageTextCopied { get; set; } = "The text of the message has been copied !";

    /// <summary>
    /// Gets or sets the message when a copy has failed.
    /// </summary>
    public string MessageTextCopyFailed { get; set; } = "An error occured. The copy of the text has failed !";

    /// <summary>
    /// Gets or sets the label to copy the message.
    /// </summary>
    public string Copy { get; set; } = "Copy";

    /// <summary>
    /// Gets or sets the label to reply the message.
    /// </summary>
    public string Reply { get; set; } = "Reply";

    /// <summary>
    /// Gets or sets the label to delete the message.
    /// </summary>
    public string Delete { get; set; } = "Delete";

    /// <summary>
    /// Gets or sets the label to edit the message.
    /// </summary>
    public string Edit { get; set; } = "Edit";

    /// <summary>
    /// Gets or sets the label for an unread message.
    /// </summary>
    public string Unread { get; set; } = "Unread";

    /// <summary>
    /// Gets or sets the label for a read message.
    /// </summary>
    public string Read { get; set; } = "Read";

    /// <summary>
    /// Gets or sets the label for a message read by everyone.
    /// </summary>
    public string ReadByEveryone { get; set; } = "Read by everyone";

    /// <summary>
    /// Gets or sets the label to delete the message.
    /// </summary>
    public string DeleteMessage { get; set; } = "Are you sure you want to delete the message ?";

    /// <summary>
    /// Gets or sets the yes label.
    /// </summary>
    public string DialogYes { get; set; } = "Yes";

    /// <summary>
    /// Gets or sets the title to delete a message.
    /// </summary>
    public string DeleteTitle { get; set; } = "Delete the message";

    /// <summary>
    /// Gets or sets the no label.
    /// </summary>
    public string DialogNo { get; set; } = "No";

    /// <summary>
    /// Gets or sets the label for an edited message.
    /// </summary>
    public string Edited { get; set; } = "Edited";

    /// <summary>
    /// Gets or sets the label to pin a message.
    /// </summary>
    public string PinLabel { get; set; } = "Pin";

    /// <summary>
    /// Gets or sets the label to unpin a message.
    /// </summary>
    public string UnpinLabel { get; set; } = "Unpin";

    /// <summary>
    /// Gets or sets the label to import files.
    /// </summary>
    public string FileSelectorDialogTitle { get; set; } = "Import files from ...";

    /// <summary>
    /// Gets or sets the label when the replied message is a gift message.
    /// </summary>
    public string ReplyFromGiftOnly { get; set; } = "Gift message";

    /// <summary>
    /// Gets or sets the label when the replied message contains only documents.
    /// </summary>
    public string ReplyFromDocumentOnly { get; set; } = "Media message (audio, images, videos...)";

    /// <summary>
    /// Gets or sets the label when the message is sent but not received by the receiver.
    /// </summary>
    public string SendingMessageLabel { get; set; } = "Message being sent ...";

    /// <summary>
    /// Gets or sets the label when a deleted message is displayed.
    /// </summary>
    public string ThisMessageWasDeleted { get; set; } = "This message was deleted.";

    /// <summary>
    /// Gets or sets the label for the message viewer dialog.
    /// </summary>
    public string MessageViewer { get; set; } = "Message viewer";

    /// <summary>
    /// Gets or sets the label for the chat message viewer to record audio.
    /// </summary>
    public string LiveAudioRecording { get; set; } = "Record an audio message.";

    /// <summary>
    /// Gets or sets the label for the chat message viewer to process an audio record.
    /// </summary>
    public string AudioProcessingLabel { get; set; } = "Audio message processing...";
}
