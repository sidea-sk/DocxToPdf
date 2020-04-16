using DocumentFormat.OpenXml.Wordprocessing;
using PdfSharp.Drawing;
using Sidea.DocxToPdf.Renderers.Borders;
using Sidea.DocxToPdf.Renderers.Core;
using Sidea.DocxToPdf.Renderers.Core.RenderingAreas;

namespace Sidea.DocxToPdf.Renderers.Tables.Models
{
    internal class RCell : IRenderer
    {
        private readonly TableCell _cell;
        private readonly XUnit _minimalHeight;
        private XUnit _width;
        private readonly IGridPositionService _positionService;

        public RCell(
            TableCell cell,
            GridPosition gridPosition,
            RBorder border,
            XUnit minimalHeight,
            IGridPositionService positionService)
        {
            _cell = cell;
            _positionService = positionService;
            _width = _positionService.CalculateWidth(gridPosition);
            _minimalHeight = minimalHeight;

            this.GridPosition = gridPosition;
            this.Border = border;
            this.TotalArea = new XSize(_width, 0);
        }

        public GridPosition GridPosition { get; }

        public RBorder Border { get; }

        public XSize TotalArea { get; private set; }

        public RenderingState Prepare(IPrerenderArea renderArea)
        {
            if (this.GridPosition.RowSpan == 0)
            {
                this.TotalArea = new XSize(0, 0);
                return RenderingState.DoneEmpty;
            }

            this.TotalArea = this.TotalArea.ExpandHeight(_minimalHeight);
            return RenderingState.Done(_width, _minimalHeight);
        }

        public RenderingState Render(IRenderArea renderArea)
        {
            if(this.GridPosition.RowSpan == 0)
            {
                return RenderingState.DoneEmpty;
            }

            // var dx = _positionService.CalculateLeftOffset(this.GridPosition);
            // var dy = XUnit.FromPoint(10);
            // var width = _positionService.CalculateWidth(this.GridPosition);

            // var cellArea = renderArea.PanLeftDown(new XSize(dx, dy));
            // Border.Render(cellArea, new XRect(0, 0, width, _minimalHeight));

            return RenderingState.Done(_width, _minimalHeight);
        }
    }
}
