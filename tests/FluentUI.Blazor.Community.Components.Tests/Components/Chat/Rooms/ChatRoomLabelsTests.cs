using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Chat;

public class ChatRoomLabelsTests
{
    [Fact]
    public void DefaultLabels_ShouldHaveExpectedEnglishValues()
    {
        var labels = ChatRoomLabels.Default;

        Assert.Equal("Loading rooms ...", labels.ProgressLabel);
        Assert.Equal("Search for a chat room", labels.SearchPlaceholder);
        Assert.Equal("Search for a chat room", labels.SearchLabel);
        Assert.Equal("The maximum number of creators that can be selected has been reached.", labels.MaximumSelectedOptionsMessageLabel);
        Assert.Equal("Suggested creators", labels.SuggestedCreators);
        Assert.Equal("No results found", labels.NoResultsFound);
        Assert.Equal("Create a new chat group", labels.NewGroup);
        Assert.Equal("<b>There are no messages to read.</b>", labels.EmptyRoomMessage);
        Assert.Equal("<b>{0} unread message</b>", labels.UnreadSingular);
        Assert.Equal("<b>{0} unread messages</b>", labels.UnreadPlural);
        Assert.Equal("You have sent 1 audio file. <br />Click on the message to discover it", labels.AudioSenderSingular);
        Assert.Equal("You have sent {0} audio files. <br />Click on the message to discover them", labels.AudioSenderPlural);
        Assert.Equal("You have received 1 audio file from {0}.<br />Click on the message to discover it", labels.AudioReceiverSingular);
        Assert.Equal("You have received {0} audio files from {1}.<br />Click on the message to discover them", labels.AudioReceiverPlural);
        Assert.Equal("You have sent 1 video file. <br />Click on the message to discover it", labels.VideoSenderSingular);
        Assert.Equal("You have sent {0} video files. <br />Click on the message to discover them", labels.VideoSenderPlural);
        Assert.Equal("You have received 1 video file from {0}.<br />Click on the message to discover it", labels.VideoReceiverSingular);
        Assert.Equal("You have received {0} video files from {1}.<br />Click on the message to discover them", labels.VideoReceiverPlural);
        Assert.Equal("You have sent 1 media file. <br />Click on the message to discover it", labels.MediaSenderSingular);
        Assert.Equal("You have sent {0} media files. <br />Click on the message to discover them", labels.MediaSenderPlural);
        Assert.Equal("You have received 1 media file from {0}.<br />Click on the message to discover it", labels.MediaReceiverSingular);
        Assert.Equal("You have received {0} media files from {1}.<br />Click on the message to discover them", labels.MediaReceiverPlural);
        Assert.Equal("You have sent 1 photo file. <br />Click on the message to discover it", labels.PhotoSenderSingular);
        Assert.Equal("You have sent {0} photo files. <br />Click on the message to discover them", labels.PhotoSenderPlural);
        Assert.Equal("You have received 1 photo file from {0}.<br />Click on the message to discover it", labels.PhotoReceiverSingular);
        Assert.Equal("You have received {0} photo files from {1}.<br />Click on the message to discover them", labels.PhotoReceiverPlural);
        Assert.Equal("You have sent a gift.", labels.GiftSender);
        Assert.Equal("You have received a gift from {0}.", labels.GiftReceiver);
        Assert.Equal("Select users", labels.UserSelectionDialogTitle);
        Assert.Equal("Search for a user", labels.SearchForUser);
        Assert.Equal("Enter the names of the users", labels.SearchForUserPlaceholder);
        Assert.Equal("The maximum number of users that can be selected has been reached.", labels.ChatUserGroupMaxOptionsSelected);
        Assert.Equal("Suggested users", labels.SuggestedUsers);
        Assert.Equal("OK", labels.DialogOk);
        Assert.Equal("Close", labels.DialogClose);
        Assert.Equal("Suggested rooms", labels.SuggestedRooms);
        Assert.Equal("Delete", labels.Delete);
        Assert.Equal("Block", labels.Block);
        Assert.Equal("Unblock", labels.Unblock);
        Assert.Equal("Rename", labels.Rename);
        Assert.Equal("Hide", labels.Hide);
        Assert.Equal("Unhide", labels.Unhide);
        Assert.Equal("Yes", labels.DialogYes);
        Assert.Equal("No", labels.DialogNo);
        Assert.Equal("Are you sure you want to delete the chat room ?", labels.DeleteRoomMessage);
        Assert.Equal("Delete the chat room", labels.DeleteRoomTitle);
        Assert.Equal("Are you sure you want to block the chat room ?", labels.BlockRoomMessage);
        Assert.Equal("Block the chat room", labels.BlockRoomTitle);
        Assert.Equal("Rename the room", labels.RenameLabel);
        Assert.Equal("Enter the name of the room", labels.RenamePlaceholder);
        Assert.Equal("Rename the room", labels.RenameDialogTitle);
        Assert.Equal("Cancel", labels.DialogCancel);
        Assert.Equal("Are you sure you want to unblock the chat room ?", labels.UnblockRoomMessage);
        Assert.Equal("Unblock the chat room", labels.UnblockRoomTitle);
        Assert.Equal("Hide the chat room", labels.HideRoomTitle);
        Assert.Equal("Are you sure you want to hide the chat room ?", labels.HideRoomMessage);
        Assert.Equal("Are you sure you want to unhide the chat room ?", labels.UnhideRoomMessage);
        Assert.Equal("Unhide the chat room", labels.UnhideRoomTitle);
    }

