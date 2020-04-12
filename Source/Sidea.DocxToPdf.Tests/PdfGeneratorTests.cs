using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sidea.DocxToPdf.Tests
{
    [TestClass]
    public class PdfGeneratorTests : TestBase
    {
        public PdfGeneratorTests() : base("")
        {
        }

        [TestMethod]
        public void HelloWorld()
        {
            this.Generate("HelloWorld");
        }
    }
}
