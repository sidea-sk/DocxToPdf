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
        public void TableWithContent()
        {
            this.Generate(nameof(TableWithContent));
        }

        [TestMethod]
        public void TableAlignment()
        {
            this.Generate(nameof(TableAlignment));
        }
    }
}
