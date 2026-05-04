using FluentUI.Blazor.Community.Components.Enums;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides a factory for creating surface renderers for barcode symbologies.
/// </summary>
/// <remarks>This class is intended for internal use to obtain an appropriate renderer based on the specified
/// barcode symbology. The returned renderer can be used to generate visual representations of barcodes according to the
/// provided rendering options.</remarks>
internal sealed class BarcodeRendererFactory
{
    /// <summary>
    /// Provides the surface renderer used to generate barcode images based on the specified rendering options.
    /// </summary>
    /// <remarks>The renderer determines how barcodes are visually rendered. Assign an implementation that
    /// supports the desired output format, such as SVG or bitmap.</remarks>
    private ISurfaceComposer<BarcodeRenderingOptions>? _renderer;

    /// <summary>
    /// Represents the symbology definition associated with the current renderer.
    /// </summary>
    private ISymbology? _symbology;

    /// <summary>
    /// Provides a mapping between supported barcode symbologies and their corresponding factory methods for creating
    /// surface renderers.
    /// </summary>
    /// <remarks>Each entry associates a specific symbology with a function that returns an appropriate
    /// surface renderer instance configured for that symbology. This dictionary enables dynamic selection and
    /// instantiation of barcode renderers based on the requested symbology.</remarks>
    private static readonly Dictionary<Symbology, Func<ISymbology, Func<string>, ISurfaceComposer<BarcodeRenderingOptions>>> _factories
        = new(EqualityComparer<Symbology>.Default)
        {
            [Symbology.Code128] = CreateCode128,
            [Symbology.Code39] = CreateCode39,
            [Symbology.UpcA] = CreateUpcA,
            [Symbology.UpcE] = CreateUpcE,
            [Symbology.Code93] = CreateCode93,
            [Symbology.Ean13] = CreateEan13,
            [Symbology.Ean8] = CreateEan8,
            [Symbology.GS1_128] = CreateGs1_128,
            [Symbology.Itf14] = CreateItf14,
            [Symbology.Codabar] = CreateCodabar,
            [Symbology.QRCode] = CreateQRCode,
            [Symbology.Pdf417] = CreatePdf417
        };

    /// <summary>
    /// Provides a mapping between barcode symbologies and their corresponding update actions for surface renderers.
    /// </summary>
    /// <remarks>Each entry associates a specific Symbology value with an action that updates the rendering
    /// options for that symbology. This dictionary enables dynamic configuration of barcode rendering based on the
    /// selected symbology.</remarks>
    private static readonly Dictionary<Symbology, Action<ISurfaceComposer<BarcodeRenderingOptions>, ISymbology>> _updates
        = new(EqualityComparer<Symbology>.Default)
        {
            [Symbology.Code128] = (r, s) => UpdateCode128Options(((Barcode1DComposer<Code128Options>)r).Options, (Code128Symbology)s),
            [Symbology.Code39] = (r, s) => UpdateCode39Options(((Barcode1DComposer<Code39Options>)r).Options, (Code39Symbology)s),
            [Symbology.UpcA] = (r, s) => UpdateUpcAOptions(((Barcode1DComposer<UpcAOptions>)r).Options, (UpcASymbology)s),
            [Symbology.UpcE] = (r, s) => UpdateUpcEOptions(((Barcode1DComposer<UpcEOptions>)r).Options, (UpcESymbology)s),
            [Symbology.Code93] = (r, s) => UpdateCode93Options(((Barcode1DComposer<Code93Options>)r).Options, (Code93Symbology)s),
            [Symbology.Ean13] = (r, s) => UpdateEan13Options(((Barcode1DComposer<Ean13Options>)r).Options, (Ean13Symbology)s),
            [Symbology.Ean8] = (r, s) => UpdateEan8Options(((Barcode1DComposer<Ean8Options>)r).Options, (Ean8Symbology)s),
            [Symbology.GS1_128] = (r, s) => UpdateGs1_128Options(((Barcode1DComposer<Gs1_128Options>)r).Options, (GS1_128Symbology)s),
            [Symbology.Itf14] = (r, s) => UpdateItf14Options(((Barcode1DComposer<Itf14Options>)r).Options, (Itf14Symbology)s),
            [Symbology.Codabar] = (r, s) => UpdateCodabarOptions(((Barcode1DComposer<CodabarOptions>)r).Options, (CodabarSymbology)s),
            [Symbology.QRCode] = (r, s) => UpdateQRCodeOptions(((Barcode2DComposer<QRCodeOptions>)r).Options, (QRCodeSymbology)s),
            [Symbology.Pdf417] = (r, s) => UpdatePdf417Options(((Barcode1DStackedComposer<Pdf417Options>)r).Options, (Pdf417Symbology)s)
        };

