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

            var border = new BorderStyle(pen, pen, pen, pen);

            var rowColIndex = 0;
            foreach(var cell in row.Cells())
            {
                var gridPosition = cell.GetGridDescription(rowIndex, rowColIndex);
                rowColIndex += gridPosition.ColumnSpan;
                var outerWidth = gridPositionService.CalculateWidth(gridPosition);
                cells.Add(new RCell(cell, gridPosition, border, outerWidth, renderingOptions));
            }

            return cells;
        }
    }
}
