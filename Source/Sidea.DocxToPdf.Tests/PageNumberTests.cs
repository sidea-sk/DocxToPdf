using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sidea.DocxToPdf.Tests
{
    [TestClass]
    public class PageNumberTests : TestBase
    {
        public PageNumberTests() : base("PageNumbers", useNextGeneration: true)
        {
        }

        [TestMethod]
        public void PageNumber()
        {
            this.Generate(nameof(PageNumber));
        }

        [TestMethod]
        public void PageNumberTotalPages()
        {
            this.Generate(nameof(PageNumberTotalPages));
        }

        [TestMethod]
        public void PageNumberTotalPages_Over10()
        {
            this.Generate(nameof(PageNumberTotalPages_Over10));
        }

        [TestMethod]
        public void TotalPages_ReconstructParagraph()
        {
            this.Generate(nameof(TotalPages_ReconstructParagraph));
        }

        [TestMethod]
        public void TotalPages_ReconstructMultipleParagraphs()
        {
            this.Generate(nameof(TotalPages_ReconstructMultipleParagraphs));
        }
    }
}
