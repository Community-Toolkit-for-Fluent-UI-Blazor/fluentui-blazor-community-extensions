namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the labels to use for the <see cref="ChatRoomListView"/>.
/// </summary>
public record ChatRoomLabels
{
    /// <summary>
    /// Gets the default labels.
    /// </summary>
    /// <remarks>
    /// The default labels are in english by default.
    /// </remarks>
    public static ChatRoomLabels Default { get; } = new();

    /// <summary>
    /// Gets the french label.
    /// </summary>
    public static ChatRoomLabels French { get; } = new()
    {
        ProgressLabel = "Chargement des salles ...",
        SearchPlaceholder = "Rechercher une salle de discussion",
        SearchLabel = "Search for a chat room",
        MaximumSelectedOptionsMessageLabel = "Le nombre maximal de créateurs pouvant être sélectionné a été atteint.",
        SuggestedCreators = "Créateurs ou créatrices suggérés",
        NoResultsFound = "Aucun résultat",
        NewGroup = "Créer un chat de groupe",
        EmptyRoomMessage = "<b>Il n'y a aucun message à lire</b>",
        UnreadPlural = "<b>{0} messages non lus</b>",
        UnreadSingular = "<b>{0} message non lu</b>",
        AudioSenderSingular = "Vous avez envoyé 1 fichier audio. <br />Cliquez sur le message pour le découvrir",
        AudioSenderPlural = "Vous avez envoyé {0} fichiers audio. <br />Cliquez sur le message pour les découvrir",
        AudioReceiverPlural = "Vous avez reçu {0} fichers audio de {1}.<br />Cliquez sur le message pour les découvrir",
        AudioReceiverSingular = "Vous avez reçu 1 fichier audio de {0}.<br />Cliquez sur le message pour le découvrir",
        VideoSenderSingular = "Vous avez envoyé 1 fichier vidéo. <br />Cliquez sur le message pour le découvrir",
        VideoSenderPlural = "Vous avez envoyé {0} fichiers vidéo. <br />Cliquez sur le message pour les découvrir",
        VideoReceiverPlural = "Vous avez reçu {0} fichers vidéo de {1}.<br />Cliquez sur le message pour les découvrir",
        VideoReceiverSingular = "Vous avez reçu 1 fichier vidéo de {0}.<br />Cliquez sur le message pour le découvrir",
        MediaSenderSingular = "Vous avez envoyé 1 fichier média. <br />Cliquez sur le message pour le découvrir",
        MediaSenderPlural = "Vous avez envoyé {0} fichiers média. <br />Cliquez sur le message pour les découvrir",
        MediaReceiverPlural = "Vous avez reçu {0} fichers média de {1}.<br />Cliquez sur le message pour les découvrir",
        MediaReceiverSingular = "Vous avez reçu 1 fichier média de {0}.<br />Cliquez sur le message pour le découvrir",
        PhotoSenderSingular = "Vous avez envoyé 1 fichier photo. <br />Cliquez sur le message pour le découvrir",
        PhotoSenderPlural = "Vous avez envoyé {0} fichiers photos. <br />Cliquez sur le message pour les découvrir",
        PhotoReceiverPlural = "Vous avez reçu {0} fichers photos de {1}.<br />Cliquez sur le message pour les découvrir",
        PhotoReceiverSingular = "Vous avez reçu 1 fichier photo de {0}.<br />Cliquez sur le message pour le découvrir",
        GiftSender = "Vous avez envoyé un cadeau.",
        GiftReceiver = "Vous avez reçu un cadeau de la part {0}.",
        UserSelectionDialogTitle = "Selectionnez les utilisateurs",
        SearchForUser = "Rechercher un utilisateur",
        ChatUserGroupMaxOptionsSelected = "Le nombre maximal d'utilisateurs pouvant être sélectionné a été atteint.",
        SuggestedUsers = "Utilisateurs suggérés",
        DialogClose = "Fermer",
        DialogOk = "Valider",
        SearchForUserPlaceholder = "Rechercher des utilisateurs ...",
        SuggestedRooms = "Salles suggérées",
        Block = "Bloquer",
        Delete = "Supprimer",
        Rename = "Renommer",
        DialogNo = "Non",
        DialogYes = "Oui",
        BlockRoomMessage = "Êtes-vous sûr de vouloir bloquer la salle de discussion ?",
        BlockRoomTitle = "Bloquer la salle de discussion",
        DeleteRoomMessage = "Êtes-vous sûr de vouloir supprimer la salle de discussion ?",
        DeleteRoomTitle = "Supprimer la salle de discussion",
        DialogCancel = "Annuler",
        RenameDialogTitle = "Renommer la salle",
        RenameLabel = "Renommer la salle",
        RenamePlaceholder = "Entrez le nom de la salle",
        Unblock = "Débloquer",
        UnblockRoomMessage = "Êtes-vous sûr de vouloir débloquer la salle de discussion ?",
        UnblockRoomTitle = "Débloquer la salle de discussion",
        Hide = "Masquer",
        Unhide = "Démasquer",
        HideRoomMessage = "Êtes-vous sûr de vouloir masquer la salle de discussion ?",
        HideRoomTitle = "Masquer la salle de discussion",
        UnhideRoomMessage = "Êtes-vous sûr de vouloir démasquer la salle de discussion ?",
        UnhideRoomTitle = "Démasquer la salle de discussion"
    };

