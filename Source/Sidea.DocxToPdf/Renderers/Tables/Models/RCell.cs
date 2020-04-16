using System.Collections.Generic;
using DocumentFormat.OpenXml.Wordprocessing;
using PdfSharp.Drawing;
using Sidea.DocxToPdf.Renderers.Borders;
using Sidea.DocxToPdf.Renderers.Core;
using Sidea.DocxToPdf.Renderers.Core.RenderingAreas;

namespace Sidea.DocxToPdf.Renderers.Tables.Models
{
    internal class RCell : RendererBase
    {
        private readonly TableCell _cell;
        private readonly RendererFactory _factory = new RendererFactory();
        private readonly List<IRenderer> _childRenderers = new List<IRenderer>();
        private XUnit _width;
        private readonly IGridPositionService _positionService;
        private readonly RenderingOptions _renderingOptions;
        private readonly RPadding _padding;

        public RCell(
            TableCell cell,
            GridPosition gridPosition,
            RBorder border,
            IGridPositionService positionService,
            RenderingOptions renderingOptions)
        {
            _cell = cell;
            _padding =  RPadding.Padding(XUnit.FromPoint(1), XUnit.FromPoint(3), XUnit.FromPoint(5), XUnit.FromPoint(3));
            _positionService = positionService;
            _renderingOptions = renderingOptions;
            _width = _positionService.CalculateWidth(gridPosition);

            this.GridPosition = gridPosition;
            this.Border = border;
        }

        public GridPosition GridPosition { get; }

        public RBorder Border { get; }

        protected override sealed XSize CalculateContentSizeCore(IPrerenderArea prerenderArea)
        {
            var size = new XSize(_width, _padding.VerticalPaddings);

            var cellPrerenderArea = prerenderArea
                .Restrict(_width - _padding.HorizonalPaddings);

            foreach (var child in _cell.RenderableChildren())
            {
                var renderer = _factory.CreateRenderer(child, _renderingOptions);
                _childRenderers.Add(renderer);
                var childContent = renderer.CalculateContentSize(cellPrerenderArea);

                size = size.ExpandHeight(childContent.Height);
            }

            return size;
        }

        protected override sealed RenderingState RenderCore(IRenderArea renderArea)
        {
            if(this.GridPosition.RowSpan == 0)
            {
                return RenderingState.DoneEmpty;
            }

            var cellRenderArea = renderArea
                .PanLeftDown(new XSize(_padding.Left, _padding.Top))
                .Restrict(renderArea.Width - _padding.HorizonalPaddings);

            var renderedHeight = XUnit.Zero;
            foreach (var renderer in _childRenderers)
            {
                var renderingState = renderer.Render(cellRenderArea);
                renderedHeight += renderingState.RenderedArea.Height;
                if(renderingState.Status == RenderingStatus.ReachedEndOfArea)
                {
                    return RenderingState.EndOfRenderArea(renderArea.AreaRectangle);
                }

                cellRenderArea = cellRenderArea
                    .PanLeftDown(new XSize(0, renderingState.RenderedArea.Height));
            }

            return RenderingState.Done(_width, renderedHeight);
        }
    }
}
