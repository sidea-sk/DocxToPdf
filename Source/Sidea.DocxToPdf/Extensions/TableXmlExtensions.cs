using System;
using System.Collections.Generic;
using System.Linq;
using DocumentFormat.OpenXml.Wordprocessing;

namespace Sidea.DocxToPdf
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
            return row.ChildElements
                .Where(c => c is TableCell || c is SdtCell)
                .Select(c =>
                {
                    return c switch
                    {
                        TableCell tc => tc,
                        SdtCell sdt => sdt.SdtContentCell.ChildElements.OfType<TableCell>().First(),
                        _ => throw new RendererException($"Unexpected element {c.GetType().Name} in table row")
                    };
                })
                .Cast<TableCell>();
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

        public static (int rowSpan, int colSpan) GetCellSpans(this TableCell cell)
        {
            var rowSpan = cell.TableCellProperties.VerticalMerge.ToRowSpan();
            var gridSpan = cell.GridSpan();
            var colSpan = Convert.ToInt32(gridSpan.Val.Value);
            return (rowSpan, colSpan);
        }

        private static int ToRowSpan(this VerticalMerge verticalMerge)
        {
            if (verticalMerge == null)
            {
                return 1;
            }

            var value = verticalMerge.Val?.Value ?? MergedCellValues.Continue;
            return (int)value;
        }
    }
}