    /// <summary>
    /// Gets or sets the progress label.
    /// </summary>
    public string ProgressLabel { get; set; } = "Loading rooms ...";

    /// <summary>
    /// Gets or sets the placeholder for searching a room.
    /// </summary>
    public string SearchPlaceholder { get; set; } = "Search for a chat room";

    /// <summary>
    /// Gets or sets the search label.
    /// </summary>
    public string SearchLabel { get; set; } = "Search for a chat room";

    /// <summary>
    /// Gets or sets the label when the maximum selected options has been reached.
    /// </summary>
    public string MaximumSelectedOptionsMessageLabel { get; set; } = "The maximum number of creators that can be selected has been reached.";

    /// <summary>
    /// Gets or sets the label for the title of the autocomplete result.
    /// </summary>
    public string SuggestedCreators { get; set; } = "Suggested creators";

    /// <summary>
    /// Gets or sets the label when the search completes with no result.
    /// </summary>
    public string NoResultsFound { get; set; } = "No results found";

    /// <summary>
    /// Gets or sets the label to create a new group.
    /// </summary>
    public string NewGroup { get; set; } = "Create a new chat group";

    /// <summary>
    /// Gets or sets the message for an empty room.
    /// </summary>
    public string EmptyRoomMessage { get; set; } = "<b>There are no messages to read.</b>";

    /// <summary>
    /// Gets or sets the label for an unread message.
    /// </summary>
    public string UnreadSingular { get; set; } = "<b>{0} unread message</b>";

    /// <summary>
    /// Gets or sets the label for many unread messages.
    /// </summary>
    public string UnreadPlural { get; set; } = "<b>{0} unread messages</b>";

    /// <summary>
    /// Gets or sets the label when an only audio file is present on the message.
    /// </summary>
    public string AudioSenderSingular { get; set; } = "You have sent 1 audio file. <br />Click on the message to discover it";

    /// <summary>
    /// Gets or sets the label when many audio files are present on the message.
    /// </summary>
    public string AudioSenderPlural { get; set; } = "You have sent {0} audio files. <br />Click on the message to discover them";

    /// <summary>
    /// Gets or sets the label for a receiver when he receives a single audio file.
    /// </summary>
    public string AudioReceiverSingular { get; set; } = "You have received 1 audio file from {0}.<br />Click on the message to discover it";