    /// <summary>
    /// Retrieves an ISurfaceRenderer instance configured for the specified symbology and value provider.
    /// </summary>
    /// <remarks>If a renderer for the specified symbology does not exist, a new one is created using the
    /// registered factory. Otherwise, the existing renderer is updated with the new options.</remarks>
    /// <param name="symbology">The symbology definition used to select and configure the renderer. Cannot be null.</param>
    /// <param name="getValue">A delegate that provides the value to be rendered by the barcode renderer. Cannot be null.</param>
    /// <returns>An ISurfaceRenderer instance configured for the given symbology and value provider, or null if no suitable
    /// renderer is available.</returns>
    internal ISurfaceComposer<BarcodeRenderingOptions>? Get(
        ISymbology symbology,
        Func<string> getValue)
    {
        if (_renderer == null || _symbology?.Symbology != symbology.Symbology)
        {
            if (_factories.TryGetValue(symbology.Symbology, out var factory))
            {
                _renderer = factory(symbology, getValue);
                _symbology = symbology;
            }
            else
            {
                return null;
            }
        }
        else if (_updates.TryGetValue(symbology.Symbology, out var update))
        {
            update(_renderer, symbology);
        }

        return _renderer;
    }

    /// <summary>
    /// Creates a surface renderer for generating Code 128 barcodes using the specified symbology and value provider.
    /// </summary>
    /// <param name="s">The symbology to use for the Code 128 barcode. Must be a value compatible with Code 128 encoding.</param>
    /// <param name="getValue">A delegate that returns the string value to encode in the barcode. The returned value determines the barcode
    /// content.</param>
    /// <returns>An ISurfaceRenderer instance configured to render a Code 128 barcode with the specified options and value.</returns>
    private static Barcode1DComposer<Code128Options> CreateCode128(ISymbology s, Func<string> getValue)
    {
        var sym = (Code128Symbology)s;

        return new Barcode1DComposer<Code128Options>(
            Code128Encoder.Instance,
            getValue,
            BuildCode128Options(sym));
    }

    /// <summary>
    /// Creates a barcode renderer for the Code 39 symbology using the specified symbology settings and value provider.
    /// </summary>
    /// <param name="s">The symbology configuration to use for rendering the Code 39 barcode. Must be an instance of Code39Symbology.</param>
    /// <param name="getValue">A delegate that returns the string value to encode in the barcode.</param>
    /// <returns>A Barcode1DRenderer configured for the Code 39 symbology and the provided value source.</returns>
    private static Barcode1DComposer<Code39Options> CreateCode39(ISymbology s, Func<string> getValue)
    {
        var sym = (Code39Symbology)s;

        return new Barcode1DComposer<Code39Options>(
            Code39Encoder.Instance,
            getValue,
            BuildCode39Options(sym));
    }

