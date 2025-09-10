using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Tests;
using Moq;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Signature;
public class SignatureSettingsTests : TestBase
{
    public SignatureSettingsTests()
    {
        JSInterop.Mode = Bunit.JSRuntimeMode.Loose;
        Services.AddSingleton(UnitTestLibraryConfiguration);
        Services.AddScoped<SignatureState>();
        Services.AddFluentUIComponents();
    }

    private IRenderedFragment RenderSignature(Action<ComponentParameterCollectionBuilder<FluentCxSignature>>? configureParameters = null)
    {
        return Render(b =>
        {
            b.OpenComponent<FluentMenuProvider>(0);
            b.CloseComponent();
            b.OpenComponent<FluentCxSignature>(1);
            if (configureParameters != null)
            {
                var parameters = new ComponentParameterCollectionBuilder<FluentCxSignature>();
                configureParameters(parameters);
                foreach (var param in parameters.Build())
                {
                    b.AddAttribute(2, param.Name, param.Value);
                }
            }
            b.CloseComponent();
        });
    }

    [Fact]
    public void OnParametersSet_ClampsValues()
    {
        var comp = RenderSignature(p =>
        {
            p.Add(p=>p.SignatureSettings, new SignatureSettings
            {
                SeparatorY = 0.5f,
                PenOpacity = 0.5f,
                GridOpacity = 0.3f
            });
        });

        var r = comp.FindComponent<FluentCxSignature>();

        Assert.InRange(r.Instance.SignatureSettings.SeparatorY, 0.0f, 1.0f);
        Assert.InRange(r.Instance.SignatureSettings.PenOpacity, 0.05f, 1.0f);
        Assert.InRange(r.Instance.SignatureSettings.GridOpacity, 0.05f, 1.0f);
    }


    [Fact]
    public void UpdateInternalValues_UpdatesPropertiesAndInvalidatesRender()
    {
        var comp = RenderSignature(p =>
        {
            p.Add(p => p.SignatureSettings, new SignatureSettings
            {
                SeparatorY = 0.5f,
                PenOpacity = 0.5f,
                GridOpacity = 0.3f
            });
        });

        var r = comp.FindComponent<FluentCxSignature>();

        var state = new SignatureState
        {
            StrokeWidth = 5.0f,
            PenColor = "#FF0000",
            PenOpacity = 0.5f,
            StrokeStyle = SignatureLineStyle.Dotted,
            ShowSeparatorLine = true,
            UseShadow = true,
            ShadowOpacity = 0.8f,
            ShadowColor = "#00FF00",
            GridType = SignatureGridType.None,
            UseSmooth = false,
            UsePointerPressure = true
        };

        var settings = r.Instance.SignatureSettings;
        settings.UpdateInternalValues(state);

        Assert.Equal(5.0f, settings.StrokeWidth);
        Assert.Equal("#FF0000", settings.PenColor);
        Assert.Equal(0.5f, settings.PenOpacity);
        Assert.Equal(SignatureLineStyle.Dotted, settings.StrokeStyle);
        Assert.True(settings.ShowSeparatorLine);
        Assert.True(settings.UseShadow);
        Assert.Equal(0.8f, settings.ShadowOpacity);
        Assert.Equal("#00FF00", settings.ShadowColor);
        Assert.Equal(SignatureGridType.None, settings.GridType);
        Assert.False(settings.Smooth);
        Assert.True(settings.UsePointerPressure);
    }

    [Fact]
    public void Reset_SetsDefaultValues()
    {
        var comp = RenderSignature(p =>
        {
            p.Add(p => p.SignatureSettings, new SignatureSettings
            {
                Tool = SignatureTool.Eraser,
                StrokeWidth = 10.0f,
                StrokeColor = "#123456",
                PenColor = "#654321",
                ShadowColor = "#abcdef",
                ShadowOpacity = 0.1f,
                PenOpacity = 0.2f,
                StrokeStyle = SignatureLineStyle.Dotted,
                Smooth = false,
                UseShadow = true,
                UsePointerPressure = true,
                BackgroundColor = "#000000",
                ShowSeparatorLine = true,
                SeparatorLineColor = "#111111"
            });
        });

        var r = comp.FindComponent<FluentCxSignature>();
        var settings = r.Instance.SignatureSettings;

        settings.Reset();

        Assert.Equal(SignatureTool.Pen, settings.Tool);
        Assert.Equal(2.0f, settings.StrokeWidth);
        Assert.Equal("#000000", settings.StrokeColor);
        Assert.Equal("#000000", settings.PenColor);
        Assert.Equal("#000000", settings.ShadowColor);
        Assert.Equal(0.6f, settings.ShadowOpacity);
        Assert.Equal(1.0f, settings.PenOpacity);
        Assert.Equal(SignatureLineStyle.Solid, settings.StrokeStyle);
        Assert.True(settings.Smooth);
        Assert.False(settings.UseShadow);
        Assert.False(settings.UsePointerPressure);
        Assert.Equal("#FFFFFF", settings.BackgroundColor);
        Assert.False(settings.ShowSeparatorLine);
        Assert.Equal("#808080", settings.SeparatorLineColor);
    }
}