    /// <summary>
    /// Gets or sets the label for a receiver when he receives many audio files.
    /// </summary>
    public string AudioReceiverPlural { get; set; } = "You have received {0} audio files from {1}.<br />Click on the message to discover them";

    /// <summary>
    /// Gets or sets the label for a user which sends a single video file.
    /// </summary>
    public string VideoSenderSingular { get; set; } = "You have sent 1 video file. <br />Click on the message to discover it";

    /// <summary>
    /// Gets or sets the label for a user which sends many video files.
    /// </summary>
    public string VideoSenderPlural { get; set; } = "You have sent {0} video files. <br />Click on the message to discover them";

    /// <summary>
    /// Gets or sets the label for a receiver when he receives single video file.
    /// </summary>
    public string VideoReceiverSingular { get; set; } = "You have received 1 video file from {0}.<br />Click on the message to discover it";

    /// <summary>
    /// Gets or sets the label for a receiver when he receives many video files.
    /// </summary>
    public string VideoReceiverPlural { get; set; } = "You have received {0} video files from {1}.<br />Click on the message to discover them";

    /// <summary>
    /// Gets or sets the label for a user which sends a media file.
    /// </summary>
    public string MediaSenderSingular { get; set; } = "You have sent 1 media file. <br />Click on the message to discover it";

    /// <summary>
    /// Gets or sets the label for a user which sends many media files.
    /// </summary>
    public string MediaSenderPlural { get; set; } = "You have sent {0} media files. <br />Click on the message to discover them";

    /// <summary>
    /// Gets or sets the label for a receiver when he receives a media file.
    /// </summary>
    public string MediaReceiverSingular { get; set; } = "You have received 1 media file from {0}.<br />Click on the message to discover it";

    /// <summary>
    /// Gets or sets the label for a receiver when he receives many media files.
    /// </summary>
    public string MediaReceiverPlural { get; set; } = "You have received {0} media files from {1}.<br />Click on the message to discover them";

    /// <summary>
    /// Gets or sets the label for a user which sends a photo file.
    /// </summary>
    public string PhotoSenderSingular { get; set; } = "You have sent 1 photo file. <br />Click on the message to discover it";

    /// <summary>
    /// Gets or sets the label for a user which sends many photo files.
    /// </summary>
    public string PhotoSenderPlural { get; set; } = "You have sent {0} photo files. <br />Click on the message to discover them";

    /// <summary>
    /// Gets or sets the label for a receiver when he receives a photo file.
    /// </summary>
    public string PhotoReceiverSingular { get; set; } = "You have received 1 photo file from {0}.<br />Click on the message to discover it";

    /// <summary>
    /// Gets or sets the label for a receiver when he receives many photo files.
    /// </summary>
    public string PhotoReceiverPlural { get; set; } = "You have received {0} photo files from {1}.<br />Click on the message to discover them";

    /// <summary>
    /// Gets or sets the label for a user which sends a gift.
    /// </summary>
    public string GiftSender { get; set; } = "You have sent a gift.";

    /// <summary>
    /// Gets or sets the label for a user which receives a gift.
    /// </summary>
    public string GiftReceiver { get; set; } = "You have received a gift from {0}.";

    /// <summary>
    /// Gets or sets the title for the user selection dialog.
    /// </summary>
    public string UserSelectionDialogTitle { get; set; } = "Select users";

    /// <summary>
    /// Gets or sets the label for the search for user field.
    /// </summary>
    public string SearchForUser { get; set; } = "Search for a user";

    /// <summary>
    /// Gets or sets the placeholder for the search for user field.
    /// </summary>
    public string SearchForUserPlaceholder { get; set; } = "Enter the names of the users";

    /// <summary>
    /// Gets or sets the label when the maximum users for a group has been selected. 
    /// </summary>
    public string ChatUserGroupMaxOptionsSelected { get; set; } = "The maximum number of users that can be selected has been reached.";