    /// <summary>
    /// Creates a renderer for generating UPC-A barcodes using the specified symbology and value provider.
    /// </summary>
    /// <param name="s">The symbology configuration to use for the UPC-A barcode. Must be of type UpcASymbology.</param>
    /// <param name="getValue">A delegate that provides the string value to encode in the UPC-A barcode.</param>
    /// <returns>An ISurfaceRenderer instance configured to render a UPC-A barcode with the specified options and value.</returns>
    private static Barcode1DComposer<UpcAOptions> CreateUpcA(ISymbology s, Func<string> getValue)
    {
        var sym = (UpcASymbology)s;

        return new Barcode1DComposer<UpcAOptions>(
            UpcAEncoder.Instance,
            getValue,
            BuildUpcAOptions(sym));
    }

    /// <summary>
    /// Creates a surface renderer for UPC-E barcodes using the specified symbology and value provider.
    /// </summary>
    /// <param name="s">The symbology configuration to use for rendering the UPC-E barcode. Must be of type UpcESymbology.</param>
    /// <param name="getValue">A delegate that returns the string value to encode in the UPC-E barcode.</param>
    /// <returns>An ISurfaceRenderer instance configured to render a UPC-E barcode with the specified options and value.</returns>
    private static Barcode1DComposer<UpcEOptions> CreateUpcE(ISymbology s, Func<string> getValue)
    {
        var sym = (UpcESymbology)s;

        return new Barcode1DComposer<UpcEOptions>(
            UpcEEncoder.Instance,
            getValue,
            BuildUpcEOptions(sym));
    }

    /// <summary>
    /// Crée un générateur de surface pour le rendu de codes-barres Code 93 avec les options spécifiées.
    /// </summary>
    /// <param name="s">La symbologie à utiliser pour configurer le générateur Code 93. Doit être de type Code93Symbology.</param>
    /// <param name="getValue">Fonction qui fournit la valeur de données à encoder dans le code-barres.</param>
    /// <returns>Un générateur de surface configuré pour le rendu de codes-barres Code 93 avec les options spécifiées.</returns>
    private static Barcode1DComposer<Code93Options> CreateCode93(ISymbology s, Func<string> getValue)
    {
        var sym = (Code93Symbology)s;

        return new Barcode1DComposer<Code93Options>(
            Code93Encoder.Instance,
            getValue,
            BuildCode93Options(sym));
    }

    /// <summary>
    /// Creates a new renderer for EAN-13 barcodes using the specified symbology and value provider.
    /// </summary>
    /// <param name="s">The symbology configuration to use for EAN-13 barcode rendering. Must be an instance of Ean13Symbology.</param>
    /// <param name="getValue">A delegate that returns the string value to encode in the EAN-13 barcode.</param>
    /// <returns>Returns a <see cref="Barcode1DComposer{Ean13Options}"/> instance configured for EAN-13 barcode generation.</returns>
    private static Barcode1DComposer<Ean13Options> CreateEan13(ISymbology s, Func<string> getValue)
    {
        var sym = (Ean13Symbology)s;

        return new Barcode1DComposer<Ean13Options>(
            Ean13Encoder.Instance,
            getValue,
            BuildEan13Options(sym));
    }

    /// <summary>
    /// Creates a new instance of a 1D barcode renderer configured for the EAN-8 symbology.
    /// </summary>
    /// <param name="s">The symbology configuration to use for the EAN-8 barcode. Must be of type Ean8Symbology.</param>
    /// <param name="getValue">A delegate that provides the string value to encode in the barcode.</param>
    /// <returns>Returns a <see cref="Barcode1DComposer{Ean8Options}"/> instance configured for EAN-8 barcode generation.</returns>
    private static Barcode1DComposer<Ean8Options> CreateEan8(ISymbology s, Func<string> getValue)
    {
        var sym = (Ean8Symbology)s;

        return new Barcode1DComposer<Ean8Options>(
            Ean8Encoder.Instance,
            getValue,
            BuildEan8Options(sym));
    }

