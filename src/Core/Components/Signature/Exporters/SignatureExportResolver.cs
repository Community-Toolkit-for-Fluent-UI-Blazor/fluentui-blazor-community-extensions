using System.Security.Cryptography;
using Microsoft.Extensions.Logging;

namespace FluentUI.Blazor.Community.Components;

internal sealed class SignatureExportResolver
{
    private readonly SignatureConformity _conformity;
    private readonly SignatureExportConfiguration _cfg;
    private readonly ISurfaceRenderTarget _target;
    private readonly ISurfaceImageExporter<StrokeLayerPayload> _imageExporter;
    private readonly ISurfaceTargetBuilder<StrokeLayerPayload> _defaultBuilder;
    private readonly ILogger _logger;

    public SignatureExportResolver(
        ILogger logger,
        SignatureConformity conformity,
        SignatureExportConfiguration cfg,
        ISurfaceRenderTarget? target,
        ISurfaceImageExporter<StrokeLayerPayload> imageExporter,
        ISurfaceTargetBuilder<StrokeLayerPayload>? defaultBuilder = null)
    {
        ArgumentNullException.ThrowIfNull(imageExporter, nameof(imageExporter));
        ArgumentNullException.ThrowIfNull(target, nameof(target));

        _conformity = conformity;
        _cfg = cfg ?? new SignatureExportConfiguration();
        _target = target;
        _defaultBuilder = defaultBuilder ?? new DefaultSurfaceTargetBuilder();
        _imageExporter = imageExporter;
        _logger = logger;
    }

    public ISurfaceExporter<StrokeLayerPayload> Resolve()
    {
        var imageExporter = new PngExporter<StrokeLayerPayload>(
            _target,
            _imageExporter,
            _defaultBuilder);

        var documentExporter = new SvgExporter<StrokeLayerPayload>(new SignatureSvgPayloadBuilder());

        return _conformity switch
        {
            SignatureConformity.SES => throw new InvalidOperationException("SES exporter must be created via CreateSesCompositeExporter()."),
            SignatureConformity.QES => ResolveQesExporter(imageExporter, documentExporter),
            SignatureConformity.AES => ResolveAesExporter(imageExporter, documentExporter),
            _ => throw new InvalidOperationException("Unknown conformity mode.")
        };
    }

    public CompositeSurfaceExporter<StrokeLayerPayload> CreateSesCompositeExporter()
    {
        var composite = new CompositeSurfaceExporter<StrokeLayerPayload>(_logger);

        void AddImage<T>() where T : SurfaceImageExporterBase<StrokeLayerPayload>
        {
            composite.Add((T)Activator.CreateInstance(typeof(T), _target, _imageExporter, _defaultBuilder)!);
        }

        AddImage<AvifExporter<StrokeLayerPayload>>();
        AddImage<BmpExporter<StrokeLayerPayload>>();
        AddImage<HeifExporter<StrokeLayerPayload>>();
        AddImage<JpegExporter<StrokeLayerPayload>>();
        AddImage<TiffExporter<StrokeLayerPayload>>();
        AddImage<WebpExporter<StrokeLayerPayload>>();
        AddImage<PngExporter<StrokeLayerPayload>>();

        composite.Add(new SvgExporter<StrokeLayerPayload>(new SignatureSvgPayloadBuilder()));
        composite.Add(new BinaryExporter<StrokeLayerPayload>());
        composite.Add(new JsonExporter<StrokeLayerPayload>());

        return composite;
    }

    public static SurfaceExportOptions ForcedOptionsForConformity() => new()
    {
        IncludeAxes = false,
        IncludeBackground = false,
        IncludeGrid = false,
        IncludeView = false,
        IncludeWatermark = true,
        Quality = 100
    };

    private FcxSurfaceBinaryExporter<object, StrokeLayerPayload> ResolveQesExporter(
        SurfaceImageExporterBase<StrokeLayerPayload> imageExporter,
        DocumentExporterBase<StrokeLayerPayload> documentExporter)
    {
        if (_cfg.UserData is null ||
            _cfg.Signer is null ||
            _cfg.Encrypter is null ||
            _cfg.EncryptionKey is null ||
            _cfg.ProofBuilder is null)
        {
            throw new InvalidOperationException("QES requires UserData, Signer, Encrypter, EncryptionKey, ProofBuilder.");
        }

        return new FcxSurfaceBinaryExporter<object, StrokeLayerPayload>(
            _cfg.UserData,
            SignatureConformity.QES,
            _cfg.EncryptionKey.Value,
            imageExporter,
            documentExporter,
            _cfg.ProofBuilder);
    }

    private FcxSurfaceBinaryExporter<object, StrokeLayerPayload> ResolveAesExporter(
        SurfaceImageExporterBase<StrokeLayerPayload> imageExporter,
        DocumentExporterBase<StrokeLayerPayload> documentExporter)
    {
        if (_cfg.UserData is null)
        {
            throw new InvalidOperationException("AES requires UserData.");
        }

        var signer = _cfg.Signer ?? new DefaultProofDocumentSigner();
        var encrypter = _cfg.Encrypter ?? new DefaultProofDocumentEncrypter();
        var proofBuilder = _cfg.ProofBuilder ?? new ProofDocumentBuilder<StrokeLayerPayload, object>(signer, encrypter);
        var key = _cfg.EncryptionKey ?? GenerateRandomKey();

        return new FcxSurfaceBinaryExporter<object, StrokeLayerPayload>(
            _cfg.UserData,
            SignatureConformity.AES,
            key,
            imageExporter,
            documentExporter,
            proofBuilder);
    }

    private static ReadOnlyMemory<byte> GenerateRandomKey()
    {
        var key = new byte[32];
        RandomNumberGenerator.Fill(key);
        return key;
    }
}
