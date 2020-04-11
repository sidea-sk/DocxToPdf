using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sidea.DocxToPdf.Tests
{
    [TestClass]
    public class PdfGeneratorTests : TestBase
    {
        [TestMethod]
        public void HelloWorld()
        {
            this.Generate("HelloWorld");
        }
    }
}