    /// <summary>
    /// Creates a barcode renderer for the GS1-128 symbology using the specified symbology and value provider.
    /// </summary>
    /// <param name="s">The symbology definition to use for configuring the GS1-128 barcode renderer. Must be compatible with GS1-128.</param>
    /// <param name="getValue">A delegate that returns the string value to encode in the barcode when invoked.</param>
    /// <returns>Returns a <see cref="Barcode1DComposer{Gs1_128Options}"/> instance configured for GS1-128 barcode generation.</returns>
    private static Barcode1DComposer<Gs1_128Options> CreateGs1_128(ISymbology s, Func<string> getValue)
    {
        var sym = (GS1_128Symbology)s;

        return new Barcode1DComposer<Gs1_128Options>(
            Gs1_128Encoder.Instance,
            getValue,
            BuildGs1_128Options(sym));
    }

    /// <summary>
    /// Creates a surface renderer for generating ITF-14 barcodes using the specified symbology and value provider.
    /// </summary>
    /// <remarks>Use this method to obtain a renderer for ITF-14 barcodes when custom symbology options or
    /// dynamic values are required.</remarks>
    /// <param name="s">The symbology configuration to use for the ITF-14 barcode. Must be an instance of Itf14Symbology.</param>
    /// <param name="getValue">A delegate that returns the string value to encode in the barcode.</param>
    /// <returns>Returns a <see cref="Barcode1DComposer{Itf14Options}"/> instance configured for ITF-14 barcode generation.</returns>
    private static Barcode1DComposer<Itf14Options> CreateItf14(ISymbology s, Func<string> getValue)
    {
        var sym = (Itf14Symbology)s;

        return new Barcode1DComposer<Itf14Options>(
            Itf14Encoder.Instance,
            getValue,
            BuildItf14Options(sym),
            new Itf14Barcode1DLayerComposer());
    }

    /// <summary>
    /// Creates a new barcode renderer for the Codabar symbology using the specified symbology settings and value
    /// provider.
    /// </summary>
    /// <param name="s">The symbology configuration to use for rendering the Codabar barcode. Must be compatible with Codabar encoding.</param>
    /// <param name="getValue">A delegate that returns the string value to encode in the barcode. The returned value should conform to Codabar
    /// format requirements.</param>
    /// <returns>A barcode renderer instance configured for Codabar symbology and ready to generate barcodes using the provided
    /// value.</returns>
    private static Barcode1DComposer<CodabarOptions> CreateCodabar(ISymbology s, Func<string> getValue)
    {
        var sym = (CodabarSymbology)s;

        return new Barcode1DComposer<CodabarOptions>(
            CodabarEncoder.Instance,
            getValue,
            BuildCodabarOptions(sym));
    }

    /// <summary>
    /// Creates a QR code renderer using the specified symbology and value provider.
    /// </summary>
    /// <param name="s">The symbology to use for QR code generation. Must be a QR code-compatible symbology.</param>
    /// <param name="getValue">A delegate that returns the string value to encode in the QR code.</param>
    /// <returns>A Barcode2DRenderer instance configured for QR code generation with the provided options and value provider.</returns>
    private static Barcode2DComposer<QRCodeOptions> CreateQRCode(ISymbology s, Func<string> getValue)
    {
        var sym = (QRCodeSymbology)s;

        return new Barcode2DComposer<QRCodeOptions>(
            QRCodeEncoder.Instance,
            getValue,
            BuildQRCodeOptions(sym));
    }

