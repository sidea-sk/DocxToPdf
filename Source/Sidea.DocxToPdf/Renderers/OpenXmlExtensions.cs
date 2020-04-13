using System.Collections.Generic;
using System.Linq;
using DocumentFormat.OpenXml.Wordprocessing;

namespace Sidea.DocxToPdf.Renderers
{
    internal static class OpenXmlExtensions
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

        public static TableCellProperties Properties(this TableCell cell)
        {
            return cell.ChildElements.OfType<TableCellProperties>().Single();
        }

    }
}
