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
        public void CellMergeWidth()
        {
            this.Generate(nameof(CellMergeWidth));
        }

        [TestMethod]
        public void TableAlignment()
        {
            this.Generate(nameof(TableAlignment));
        }
    }
}
