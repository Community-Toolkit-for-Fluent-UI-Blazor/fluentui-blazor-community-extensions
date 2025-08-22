using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Chat;

public class RecordAudioTests
{
    [Fact]
    public void Constructor_SetsPropertiesCorrectly()
    {
        // Arrange
        var audioData = new byte[] { 1, 2, 3 };
        var contentType = "audio/wav";

        // Act
        var audio = new RecordedAudio(audioData, contentType);

        // Assert
        Assert.Equal(audioData, audio.AudioData);
        Assert.Equal(contentType, audio.ContentType);
    }

    [Fact]
    public void Equality_WorksForIdenticalValues()
    {
        var audioData = new byte[] { 1, 2, 3 };
        var contentType = "audio/wav";

        var audio1 = new RecordedAudio(audioData, contentType);
        var audio2 = new RecordedAudio(audioData, contentType);

        Assert.Equal(audio1, audio2);
        Assert.True(audio1 == audio2);
        Assert.False(audio1 != audio2);
    }

    [Fact]
    public void Equality_FailsForDifferentValues()
    {
        var audio1 = new RecordedAudio(new byte[] { 1, 2, 3 }, "audio/wav");
        var audio2 = new RecordedAudio(new byte[] { 4, 5, 6 }, "audio/mp3");

        Assert.NotEqual(audio1, audio2);
        Assert.False(audio1 == audio2);
        Assert.True(audio1 != audio2);
    }

    [Fact]
    public void GetHashCode_IsConsistentForEqualObjects()
    {
        var audioData = new byte[] { 1, 2, 3 };
        var contentType = "audio/wav";

        var audio1 = new RecordedAudio(audioData, contentType);
        var audio2 = new RecordedAudio(audioData, contentType);

        Assert.Equal(audio1.GetHashCode(), audio2.GetHashCode());
    }

    [Fact]
    public void Deconstruct_ReturnsCorrectValues()
    {
        var audioData = new byte[] { 1, 2, 3 };
        var contentType = "audio/wav";
        var audio = new RecordedAudio(audioData, contentType);

        audio.Deconstruct(out var actualAudioData, out var actualContentType);

        Assert.Equal(audioData, actualAudioData);
        Assert.Equal(contentType, actualContentType);
    }
}
