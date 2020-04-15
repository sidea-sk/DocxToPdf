using DocumentFormat.OpenXml.Wordprocessing;
using PdfSharp.Drawing;
using Sidea.DocxToPdf.Renderers.Borders;
using Sidea.DocxToPdf.Renderers.Core;

namespace Sidea.DocxToPdf.Renderers.Tables.Models
{
    internal class RCell : IRenderer
    {
        private readonly TableCell _cell;
        private readonly GridPosition _gridPosition;
        private readonly RBorder _border;
        private readonly IGridPositionService _positionService;

        public RCell(
            TableCell cell,
            GridPosition gridPosition,
            RBorder border,
            IGridPositionService positionService)
        {
            _cell = cell;
            _gridPosition = gridPosition;
            _border = border;
            _positionService = positionService;
        }

        public RenderingState Prepare(IRenderArea renderArea)
        {
            var width = _positionService.CalculateWidth(_gridPosition);
            return new RenderingState(RenderingStatus.Done, new XPoint(width, XUnit.FromPoint(10)));
        }

        public RenderingState Render(IRenderArea renderArea)
        {
            if(_gridPosition.RowSpan == 0)
            {
                return new RenderingState(RenderingStatus.Done, new XPoint(0 ,0));
            }

            var dx = _positionService.CalculateLeftOffset(_gridPosition);
            var dy = _gridPosition.Row * XUnit.FromPoint(10);
            var width = _positionService.CalculateWidth(_gridPosition);
            var height = XUnit.FromPoint(10);

            var cellArea = renderArea.PanLeftDown(new XSize(dx, dy));
            _border.Render(cellArea, new XRect(0, 0, width, height));

            return new RenderingState(RenderingStatus.Done, new XPoint(height, width));
        }
    }
}
