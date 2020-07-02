using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sidea.DocxToPdf.Tests
{
    [TestClass]
    public class SectionTests : TestBase
    {
        public SectionTests() : base("Sections")
        {
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
    }
}
