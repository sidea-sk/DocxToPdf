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
    }
}
