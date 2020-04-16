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
        public static RGrid InitializeGrid(this Table table)
        {
            var columnWidths = table
               .GetGridColumnWidths();

            var rowHeights = table
                .ChildsOfType<TableRow>()
                .Select(r => r.ToGridRow());

            return new RGrid(columnWidths, rowHeights);
        }

        public static IEnumerable<RCell> RCells(this TableRow row, int rowIndex, IGridPositionService gridPositionService, RenderingOptions renderingOptions)
        {
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

                cells.Add(new RCell(cell, gridDescription, border, gridPositionService, renderingOptions));
            }

            return cells;
        }
    }
}
