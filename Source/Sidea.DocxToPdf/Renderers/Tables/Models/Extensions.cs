using System.Collections.Generic;
using System.Linq;

namespace Sidea.DocxToPdf.Renderers.Tables.Models
{
    internal static class Extensions
    {
        public static IEnumerable<RCell> VerticalFirstCellsOfRow(this IEnumerable<RCell> cells, int rowIndex)
        {
            return cells.CellsOfRow(rowIndex)
                .Where(c => c.GridPosition.IsFirstVerticalCellOfRow(rowIndex));
        }

        public static IEnumerable<RCell> LastCellsOfRow(this IEnumerable<RCell> cells, int rowIndex)
        {
            return cells
               .Where(c => c.GridPosition.IsLastVerticalCellOfRow(rowIndex));
        }

        public static IEnumerable<RCell> CellsOfRow(this IEnumerable<RCell> cells, int rowIndex)
        {
            return cells
                .Where(c => c.GridPosition.IsInRow(rowIndex))
                .OrderBy(c => c.GridPosition.Column);
        }
    }
}