    /// <summary>
    /// Creates a new instance of a PDF417 barcode renderer using the specified symbology and value provider.
    /// </summary>
    /// <remarks>The returned renderer uses the provided symbology settings and value provider to generate
    /// PDF417 barcodes. Ensure that the symbology parameter is compatible with PDF417.</remarks>
    /// <param name="s">The symbology configuration to use for the PDF417 barcode. Must be an instance of Pdf417Symbology.</param>
    /// <param name="getValue">A delegate that returns the string value to encode in the barcode.</param>
    /// <returns>A Barcode2DRenderer configured for PDF417 barcodes with the specified options and value provider.</returns>
    private static Barcode1DStackedComposer<Pdf417Options> CreatePdf417(ISymbology s, Func<string> getValue)
    {
        var sym = (Pdf417Symbology)s;
        return new Barcode1DStackedComposer<Pdf417Options>(
            Pdf417Encoder.Instance,
            getValue,
            BuildPdf417Options(sym));
    }

    /// <summary>
    /// Creates a new instance of the Code128Options class using the specified Code128Symbology settings.
    /// </summary>
    /// <param name="s">The Code128Symbology object containing the configuration values to apply to the options.</param>
    /// <returns>A Code128Options object initialized with values from the specified symbology.</returns>
    private static Code128Options BuildCode128Options(Code128Symbology s) => new()
    {
        ModuleHeight = s.ModuleHeight,
        ModuleWidth = s.ModuleWidth,
        Subset = s.Subset,
        ShowText = s.ShowText
    };

    /// <summary>
    /// Updates the specified Code128Options instance with values from the provided Code128Symbology.
    /// </summary>
    /// <remarks>This method copies relevant properties from the symbology to the options object, overwriting
    /// any existing values in the options.</remarks>
    /// <param name="opt">The Code128Options instance to update with new values.</param>
    /// <param name="s">The Code128Symbology containing the values to apply to the options.</param>
    private static void UpdateCode128Options(Code128Options opt, Code128Symbology s)
    {
        opt.ModuleHeight = s.ModuleHeight;
        opt.ModuleWidth = s.ModuleWidth;
        opt.Subset = s.Subset;
        opt.ShowText = s.ShowText;
    }

    /// <summary>
    /// Creates a new instance of the Code39Options class using the specified Code39 symbology settings.
    /// </summary>
    /// <param name="s">The Code39Symbology object containing the configuration values to apply to the options.</param>
    /// <returns>A Code39Options object initialized with the values from the provided symbology.</returns>
    private static Code39Options BuildCode39Options(Code39Symbology s) => new()
    {
        ModuleHeight = s.ModuleHeight,
        ModuleWidth = s.ModuleWidth,
        EnableChecksum = s.EnableChecksum,
        WideRatio = s.WideRatio,
        IsExtended = s.IsExtended
    };

    /// <summary>
    /// Updates the specified Code39 options with values from the provided symbology settings.
    /// </summary>
    /// <param name="opt">The Code39 options object to update. This object will have its properties set based on the values from the
    /// symbology parameter.</param>
    /// <param name="s">The Code39 symbology settings whose values are used to update the options object.</param>
    private static void UpdateCode39Options(Code39Options opt, Code39Symbology s)
    {
        opt.ModuleHeight = s.ModuleHeight;
        opt.ModuleWidth = s.ModuleWidth;
        opt.EnableChecksum = s.EnableChecksum;
        opt.WideRatio = s.WideRatio;
        opt.IsExtended = s.IsExtended;
    }

    /// <summary>
    /// Creates a new instance of the UpcAOptions class using the specified UPC-A symbology settings.
    /// </summary>
    /// <param name="s">The UPC-A symbology configuration from which to initialize the options. Cannot be null.</param>
    /// <returns>A new UpcAOptions object initialized with values from the provided symbology.</returns>
    private static UpcAOptions BuildUpcAOptions(UpcASymbology s) => new()
    {
        ModuleHeight = s.ModuleHeight,
        ModuleWidth = s.ModuleWidth
    };

    /// <summary>
    /// Updates the specified UPC-A options with values from the provided symbology settings.
    /// </summary>
    /// <param name="opt">The UPC-A options object to update. This object will have its properties set based on the provided symbology.</param>
    /// <param name="s">The symbology settings containing the values to apply to the options object.</param>
    private static void UpdateUpcAOptions(UpcAOptions opt, UpcASymbology s)
    {
        opt.ModuleHeight = s.ModuleHeight;
        opt.ModuleWidth = s.ModuleWidth;
    }

