using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components;

public class ChatMessageListLabelsTests
{
    [Fact]
    public void DefaultLabels_AreEnglish()
    {
        var labels = ChatMessageListLabels.Default;
        Assert.Equal("Loading ...", labels.LoadingLabel);
        Assert.Equal("You started a discussion with {0}.<br />Creators are people first and foremost.<br />Messages should be respectful...", labels.EmptyRoomWelcome);
        Assert.Equal("Welcome from {0} 😀", labels.EmptyRoomMessage);
        Assert.Equal("Let's chat", labels.EmptyRoomLetsTalk);
        Assert.Equal("Enter your message ...", labels.EnterMessage);
        Assert.Equal("Add media (images, videos, audios)", labels.InsertMedia);
        Assert.Equal("Add emojis", labels.InsertEmojis);
        Assert.Equal("Add a gift", labels.InsertGift);
        Assert.Equal("Send the message", labels.SendMessage);
        Assert.Equal("Cancel", labels.DialogCancel);
        Assert.Equal("Ok", labels.DialogOk);
        Assert.Equal("Drag files here you wish to upload, or <label for=\"{0}\">browse</label> for them<span style=\"color: red;\">*</span>.<br /><em>Maximum of {1} files allowed.</em>", labels.DragDropFileLabel);
        Assert.Equal("Completed", labels.Completed);
        Assert.Equal("Loading {0} - {1}", labels.Progression);
        Assert.Equal("This chat room has been blocked by a user... You cannot communicate until the user unblock you.", labels.BlockMessage);
        Assert.Equal("The text of the message has been copied !", labels.MessageTextCopied);
        Assert.Equal("Copy", labels.Copy);
        Assert.Equal("Delete", labels.Delete);
        Assert.Equal("Edit", labels.Edit);
        Assert.Equal("Unread", labels.Unread);
        Assert.Equal("Read", labels.Read);
        Assert.Equal("Read by everyone", labels.ReadByEveryone);
        Assert.Equal("Are you sure you want to delete the message ?", labels.DeleteMessage);
        Assert.Equal("Delete the message", labels.DeleteTitle);
        Assert.Equal("No", labels.DialogNo);
        Assert.Equal("Yes", labels.DialogYes);
        Assert.Equal("Edited", labels.Edited);
        Assert.Equal("Pin", labels.PinLabel);
        Assert.Equal("Unpin", labels.UnpinLabel);
        Assert.Equal("Import files from ...", labels.FileSelectorDialogTitle);
        Assert.Equal("Reply", labels.Reply);
        Assert.Equal("Gift message", labels.ReplyFromGiftOnly);
        Assert.Equal("Media message (audio, images, videos...)", labels.ReplyFromDocumentOnly);
        Assert.Equal("Message being sent ...", labels.SendingLabel);
        Assert.Equal("This message was deleted.", labels.ThisMessageWasDeleted);
    }

    [Fact]
    public void FrenchLabels_AreFrench()
    {
        var labels = ChatMessageListLabels.French;
        Assert.Equal("Chargement des messages ...", labels.LoadingLabel);
        Assert.Equal("Bienvenue de la part de {0} 😀", labels.EmptyRoomWelcome);
        Assert.Equal("Vous avez commencé une discussion avec {0}.<br />Les créateurs et les créatrices sont des personnes avant tout.<br />Les messages doivent être respectueux...", labels.EmptyRoomMessage);
        Assert.Equal("Discutons", labels.EmptyRoomLetsTalk);
        Assert.Equal("Entrez votre message", labels.EnterMessage);
        Assert.Equal("Ajouter des médias (audio, vidéo, images)", labels.InsertMedia);
        Assert.Equal("Ajouter des émojis", labels.InsertEmojis);
        Assert.Equal("Ajouter des cadeaux", labels.InsertGift);
        Assert.Equal("Envoyer le message", labels.SendMessage);
        Assert.Equal("Annuler", labels.DialogCancel);
        Assert.Equal("Valider", labels.DialogOk);
        Assert.Equal("Faites glisser les fichiers, que vous souhaitez télécharger, ici ou <label for=\"{0}\">sélectionnez</label> les<span style=\"color: red;\">*</span>.<br /><em>Maximum de {1} fichiers autorisés.</em>", labels.DragDropFileLabel);
        Assert.Equal("Terminé", labels.Completed);
        Assert.Equal("Chargement {0} - {1}", labels.Progression);
        Assert.Equal("Cette salle de discussion a été bloquée par un utilisateur. Vous ne pouvez plus communiquer avec lui tant qu'il ne vous a pas débloqué", labels.BlockMessage);
        Assert.Equal("Le texte du message a bien été copié !", labels.MessageTextCopied);
        Assert.Equal("Copier", labels.Copy);
        Assert.Equal("Supprimer", labels.Delete);
        Assert.Equal("Modifier", labels.Edit);
        Assert.Equal("Non lu", labels.Unread);
        Assert.Equal("Lu", labels.Read);
        Assert.Equal("Lu par tout le monde", labels.ReadByEveryone);
        Assert.Equal("Êtes-vous sûr de vouloir supprimer le message ?", labels.DeleteMessage);
        Assert.Equal("Supprimer le message", labels.DeleteTitle);
        Assert.Equal("Non", labels.DialogNo);
        Assert.Equal("Oui", labels.DialogYes);
        Assert.Equal("Modifié", labels.Edited);
        Assert.Equal("Épingler", labels.PinLabel);
        Assert.Equal("Désépingler", labels.UnpinLabel);
        Assert.Equal("Importer des fichiers de", labels.FileSelectorDialogTitle);
        Assert.Equal("Répondre", labels.Reply);
        Assert.Equal("Message cadeau", labels.ReplyFromGiftOnly);
        Assert.Equal("Message document (photos, audio, vidéos...)", labels.ReplyFromDocumentOnly);
        Assert.Equal("Message en cours d'envoi ...", labels.SendingLabel);
        Assert.Equal("Ce message a été supprimé.", labels.ThisMessageWasDeleted);
    }

    [Fact]
    public void CanModifyProperties()
    {
        var labels = new ChatMessageListLabels();
        labels.LoadingLabel = "Test";
        labels.EmptyRoomWelcome = "Bienvenue";
        labels.Delete = "Effacer";
        Assert.Equal("Test", labels.LoadingLabel);
        Assert.Equal("Bienvenue", labels.EmptyRoomWelcome);
        Assert.Equal("Effacer", labels.Delete);
    }

    [Fact]
    public void Equality_WorksForSameValues()
    {
        var l1 = new ChatMessageListLabels { LoadingLabel = "A", Delete = "B" };
        var l2 = new ChatMessageListLabels { LoadingLabel = "A", Delete = "B" };
        Assert.Equal(l1, l2);
        Assert.True(l1 == l2);
        Assert.False(l1 != l2);
    }
}
