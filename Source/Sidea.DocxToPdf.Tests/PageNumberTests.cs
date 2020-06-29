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
    }
}
