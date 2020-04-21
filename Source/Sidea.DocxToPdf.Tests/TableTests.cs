using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sidea.DocxToPdf.Tests
{
    [TestClass]
    public class TableTests : TestBase
    {
        public TableTests() : base("Tables")
        {
        }

        [TestMethod]
        public void Table()
        {
            this.Generate(nameof(Table));
        }

        [TestMethod]
        public void Layout()
        {
            this.Generate(nameof(Layout));
        }

        [TestMethod]
        public void TableWithParagraphsSM()
        {
            this.Generate(nameof(TableWithParagraphsSM));
        }

        [TestMethod]
        public void TableWithParagraphsXL()
        {
            this.Generate(nameof(TableWithParagraphsXL));
        }

        [TestMethod]
        public void TableWithParagraphsXXL()
        {
            this.Generate(nameof(TableWithParagraphsXXL));
        }

        [TestMethod]
        public void TableWithParagraphsXXXL()
        {
            this.Generate(nameof(TableWithParagraphsXXXL));
        }

        [TestMethod]
        public void TableWithTable()
        {
            this.Generate(nameof(TableWithTable));
        }

        [TestMethod]
        public void TableAlignment()
        {
            this.Generate(nameof(TableAlignment));
        }
    }
}