    /// <summary>
    /// Gets or sets the title for suggested users.
    /// </summary>
    public string SuggestedUsers { get; set; } = "Suggested users";

    /// <summary>
    /// Gets or sets the label for OK.
    /// </summary>
    public string DialogOk { get; set; } = "OK";

    /// <summary>
    /// Gets or sets the label for Close.
    /// </summary>
    public string DialogClose { get; set; } = "Close";

    /// <summary>
    /// Gets or sets the title for suggested rooms.
    /// </summary>
    public string SuggestedRooms { get; set; } = "Suggested rooms";

    /// <summary>
    /// Gets or sets the label for Delete.
    /// </summary>
    public string Delete { get; set; } = "Delete";

    /// <summary>
    /// Gets or sets the label for Block.
    /// </summary>
    public string Block { get; set; } = "Block";

    /// <summary>
    /// Gets or sets the label for Unblock.
    /// </summary>
    public string Unblock { get; set; } = "Unblock";

    /// <summary>
    /// Gets or sets the label for Rename.
    /// </summary>
    public string Rename { get; set; } = "Rename";

    /// <summary>
    /// Gets or sets the label for Hide.
    /// </summary>
    public string Hide { get; set; } = "Hide";

    /// <summary>
    /// Gets or sets the label for Unhide.
    /// </summary>
    public string Unhide { get; set; } = "Unhide";

    /// <summary>
    /// Gets or sets the label for Yes.
    /// </summary>
    public string DialogYes { get; set; } = "Yes";

    /// <summary>
    /// Gets or sets the label for No.
    /// </summary>
    public string DialogNo { get; set; } = "No";

    /// <summary>
    /// Gets or sets the message for deleting a room.
    /// </summary>
    public string DeleteRoomMessage { get; set; } = "Are you sure you want to delete the chat room ?";

    /// <summary>
    /// Gets or sets the title to delete a room.
    /// </summary>
    public string DeleteRoomTitle { get; set; } = "Delete the chat room";

    /// <summary>
    /// Gets or sets the message for blocking a room.
    /// </summary>
    public string BlockRoomMessage { get; set; } = "Are you sure you want to block the chat room ?";

    /// <summary>
    /// Gets or sets the title to block a room.
    /// </summary>
    public string BlockRoomTitle { get; set; } = "Block the chat room";

    /// <summary>
    /// Gets or sets the label for the rename field.
    /// </summary>
    public string RenameLabel { get; set; } = "Rename the room";

    /// <summary>
    /// Gets or sets the placeholder for the rename field.
    /// </summary>
    public string RenamePlaceholder { get; set; } = "Enter the name of the room";

    /// <summary>
    /// Gets or sets the title to rename a room.
    /// </summary>
    public string RenameDialogTitle { get; set; } = "Rename the room";

    /// <summary>
    /// Gets or sets the label for Cancel.
    /// </summary>
    public string DialogCancel { get; set; } = "Cancel";

    /// <summary>
    /// Gets or sets the message to unblock a room.
    /// </summary>
    public string UnblockRoomMessage { get; set; } = "Are you sure you want to unblock the chat room ?";

    /// <summary>
    /// Gets or sets the title to unblock a room.
    /// </summary>
    public string UnblockRoomTitle { get; set; } = "Unblock the chat room";

    /// <summary>
    /// Gets or sets the title to hide a room.
    /// </summary>
    public string HideRoomTitle { get; set; } = "Hide the chat room";

    /// <summary>
    /// Gets or sets the message to hide a room.
    /// </summary>
    public string HideRoomMessage { get; set; } = "Are you sure you want to hide the chat room ?";

    /// <summary>
    /// Gets or sets the message to unhide a room.
    /// </summary>
    public string UnhideRoomMessage { get; set; } = "Are you sure you want to unhide the chat room ?";

    /// <summary>
    /// Gets or sets the title to unhide a room.
    /// </summary>
    public string UnhideRoomTitle { get; set; } = "Unhide the chat room";
}
