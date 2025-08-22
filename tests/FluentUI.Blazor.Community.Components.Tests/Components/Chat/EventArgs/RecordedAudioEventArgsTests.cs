using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Chat;

public class RecordedAudioEventArgsTests
{
    [Fact]
    public void Constructor_SetsOriginalData()
    {
        // Arrange
        var data = new byte[] { 1, 2, 3 };

        // Act
        var args = new RecordedAudioEventArgs(data);

        // Assert
        Assert.Equal(data, args.OriginalData);
    }

    [Fact]
    public void Audio_Property_GetSet_Works()
    {
        // Arrange
        var args = new RecordedAudioEventArgs([4, 5, 6]);
        var audio = new RecordedAudio([1, 2, 3, 4, 5, 6], "audio/wav");

        // Act
        args.Audio = audio;

        // Assert
        Assert.Equal(audio, args.Audio);
    }

    [Fact]
    public void Audio_Property_DefaultIsNull()
    {
        // Arrange
        var args = new RecordedAudioEventArgs([7, 8, 9]);

        // Assert
        Assert.Null(args.Audio);
    }
}
