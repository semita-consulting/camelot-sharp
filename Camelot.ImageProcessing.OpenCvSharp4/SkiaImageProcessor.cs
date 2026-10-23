using UglyToad.PdfPig;
using UglyToad.PdfPig.Content;
using UglyToad.PdfPig.Rendering;
using UglyToad.PdfPig.Rendering.Skia;

namespace Camelot.ImageProcessing.OpenCvSharp4
{
    /// <summary>
    /// Cross-platform page renderer backed by PdfPig.Rendering.Skia (SkiaSharp).
    /// Replaces the Windows-only <c>System.Drawing</c> implementation so the Lattice
    /// parser can render pages on our Linux hosts.
    /// </summary>
    /// <remarks>
    /// PdfPig's Skia renderer works at the document level (it renders by page number via
    /// the registered Skia page factory), so this adapter holds the owning
    /// <see cref="PdfDocument"/> and bridges it to the page-based
    /// <see cref="IPageImageRenderer"/> contract the processors expect. Pass the same
    /// document instance the pages are fetched from.
    /// </remarks>
    public sealed class SkiaImageProcessor : IPageImageRenderer
    {
        private readonly PdfDocument _document;

        /// <summary>
        /// Creates a renderer for the supplied document and registers the Skia page factory on it.
        /// </summary>
        /// <param name="document">The document whose pages will be rendered. Must be the same
        /// instance the <see cref="Page"/> passed to <see cref="Render"/> is fetched from.</param>
        public SkiaImageProcessor(PdfDocument document)
        {
            _document = document ?? throw new ArgumentNullException(nameof(document));

            // AddSkiaPageFactory throws if a factory is already registered for the document;
            // swallow that so constructing more than one renderer over the same document is safe.
            try
            {
                _document.AddSkiaPageFactory();
            }
            catch (InvalidOperationException)
            {
                // Factory already registered on this document - nothing to do.
            }
        }

        /// <summary>
        /// Renders the given page to PNG bytes. Only <see cref="PdfRendererImageFormat.Png"/>
        /// is supported; the Skia renderer always produces PNG.
        /// </summary>
        public byte[] Render(Page page, double pageScale, PdfRendererImageFormat imageFormat = PdfRendererImageFormat.Png)
        {
            ArgumentNullException.ThrowIfNull(page);

            using var ms = _document.GetPageAsPng(page.Number, (float)pageScale);
            return ms.ToArray();
        }
    }
}
