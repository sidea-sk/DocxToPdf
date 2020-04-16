using PdfSharp.Drawing;

namespace Sidea.DocxToPdf.Renderers.Tables.Models
{
    internal interface IGridPositionService
    {
        public int ColumnsCount { get; }

        public XUnit CalculateWidth(GridPosition description);

        public XUnit CalculateLeftOffset(GridPosition gridPosition);
    }
}
