﻿using System.Collections.Generic;
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
        private readonly RendererFactory _factory = new RendererFactory();
        private readonly List<IRenderer> _childRenderers = new List<IRenderer>();
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

        public RenderingState Prepare(IPrerenderArea prerenderArea)
        {
            if (this.GridPosition.RowSpan == 0)
            {
                this.TotalArea = new XSize(0, 0);
                return RenderingState.DoneEmpty;
            }

            var left = _positionService.CalculateLeftOffset(this.GridPosition);

            var prerenderedHeight = XUnit.Zero;

            var cellPrerenderArea = prerenderArea
                .PanLeft(left)
                .Restrict(_width);

            foreach(var child in _cell.RenderableChildren())
            {
                var renderer = _factory.CreateRenderer(child);
                _childRenderers.Add(renderer);
                
                var renderingState = renderer.Prepare(cellPrerenderArea);
                prerenderedHeight += renderingState.RenderedArea.Height;

                if(renderingState.Status == RenderingStatus.ReachedEndOfArea)
                {
                    break;
                }

                cellPrerenderArea.PanLeftDown(new XSize(0, renderingState.RenderedArea.Height));
            }

            this.TotalArea = this.TotalArea.ExpandHeight(prerenderedHeight);
            return RenderingState.Done(_width, prerenderedHeight);
        }

        public RenderingState Render(IRenderArea renderArea)
        {
            if(this.GridPosition.RowSpan == 0)
            {
                return RenderingState.DoneEmpty;
            }

            var cellRenderArea = renderArea;

            var renderedHeight = XUnit.Zero;
            foreach (var renderer in _childRenderers)
            {
                var renderingState = renderer.Render(cellRenderArea);
                renderedHeight += renderingState.RenderedArea.Height;
                if(renderingState.Status == RenderingStatus.ReachedEndOfArea)
                {
                    break;
                }

                cellRenderArea = cellRenderArea
                    .PanLeftDown(new XSize(0, renderingState.RenderedArea.Height));
            }

            return RenderingState.Done(_width, renderedHeight);
        }
    }
}