    /// <summary>
    /// Crée une nouvelle instance de UpcEOptions à partir des propriétés spécifiées d'un objet UpcESymbology.
    /// </summary>
    /// <param name="s">L'objet UpcESymbology à partir duquel les options UPC-E sont extraites. Ne peut pas être null.</param>
    /// <returns>Une instance de UpcEOptions initialisée avec les valeurs de hauteur de module, largeur de module et système
    /// numérique provenant de l'objet spécifié.</returns>
    private static UpcEOptions BuildUpcEOptions(UpcESymbology s) => new()
    {
        ModuleHeight = s.ModuleHeight,
        ModuleWidth = s.ModuleWidth,
        NumberSystem = s.NumberSystem
    };

    /// <summary>
    /// Updates the specified UPC-E options with values from the provided symbology settings.
    /// </summary>
    /// <param name="opt">The UPC-E options object to update. This object will have its properties set based on the values from the
    /// symbology parameter.</param>
    /// <param name="s">The UPC-E symbology settings containing the values to apply to the options object.</param>
    private static void UpdateUpcEOptions(UpcEOptions opt, UpcESymbology s)
    {
        opt.ModuleHeight = s.ModuleHeight;
        opt.ModuleWidth = s.ModuleWidth;
        opt.NumberSystem = s.NumberSystem;
    }

    /// <summary>
    /// Creates a new instance of the Code93Options class using the specified Code93Symbology settings.
    /// </summary>
    /// <param name="s">The Code93Symbology instance containing the configuration values to apply. Cannot be null.</param>
    /// <returns>A Code93Options object initialized with the module height, module width, extended mode, and text display
    /// settings from the specified symbology.</returns>
    private static Code93Options BuildCode93Options(Code93Symbology s) => new()
    {
        ModuleHeight = s.ModuleHeight,
        ModuleWidth = s.ModuleWidth,
        IsExtended = s.IsExtended,
        ShowText = s.ShowText
    };

    /// <summary>
    /// Updates the specified Code93Options instance with values from the provided Code93Symbology.
    /// </summary>
    /// <param name="opt">The Code93Options instance to update with new values.</param>
    /// <param name="s">The Code93Symbology instance containing the values to apply.</param>
    private static void UpdateCode93Options(Code93Options opt, Code93Symbology s)
    {
        opt.ModuleHeight = s.ModuleHeight;
        opt.ModuleWidth = s.ModuleWidth;
        opt.IsExtended = s.IsExtended;
        opt.ShowText = s.ShowText;
    }

    /// <summary>
    /// Creates a new instance of the Ean13Options class using the specified Ean13Symbology settings.
    /// </summary>
    /// <param name="s">The Ean13Symbology object containing the module height and width settings to apply.</param>
    /// <returns>A new Ean13Options instance configured with the module height and width from the specified symbology.</returns>
    private static Ean13Options BuildEan13Options(Ean13Symbology s) => new()
    {
        ModuleHeight = s.ModuleHeight,
        ModuleWidth = s.ModuleWidth
    };

    /// <summary>
    /// Updates the specified Ean13Options instance with module height and width values from the provided
    /// Ean13Symbology.
    /// </summary>
    /// <param name="opt">The Ean13Options instance to update with new module height and width values.</param>
    /// <param name="s">The Ean13Symbology instance containing the module height and width values to apply.</param>
    private static void UpdateEan13Options(Ean13Options opt, Ean13Symbology s)
    {
        opt.ModuleHeight = s.ModuleHeight;
        opt.ModuleWidth = s.ModuleWidth;
    }

