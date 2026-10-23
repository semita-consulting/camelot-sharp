using Camelot.ImageProcessing.OpenCvSharp4;
using UglyToad.PdfPig;
using Xunit;

namespace Camelot.ImageProcessing.Tests
{
    public class SkiaImageProcessorTests
    {
        public SkiaImageProcessorTests()
        {
            Directory.CreateDirectory("Files/Output/");
        }

        [Fact]
        public void DrawScale1()
        {
            using var document = PdfDocument.Open("Files/foo.pdf", new ParsingOptions() { ClipPaths = true });
            var draw = new SkiaImageProcessor(document);
            var page = document.GetPage(1); // always page 1 for the moment

            var png = draw.Render(page, 1);

            Assert.NotNull(png);
            Assert.True(png.Length > 0);
            File.WriteAllBytes("Files/Output/foo_basic_render_1.png", png);
        }

        [Fact]
        public void DrawScale3()
        {
            using var document = PdfDocument.Open("Files/foo.pdf", new ParsingOptions() { ClipPaths = true });
            var draw = new SkiaImageProcessor(document);
            var page = document.GetPage(1); // always page 1 for the moment

            var png = draw.Render(page, 3);

            Assert.NotNull(png);
            Assert.True(png.Length > 0);
            File.WriteAllBytes("Files/Output/foo_basic_render_3.png", png);
        }
    }
}
