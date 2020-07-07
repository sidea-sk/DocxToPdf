using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sidea.DocxToPdf.Tests
{
    [TestClass]
    public class SectionTests : TestBase
    {
        public SectionTests() : base("Sections", useNextGeneration: true)
        {
            this.Options = new RenderingOptions(
                hiddenChars: true,
                sectionRegionBoundaries: true);
        }

        [TestMethod]
        public void DefaultMargins()
        {
            this.Generate(nameof(DefaultMargins));
        }

        [TestMethod]
        public void ResizedMargins()
        {
            this.Generate(nameof(ResizedMargins));
        }

        [TestMethod]
        public void Margins()
        {
            this.Generate(nameof(Margins));
        }

        [TestMethod]
        public void Columns()
        {
            this.Generate(nameof(Columns));
        }

        [TestMethod]
        public void TextOverMultipleColumns()
        {
            this.Generate(nameof(TextOverMultipleColumns));
        }

        [TestMethod]
        public void TextOverMultipleColumnsOverPages()
        {
            this.Generate(nameof(TextOverMultipleColumnsOverPages));
        }

        [TestMethod]
        public void PageOrientation()
        {
            this.Generate(nameof(PageOrientation));
        }

        [TestMethod]
        public void HeaderFooterOnContinuosSections()
        {
            this.Generate(nameof(HeaderFooterOnContinuosSections));
        }

        [TestMethod]
        public void DifferentHeaderFooterForSections()
        {
            this.Generate(nameof(DifferentHeaderFooterForSections));
        }

        [TestMethod]
        public void DifferentHeaderSameFooterForSections()
        {
            this.Generate(nameof(DifferentHeaderSameFooterForSections));
        }

        [TestMethod]
        public void SameHeaderDifferentFooterForSections()
        {
            this.Generate(nameof(SameHeaderDifferentFooterForSections));
        }

        [TestMethod]
        public void ContinuousSectionsWithMultipleColumns()
        {
            this.Generate(nameof(ContinuousSectionsWithMultipleColumns));
        }
    }
}
