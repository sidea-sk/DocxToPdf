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
    }
}
