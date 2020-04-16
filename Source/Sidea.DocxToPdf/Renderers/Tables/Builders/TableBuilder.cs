using System.Collections.Generic;
using System.Linq;
using DocumentFormat.OpenXml.Wordprocessing;
using PdfSharp.Drawing;
using Sidea.DocxToPdf.Renderers.Borders;
using Sidea.DocxToPdf.Renderers.Tables.Models;

namespace Sidea.DocxToPdf.Renderers.Tables.Builders
{
    internal static class TableBuilder
    {
        public static RTableGrid InitializeGrid(this Table table)
        {
            var columnWidths = table
               .GetGridColumnWidths()
               .ToArray();

            return new RTableGrid(columnWidths);
        }

        public static IEnumerable<RCell> RCells(this TableRow row, int rowIndex, IGridPositionService gridPositionService)
        {
            var rowHeight = row.RowHeight();

            var cells = new List<RCell>();
            var pen = new XPen(XPens.Black)
            {
                Width = 0.5
            };

            var border = new RBorder(pen, pen, pen, pen);

            var rowColIndex = 0;
            foreach(var cell in row.Cells())
            {
                var gridDescription = cell.GetGridDescription(rowIndex, rowColIndex);
                rowColIndex += gridDescription.Span;

                cells.Add(new RCell(cell, gridDescription, border, rowHeight, gridPositionService));
            }

            return cells;
        }

        private static XUnit RowHeight(this TableRow row)
        {
            // examine height rule
            var rowHeight = row
                .TableRowProperties?
                .ChildsOfType<TableRowHeight>()
                .FirstOrDefault()
                ?.Val ?? 200;

            return XUnit.FromPoint(rowHeight / 20);
        }
    }
}
