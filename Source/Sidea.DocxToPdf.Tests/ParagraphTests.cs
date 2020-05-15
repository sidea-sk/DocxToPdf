using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sidea.DocxToPdf.Tests
{
    [TestClass]
    public class ParagraphTests : TestBase
    {
        public ParagraphTests(): base("Paragraphs")
        {
        }

        [TestMethod]
        public void Paragraph()
        {
            this.Generate(nameof(Paragraph));
        }

        [TestMethod]
        public void ParagraphAlignments()
        {
            this.Generate(nameof(ParagraphAlignments));
        }

        [TestMethod]
        public void ParagraphFontStyles()
        {
            this.Generate(nameof(ParagraphFontStyles));
        }

        [TestMethod]
        public void DefaultStyles()
        {
            this.Generate(nameof(DefaultStyles));
        }

        [TestMethod]
        public void ParagraphOverPage()
        {
            this.Generate(nameof(ParagraphOverPage));
        }

        [TestMethod]
        public void ParagraphOverPageLandscape()
        {
            this.Generate(nameof(ParagraphOverPageLandscape));
        }

        [TestMethod]
        public void ParagraphLineSpacing()
        {
            this.Generate(nameof(ParagraphLineSpacing));
        }

        [TestMethod]
        public void VeryLongTextWithoutSpaces()
        {
            this.Generate(nameof(VeryLongTextWithoutSpaces));
        }
    }
}
