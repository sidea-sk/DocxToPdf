using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sidea.DocxToPdf.Tests
{
    [TestClass]
    public class PageBreakTests : TestBase
    {
        public PageBreakTests() : base("PageBreaks", useNextGeneration: true)
        {
            this.Options = new RenderingOptions(
                hiddenChars: true,
                paragraphBorders: RenderingOptions.ParagraphDefault);
        }

        [TestMethod]
        public void PageBreak()
        {
            this.Generate(nameof(PageBreak));
        }
    }
}
