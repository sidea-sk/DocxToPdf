using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sidea.DocxToPdf.Tests
{
    [TestClass]
    public class TableTests : TestBase
    {
        public TableTests() : base("Tables", useNextGeneration: true)
        {
            this.Options = new RenderingOptions(
                hiddenChars: true,
                sectionRegionBoundaries: false);
        }

        [TestMethod]
        public void Table()
        {
            this.Generate(nameof(Table));
        }

        [TestMethod]
        public void TableBorders()
        {
            this.Generate(nameof(TableBorders));
        }

        [TestMethod]
        public void CellBorders()
        {
            this.Generate(nameof(CellBorders));
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
        public void TableInSectionColumns()
        {
            this.Generate(nameof(TableInSectionColumns));
        }

        [TestMethod]
        public void TableOverSectionColumns()
        {
            this.Generate(nameof(TableOverSectionColumns));
        }

        [TestMethod]
        public void TableOverSectionColumnsOverPages()
        {
            this.Generate(nameof(TableOverSectionColumnsOverPages));
        }

        [TestMethod]
        [Ignore]
        public void TableAlignment()
        {
            this.Generate(nameof(TableAlignment));
        }
    }
}
