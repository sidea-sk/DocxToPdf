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
        public static Alignment GetAlignment(this TableProperties properties)
        {
            return Alignment.Left;
        }

        public static XUnit GetTableWidth(this TableProperties properties, XUnit availableWidth)
        {
            var w = properties.TableWidth.ToXUnit(availableWidth);
            return w;
        }

        public static IEnumerable<XUnit> GetGridColumnWidths(this Table table)
        {
            var grid = table.Grid();
            var columns = grid.Columns().ToArray();
            var widths = columns
                .Select(c => c.Width.ToXUnit());
            return widths;
        }

        public static XUnit CellWidth(this TableCell cell, int index, IEnumerable<XUnit> columnWidths)
        {
            var gridSpan = cell.GridSpan();
            var colSpan = Convert.ToInt32(gridSpan.Val.Value);
            var width = columnWidths
                .Skip(index)
                .Take(colSpan)
                .Aggregate(XUnit.Zero, (col, acc) => acc + col);

            return width;
        }

        public static GridPosition GetGridDescription(this TableCell cell, int rowIndex, int rowGridColIndex)
        {
            var rowSpan = cell.TableCellProperties.VerticalMerge.ToRowSpan();
            var gridSpan = cell.GridSpan();
            var colSpan = Convert.ToInt32(gridSpan.Val.Value);
            return new GridPosition(rowIndex, rowGridColIndex, rowSpan, colSpan);
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
