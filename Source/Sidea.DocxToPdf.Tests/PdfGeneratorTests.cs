using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sidea.DocxToPdf.Tests
{
    [TestClass]
    public class PdfGeneratorTests : TestBase
    {
        public PdfGeneratorTests() : base("", useNextGeneration: true)
        {
        }

        [TestMethod]
        public void HelloWorld()
        {
            this.Generate("HelloWorld");
        }
    }
}
