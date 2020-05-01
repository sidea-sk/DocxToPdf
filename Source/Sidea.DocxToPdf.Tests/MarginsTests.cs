using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sidea.DocxToPdf.Tests
{
    [TestClass]
    public class MarginsTests : TestBase
    {
        public MarginsTests() : base("Margins")
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
    }
}
