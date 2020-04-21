using System.Collections.Generic;
using System.Linq;

namespace Sidea.DocxToPdf.Renderers.Tables.Models
{
    internal static class Extensions
    {
        public static IEnumerable<RCell> VerticalFirstCellsOfRow(this IEnumerable<RCell> cells, int rowIndex)
        {
            return cells.CellsOfRow(rowIndex)
                .Where(c => c.GridPosition.RowSpan > 0);
        }

        public static IEnumerable<RCell> LastCellsOfRow(this IEnumerable<RCell> cells, int rowIndex)
        {
            var temp = cells.ToArray();
            var cellsOfNext = temp
                .VerticalFirstCellsOfRow(rowIndex + 1)
                .ToArray();

            var lastVertical = temp
                .CellsOfRow(rowIndex)
                .Where(c => cellsOfNext.Length == 0 || cellsOfNext.Any(n => n.GridPosition.IsInColumn(c.GridPosition.Column)));

            return lastVertical;
        }

        public static IEnumerable<RCell> CellsOfRow(this IEnumerable<RCell> cells, int rowIndex)
        {
            return cells
                .Where(c => c.GridPosition.IsInRow(rowIndex))
                .OrderBy(c => c.GridPosition.Column);
        }
    }
}
