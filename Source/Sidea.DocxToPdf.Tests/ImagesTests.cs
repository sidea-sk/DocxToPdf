using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sidea.DocxToPdf.Tests
{
    [TestClass]
    public class ImagesTests : TestBase
    {
        public ImagesTests() : base("Images")
        {
        }

        [TestMethod]
        public void Image()
        {
            this.Generate(nameof(Image));
        }

        [TestMethod]
        public void ImageTextWrapInLine()
        {
            this.Generate(nameof(ImageTextWrapInLine));
        }

        [TestMethod]
        public void ImageTextWrapping()
        {
            this.Generate(nameof(ImageTextWrapping));
        }

        [TestMethod]
        public void ImageTextWrappingLineSpacing()
        {
            this.Generate(nameof(ImageTextWrappingLineSpacing));
        }

        [TestMethod]
        public void ImageResize()
        {
            this.Generate(nameof(ImageResize));
        }
    }
}