    [Fact]
    public void FrenchLabels_ShouldHaveExpectedFrenchValues()
    {
        var labels = ChatRoomLabels.French;

        Assert.Equal("Chargement des salles ...", labels.ProgressLabel);
        Assert.Equal("Rechercher une salle de discussion", labels.SearchPlaceholder);
        Assert.Equal("Search for a chat room", labels.SearchLabel); // Note: English value in French instance
        Assert.Equal("Le nombre maximal de créateurs pouvant être sélectionné a été atteint.", labels.MaximumSelectedOptionsMessageLabel);
        Assert.Equal("Créateurs ou créatrices suggérés", labels.SuggestedCreators);
        Assert.Equal("Aucun résultat", labels.NoResultsFound);
        Assert.Equal("Créer un chat de groupe", labels.NewGroup);
        Assert.Equal("<b>Il n'y a aucun message à lire</b>", labels.EmptyRoomMessage);
        Assert.Equal("<b>{0} message non lu</b>", labels.UnreadSingular);
        Assert.Equal("<b>{0} messages non lus</b>", labels.UnreadPlural);
        Assert.Equal("Vous avez envoyé 1 fichier audio. <br />Cliquez sur le message pour le découvrir", labels.AudioSenderSingular);
        Assert.Equal("Vous avez envoyé {0} fichiers audio. <br />Cliquez sur le message pour les découvrir", labels.AudioSenderPlural);
        Assert.Equal("Vous avez reçu 1 fichier audio de {0}.<br />Cliquez sur le message pour le découvrir", labels.AudioReceiverSingular);
        Assert.Equal("Vous avez reçu {0} fichers audio de {1}.<br />Cliquez sur le message pour les découvrir", labels.AudioReceiverPlural);
        Assert.Equal("Vous avez envoyé 1 fichier vidéo. <br />Cliquez sur le message pour le découvrir", labels.VideoSenderSingular);
        Assert.Equal("Vous avez envoyé {0} fichiers vidéo. <br />Cliquez sur le message pour les découvrir", labels.VideoSenderPlural);
        Assert.Equal("Vous avez reçu 1 fichier vidéo de {0}.<br />Cliquez sur le message pour le découvrir", labels.VideoReceiverSingular);
        Assert.Equal("Vous avez reçu {0} fichers vidéo de {1}.<br />Cliquez sur le message pour les découvrir", labels.VideoReceiverPlural);
        Assert.Equal("Vous avez envoyé 1 fichier média. <br />Cliquez sur le message pour le découvrir", labels.MediaSenderSingular);
        Assert.Equal("Vous avez envoyé {0} fichiers média. <br />Cliquez sur le message pour les découvrir", labels.MediaSenderPlural);
        Assert.Equal("Vous avez reçu 1 fichier média de {0}.<br />Cliquez sur le message pour le découvrir", labels.MediaReceiverSingular);
        Assert.Equal("Vous avez reçu {0} fichers média de {1}.<br />Cliquez sur le message pour les découvrir", labels.MediaReceiverPlural);
        Assert.Equal("Vous avez envoyé 1 fichier photo. <br />Cliquez sur le message pour le découvrir", labels.PhotoSenderSingular);
        Assert.Equal("Vous avez envoyé {0} fichiers photos. <br />Cliquez sur le message pour les découvrir", labels.PhotoSenderPlural);
        Assert.Equal("Vous avez reçu 1 fichier photo de {0}.<br />Cliquez sur le message pour le découvrir", labels.PhotoReceiverSingular);
        Assert.Equal("Vous avez reçu {0} fichers photos de {1}.<br />Cliquez sur le message pour les découvrir", labels.PhotoReceiverPlural);
        Assert.Equal("Vous avez envoyé un cadeau.", labels.GiftSender);
        Assert.Equal("Vous avez reçu un cadeau de la part {0}.", labels.GiftReceiver);
        Assert.Equal("Selectionnez les utilisateurs", labels.UserSelectionDialogTitle);
        Assert.Equal("Rechercher un utilisateur", labels.SearchForUser);
        Assert.Equal("Rechercher des utilisateurs ...", labels.SearchForUserPlaceholder);
        Assert.Equal("Le nombre maximal d'utilisateurs pouvant être sélectionné a été atteint.", labels.ChatUserGroupMaxOptionsSelected);
        Assert.Equal("Utilisateurs suggérés", labels.SuggestedUsers);
        Assert.Equal("Valider", labels.DialogOk);
        Assert.Equal("Fermer", labels.DialogClose);
        Assert.Equal("Salles suggérées", labels.SuggestedRooms);
        Assert.Equal("Supprimer", labels.Delete);
        Assert.Equal("Bloquer", labels.Block);
        Assert.Equal("Débloquer", labels.Unblock);
        Assert.Equal("Renommer", labels.Rename);
        Assert.Equal("Masquer", labels.Hide);
        Assert.Equal("Démasquer", labels.Unhide);
        Assert.Equal("Oui", labels.DialogYes);
        Assert.Equal("Non", labels.DialogNo);
        Assert.Equal("Êtes-vous sûr de vouloir supprimer la salle de discussion ?", labels.DeleteRoomMessage);
        Assert.Equal("Supprimer la salle de discussion", labels.DeleteRoomTitle);
        Assert.Equal("Êtes-vous sûr de vouloir bloquer la salle de discussion ?", labels.BlockRoomMessage);
        Assert.Equal("Bloquer la salle de discussion", labels.BlockRoomTitle);
        Assert.Equal("Renommer la salle", labels.RenameLabel);
        Assert.Equal("Entrez le nom de la salle", labels.RenamePlaceholder);
        Assert.Equal("Renommer la salle", labels.RenameDialogTitle);
        Assert.Equal("Annuler", labels.DialogCancel);
        Assert.Equal("Êtes-vous sûr de vouloir débloquer la salle de discussion ?", labels.UnblockRoomMessage);
        Assert.Equal("Débloquer la salle de discussion", labels.UnblockRoomTitle);
        Assert.Equal("Masquer la salle de discussion", labels.HideRoomTitle);
        Assert.Equal("Êtes-vous sûr de vouloir masquer la salle de discussion ?", labels.HideRoomMessage);
        Assert.Equal("Êtes-vous sûr de vouloir démasquer la salle de discussion ?", labels.UnhideRoomMessage);
        Assert.Equal("Démasquer la salle de discussion", labels.UnhideRoomTitle);
    }

