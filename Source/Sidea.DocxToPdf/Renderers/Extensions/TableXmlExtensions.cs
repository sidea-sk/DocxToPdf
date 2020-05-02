using System.Collections.Generic;
using System.Linq;
using DocumentFormat.OpenXml.Wordprocessing;

namespace Sidea.DocxToPdf.Renderers
{
    internal static class TableXmlExtensions
    {
        public static TableProperties Properties(this Table table)
        {
            return table.ChildElements.OfType<TableProperties>().Single();
        }

        public static IEnumerable<TableRow> Rows(this Table table)
        {
            return table.ChildElements.OfType<TableRow>();
        }

        public static TableRowProperties Properties(this TableRow row)
        {
            return row.ChildElements
                .OfType<TableRowProperties>()
                .FirstOrDefault();
        }

        public static IEnumerable<TableCell> Cells(this TableRow row)
        {
            return row.ChildElements.OfType<TableCell>();
        }

        public static TableGrid Grid(this Table table)
        {
            return table.ChildElements.OfType<TableGrid>().Single();
        }

        public static IEnumerable<GridColumn> Columns(this TableGrid grid)
        {
            return grid.ChildElements.OfType<GridColumn>();
        }

        public static GridSpan GridSpan(this TableCell cell)
        {
            var properties = cell.TableCellProperties;
            return properties.GridSpan ?? new GridSpan() { Val = 1 };
        }
    }
}
