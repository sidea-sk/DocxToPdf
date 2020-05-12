using System;
using System.Collections.Generic;
using System.Linq;
using DocumentFormat.OpenXml.Wordprocessing;
using PdfSharp.Drawing;
using Sidea.DocxToPdf.Renderers.Tables.Models;

namespace Sidea.DocxToPdf.Renderers.Tables.Builders
{
    internal static class Extensions
    {
        public static IEnumerable<XUnit> GetGridColumnWidths(this Table table)
        {
            var grid = table.Grid();
            var columns = grid.Columns().ToArray();
            var widths = columns
                .Select(c => c.Width.ToXUnit());
            return widths;
        }

        public static RGridRow ToGridRow(this TableRow row)
        {
            var trh = row
                .TableRowProperties?
                .ChildsOfType<TableRowHeight>()
                .FirstOrDefault();

            var rowHeight = trh?.Val ?? 200;
            var rule = trh?.HeightType?.Value ?? HeightRuleValues.Auto;

            return new RGridRow(new XUnit(rowHeight / 20), rule);
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
            if(verticalMerge == null)
            {
                return 1;
            }

            var value = verticalMerge.Val?.Value ?? MergedCellValues.Continue;
            return (int)value;
        }
    }
}
