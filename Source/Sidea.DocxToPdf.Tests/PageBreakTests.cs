using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sidea.DocxToPdf.Tests
{
    [TestClass]
    public class PageBreakTests : TestBase
    {
        public PageBreakTests() : base("PageBreaks", useNextGeneration: true)
        {
        }

        [TestMethod]
        public void PageBreak()
        {
            this.Generate(nameof(PageBreak));
        }
    }
}