    [Fact]
    public void Properties_ShouldBeSettable()
    {
        var labels = new ChatRoomLabels
        {
            ProgressLabel = "Test Progress",
            SearchPlaceholder = "Test Search",
            SearchLabel = "Test Search Label",
            MaximumSelectedOptionsMessageLabel = "Test Max",
            SuggestedCreators = "Test Creators",
            NoResultsFound = "Test No Results",
            NewGroup = "Test New Group",
            EmptyRoomMessage = "Test Empty",
            UnreadSingular = "Test Unread 1",
            UnreadPlural = "Test Unread N",
            AudioSenderSingular = "Test Audio 1",
            AudioSenderPlural = "Test Audio N",
            AudioReceiverSingular = "Test Audio R1",
            AudioReceiverPlural = "Test Audio RN",
            VideoSenderSingular = "Test Video 1",
            VideoSenderPlural = "Test Video N",
            VideoReceiverSingular = "Test Video R1",
            VideoReceiverPlural = "Test Video RN",
            MediaSenderSingular = "Test Media 1",
            MediaSenderPlural = "Test Media N",
            MediaReceiverSingular = "Test Media R1",
            MediaReceiverPlural = "Test Media RN",
            PhotoSenderSingular = "Test Photo 1",
            PhotoSenderPlural = "Test Photo N",
            PhotoReceiverSingular = "Test Photo R1",
            PhotoReceiverPlural = "Test Photo RN",
            GiftSender = "Test Gift S",
            GiftReceiver = "Test Gift R",
            UserSelectionDialogTitle = "Test User Dialog",
            SearchForUser = "Test Search User",
            SearchForUserPlaceholder = "Test Search User Placeholder",
            ChatUserGroupMaxOptionsSelected = "Test Max Users",
            SuggestedUsers = "Test Suggested Users",
            DialogOk = "Test OK",
            DialogClose = "Test Close",
            SuggestedRooms = "Test Suggested Rooms",
            Delete = "Test Delete",
            Block = "Test Block",
            Unblock = "Test Unblock",
            Rename = "Test Rename",
            Hide = "Test Hide",
            Unhide = "Test Unhide",
            DialogYes = "Test Yes",
            DialogNo = "Test No",
            DeleteRoomMessage = "Test Delete Room Message",
            DeleteRoomTitle = "Test Delete Room Title",
            BlockRoomMessage = "Test Block Room Message",
            BlockRoomTitle = "Test Block Room Title",
            RenameLabel = "Test Rename Label",
            RenamePlaceholder = "Test Rename Placeholder",
            RenameDialogTitle = "Test Rename Dialog Title",
            DialogCancel = "Test Cancel",
            UnblockRoomMessage = "Test Unblock Room Message",
            UnblockRoomTitle = "Test Unblock Room Title",
            HideRoomTitle = "Test Hide Room Title",
            HideRoomMessage = "Test Hide Room Message",
            UnhideRoomMessage = "Test Unhide Room Message",
            UnhideRoomTitle = "Test Unhide Room Title"
        };

        Assert.Equal("Test Progress", labels.ProgressLabel);
        Assert.Equal("Test Search", labels.SearchPlaceholder);
        Assert.Equal("Test Search Label", labels.SearchLabel);
        Assert.Equal("Test Max", labels.MaximumSelectedOptionsMessageLabel);
        Assert.Equal("Test Creators", labels.SuggestedCreators);
        Assert.Equal("Test No Results", labels.NoResultsFound);
        Assert.Equal("Test New Group", labels.NewGroup);
        Assert.Equal("Test Empty", labels.EmptyRoomMessage);
        Assert.Equal("Test Unread 1", labels.UnreadSingular);
        Assert.Equal("Test Unread N", labels.UnreadPlural);
        Assert.Equal("Test Audio 1", labels.AudioSenderSingular);
        Assert.Equal("Test Audio N", labels.AudioSenderPlural);
        Assert.Equal("Test Audio R1", labels.AudioReceiverSingular);
        Assert.Equal("Test Audio RN", labels.AudioReceiverPlural);
        Assert.Equal("Test Video 1", labels.VideoSenderSingular);
        Assert.Equal("Test Video N", labels.VideoSenderPlural);
        Assert.Equal("Test Video R1", labels.VideoReceiverSingular);
        Assert.Equal("Test Video RN", labels.VideoReceiverPlural);
        Assert.Equal("Test Media 1", labels.MediaSenderSingular);
        Assert.Equal("Test Media N", labels.MediaSenderPlural);
        Assert.Equal("Test Media R1", labels.MediaReceiverSingular);
        Assert.Equal("Test Media RN", labels.MediaReceiverPlural);
        Assert.Equal("Test Photo 1", labels.PhotoSenderSingular);
        Assert.Equal("Test Photo N", labels.PhotoSenderPlural);
        Assert.Equal("Test Photo R1", labels.PhotoReceiverSingular);
        Assert.Equal("Test Photo RN", labels.PhotoReceiverPlural);
        Assert.Equal("Test Gift S", labels.GiftSender);
        Assert.Equal("Test Gift R", labels.GiftReceiver);
        Assert.Equal("Test User Dialog", labels.UserSelectionDialogTitle);
        Assert.Equal("Test Search User", labels.SearchForUser);
        Assert.Equal("Test Search User Placeholder", labels.SearchForUserPlaceholder);
        Assert.Equal("Test Max Users", labels.ChatUserGroupMaxOptionsSelected);
        Assert.Equal("Test Suggested Users", labels.SuggestedUsers);
        Assert.Equal("Test OK", labels.DialogOk);
        Assert.Equal("Test Close", labels.DialogClose);
        Assert.Equal("Test Suggested Rooms", labels.SuggestedRooms);
        Assert.Equal("Test Delete", labels.Delete);
        Assert.Equal("Test Block", labels.Block);
        Assert.Equal("Test Unblock", labels.Unblock);
        Assert.Equal("Test Rename", labels.Rename);
        Assert.Equal("Test Hide", labels.Hide);
        Assert.Equal("Test Unhide", labels.Unhide);
        Assert.Equal("Test Yes", labels.DialogYes);
        Assert.Equal("Test No", labels.DialogNo);
        Assert.Equal("Test Delete Room Message", labels.DeleteRoomMessage);
        Assert.Equal("Test Delete Room Title", labels.DeleteRoomTitle);
        Assert.Equal("Test Block Room Message", labels.BlockRoomMessage);
        Assert.Equal("Test Block Room Title", labels.BlockRoomTitle);
        Assert.Equal("Test Rename Label", labels.RenameLabel);
        Assert.Equal("Test Rename Placeholder", labels.RenamePlaceholder);
        Assert.Equal("Test Rename Dialog Title", labels.RenameDialogTitle);
        Assert.Equal("Test Cancel", labels.DialogCancel);
        Assert.Equal("Test Unblock Room Message", labels.UnblockRoomMessage);
        Assert.Equal("Test Unblock Room Title", labels.UnblockRoomTitle);
        Assert.Equal("Test Hide Room Title", labels.HideRoomTitle);
        Assert.Equal("Test Hide Room Message", labels.HideRoomMessage);
        Assert.Equal("Test Unhide Room Message", labels.UnhideRoomMessage);
        Assert.Equal("Test Unhide Room Title", labels.UnhideRoomTitle);
    }
}