    /// <summary>
    /// Creates a new instance of the Ean8Options class using the specified Ean8Symbology settings.
    /// </summary>
    /// <param name="s">The Ean8Symbology instance containing the configuration values to apply.</param>
    /// <returns>A new Ean8Options object initialized with values from the specified Ean8Symbology.</returns>
    private static Ean8Options BuildEan8Options(Ean8Symbology s) => new()
    {
        ModuleHeight = s.ModuleHeight,
        ModuleWidth = s.ModuleWidth
    };

    /// <summary>
    /// Updates the specified Ean8Options instance with module height and width values from the provided Ean8Symbology.
    /// </summary>
    /// <param name="opt">The Ean8Options instance to update with new module height and width values.</param>
    /// <param name="s">The Ean8Symbology instance containing the module height and width values to apply.</param>
    private static void UpdateEan8Options(Ean8Options opt, Ean8Symbology s)
    {
        opt.ModuleHeight = s.ModuleHeight;
        opt.ModuleWidth = s.ModuleWidth;
    }

    /// <summary>
    /// Crée une nouvelle instance de Gs1_128Options à partir des paramètres spécifiés du symbologie GS1-128.
    /// </summary>
    /// <param name="s">Le symbologie GS1-128 à partir duquel les options doivent être extraites. Ne peut pas être null.</param>
    /// <returns>Une instance de Gs1_128Options initialisée avec les valeurs du symbologie fourni.</returns>
    private static Gs1_128Options BuildGs1_128Options(GS1_128Symbology s) => new()
    {
        ModuleHeight = s.ModuleHeight,
        ModuleWidth = s.ModuleWidth,
        ShowText = s.ShowText,
        ValidationMode = s.ValidationMode
    };

    /// <summary>
    /// Updates the specified GS1-128 options with values from the provided symbology settings.
    /// </summary>
    /// <param name="opt">The options object to update with new GS1-128 configuration values. Cannot be null.</param>
    /// <param name="s">The symbology settings containing the values to apply to the options object. Cannot be null.</param>
    private static void UpdateGs1_128Options(Gs1_128Options opt, GS1_128Symbology s)
    {
        opt.ModuleHeight = s.ModuleHeight;
        opt.ModuleWidth = s.ModuleWidth;
        opt.ShowText = s.ShowText;
        opt.ValidationMode = s.ValidationMode;
    }

    /// <summary>
    /// Creates a new instance of the Itf14Options class using the specified ITF-14 symbology settings.
    /// </summary>
    /// <param name="s">The ITF-14 symbology configuration from which to initialize the options. Cannot be null.</param>
    /// <returns>A new Itf14Options object initialized with values from the specified symbology.</returns>
    private static Itf14Options BuildItf14Options(Itf14Symbology s) => new()
    {
        ModuleHeight = s.ModuleHeight,
        ModuleWidth = s.ModuleWidth,
        ShowText = s.ShowText
    };

    /// <summary>
    /// Updates the specified ITF-14 options with values from the provided symbology settings.
    /// </summary>
    /// <param name="opt">The options object to update with new module height, module width, and text display settings. Cannot be null.</param>
    /// <param name="s">The symbology settings from which to copy the module height, module width, and text display values. Cannot be
    /// null.</param>
    private static void UpdateItf14Options(Itf14Options opt, Itf14Symbology s)
    {
        opt.ModuleHeight = s.ModuleHeight;
        opt.ModuleWidth = s.ModuleWidth;
        opt.ShowText = s.ShowText;
    }

    /// <summary>
    /// Creates a new instance of the CodabarOptions class using the specified CodabarSymbology settings.
    /// </summary>
    /// <param name="s">The CodabarSymbology instance containing the configuration values to apply.</param>
    /// <returns>A CodabarOptions object initialized with values from the specified CodabarSymbology.</returns>
    private static CodabarOptions BuildCodabarOptions(CodabarSymbology s) => new()
    {
        ModuleHeight = s.ModuleHeight,
        ModuleWidth = s.ModuleWidth,
        WideRatio = s.WideRatio,
        ShowText = s.ShowText
    };

