using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Signature;
public class SignatureExportSettingsTests
{
    [Fact]
    public void Default_Values_Are_Correct()
    {
        // Arrange
        var settings = new SignatureExportSettings();

        // Assert
        Assert.Equal(SignatureExportFormat.Webp, settings.Format);
        Assert.Equal(90, settings.Quality);
    }

    [Fact]
    public void UpdateInternalValues_Updates_Properties()
    {
        // Arrange
        var settings = new SignatureExportSettings();
        var state = new SignatureState
        {
            Quality = 75,
            ExportFormat = SignatureExportFormat.Jpeg
        };

        // Act
        settings.UpdateInternalValues(state);

        // Assert
        Assert.Equal(75, settings.Quality);
        Assert.Equal(SignatureExportFormat.Jpeg, settings.Format);
    }
}
