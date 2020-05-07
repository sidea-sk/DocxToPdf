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

        public static IEnumerable<RCell> RCells(this Table table, IGridPositionService gridPositionService)
        {
            var spans = table.PrepareCellSpans();
            var cells = table
                .Rows()
                .SelectMany((row, index) => 
                {
                    var rowCells = row.RCells(index, gridPositionService, spans);
                    return rowCells;
                })
                .ToArray();
            return cells;
        }

        private static IEnumerable<RCell> RCells(
            this TableRow row,
            int rowIndex,
            IGridPositionService gridPositionService,
            List<GridSpanInfo[]> spans)
        {
            var pen = new XPen(XPens.Black)
            {
                Width = 0.5
            };

            var border = new BorderStyle(pen, pen, pen, pen);

            var cells = row
                .Cells()
                .Select((cell, index) => 
                {
                    var gridPosition = GetCellGridPosition(rowIndex, index, spans);
                    var outerWidth = gridPositionService.CalculateWidth(gridPosition);
                    return new RCell(cell, gridPosition, border, outerWidth);
                })
                .ToArray();

            return cells;
        }

        private static GridPosition GetCellGridPosition(
            int rowIndex,
            int cellIndex,
            List<GridSpanInfo[]> spans)
        {
            var info = spans[rowIndex][cellIndex];

            var rowSpan = info.RowSpan == 1 ? 1 : -1;

            var isLastCellOfRow = true;
            if(rowSpan == 1)
            {
                var ri = rowIndex + 1;
                while (ri < spans.Count)
                {
                    var i = spans[ri].FindSpanInfoForColumn(info.Column);
                    if(i.RowSpan == 1)
                    {
                        break;
                    }

                    isLastCellOfRow = false;
                    rowSpan++;
                    ri++;
                }
            }
            else
            {
                var ri = rowIndex - 1;
                while (ri >= 0)
                {
                    var i = spans[ri].FindSpanInfoForColumn(info.Column);
                    rowSpan--;
                    if(i.RowSpan > 0)
                    {
                        break;
                    }
                    ri--;
                }

                isLastCellOfRow = rowIndex == spans.Count - 1
                    || spans[rowIndex + 1].FindSpanInfoForColumn(info.Column).RowSpan > 0;
            }

            return new GridPosition(rowIndex, info.Column, rowSpan, info.ColSpan, isLastCellOfRow);
        }

        private static List<GridSpanInfo[]> PrepareCellSpans(this Table table)
        {
            var rowSpans = table
                .Rows()
                .Select(row =>
                {
                    var rowColIndex = 0;
                    var rowCellSpans = row
                        .Cells()
                        .Select(cell =>
                        {
                            var (rowSpan, colSpan) = cell.GetCellSpans();
                            var cellInfo = new GridSpanInfo
                            {
                                RowSpan = rowSpan,
                                ColSpan = colSpan,
                                Column = rowColIndex
                            };

                            rowColIndex += colSpan;
                            return cellInfo;
                        })
                            .ToArray();

                    return rowCellSpans;
                })
                .ToList();

            return rowSpans;
        }

        private static GridSpanInfo FindSpanInfoForColumn(this IEnumerable<GridSpanInfo> rowSpanInfos, int column)
        {
            var info = rowSpanInfos
                    .First(i => i.Column <= column && i.Column + i.ColSpan - 1 >= column);

            return info;
        }


        private class GridSpanInfo
        {
            public int RowSpan { get; set; }
            public int ColSpan { get; set; }
            public int Column { get; set; }
        }
    }
}