    /// <summary>
    /// Updates the specified Codabar options with values from the provided symbology settings.
    /// </summary>
    /// <param name="opt">The Codabar options instance to update. The properties of this object will be set based on the values from the
    /// symbology parameter.</param>
    /// <param name="s">The Codabar symbology settings whose values are used to update the options.</param>
    private static void UpdateCodabarOptions(CodabarOptions opt, CodabarSymbology s)
    {
        opt.ModuleHeight = s.ModuleHeight;
        opt.ModuleWidth = s.ModuleWidth;
        opt.ShowText = s.ShowText;
        opt.WideRatio = s.WideRatio;
    }

    /// <summary>
    /// Creates a new instance of the QRCodeOptions class using the specified QR code symbology settings.
    /// </summary>
    /// <param name="s">The QRCodeSymbology object containing the configuration values to apply to the QRCodeOptions instance.</param>
    /// <returns>A QRCodeOptions object initialized with the module height, module width, subset, and text display settings from
    /// the provided symbology.</returns>
    private static QRCodeOptions BuildQRCodeOptions(QRCodeSymbology s) => new()
    {
        EncodingMode = s.EncodingMode,
        ErrorCorrection = s.ErrorCorrection,
        ModuleSize = s.ModuleSize,
        Version = s.Version
    };

    /// <summary>
    /// Updates the specified QR code options to match the provided symbology settings.
    /// </summary>
    /// <param name="opt">The QR code options instance to update. The properties of this object will be set based on the values in
    /// <paramref name="s"/>.</param>
    /// <param name="s">The QR code symbology containing the encoding mode, error correction level, module size, and version to apply.</param>
    private static void UpdateQRCodeOptions(QRCodeOptions opt, QRCodeSymbology s)
    {
        opt.EncodingMode = s.EncodingMode;
        opt.ErrorCorrection = s.ErrorCorrection;
        opt.ModuleSize = s.ModuleSize;
        opt.Version = s.Version;
    }

    /// <summary>
    /// Creates a new instance of the QRCodeOptions class using the specified QR code symbology settings.
    /// </summary>
    /// <param name="s">The QRCodeSymbology object containing the configuration values to apply to the QRCodeOptions instance.</param>
    /// <returns>A QRCodeOptions object initialized with the module height, module width, subset, and text display settings from
    /// the provided symbology.</returns>
    private static Pdf417Options BuildPdf417Options(Pdf417Symbology s) => new()
    {
        AutoMicroGrid = s.AutoMicroGrid,
        Columns = s.Columns,
        Compact = s.Compact,
        ErrorLevel = s.ErrorLevel,
        MicroGrid = s.MicroGrid,
        Mode = s.Mode,
        ModuleWidth = s.ModuleWidth,
        ModuleHeight = s.ModuleHeight,
        Rows = s.Rows
    };

    /// <summary>
    /// Updates the specified <see cref="Pdf417Options"/> instance with values from the provided <see
    /// cref="Pdf417Symbology"/>.
    /// </summary>
    /// <param name="opt">The <see cref="Pdf417Options"/> instance to update with new settings.</param>
    /// <param name="s">The <see cref="Pdf417Symbology"/> instance containing the values to apply.</param>
    private static void UpdatePdf417Options(Pdf417Options opt, Pdf417Symbology s)
    {
        opt.AutoMicroGrid = s.AutoMicroGrid;
        opt.Columns = s.Columns;
        opt.Compact = s.Compact;
        opt.ErrorLevel = s.ErrorLevel;
        opt.MicroGrid = s.MicroGrid;
        opt.Mode = s.Mode;
        opt.ModuleHeight = s.ModuleHeight;
        opt.ModuleWidth = s.ModuleWidth;
        opt.Rows = s.Rows;
    }
}
