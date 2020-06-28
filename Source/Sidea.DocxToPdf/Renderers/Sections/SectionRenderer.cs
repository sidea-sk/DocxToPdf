using System;
using System.Collections.Generic;
using System.Linq;
using PdfSharp.Drawing;
using Sidea.DocxToPdf.Renderers.Common;
using Sidea.DocxToPdf.Renderers.Core;
using Sidea.DocxToPdf.Renderers.Core.RenderingAreas;
using Sidea.DocxToPdf.Renderers.Sections.Models;

namespace Sidea.DocxToPdf.Renderers.Sections
{
    internal class SectionRenderer : RendererBase
    {
        private readonly Stack<SectionPart> _sectionParts;
        private SectionPart[] _currentRenderRow = new SectionPart[0];

        public SectionRenderer(SectionData sectionData)
        {
            this.SectionProperties = sectionData.Properties;
            _sectionParts = sectionData.SectionParts.ToStack();
        }

        public SectionProperties SectionProperties { get; }

        private PageMargin Margin => this.SectionProperties.PageConfiguration.Margin;

        protected override XSize CalculateContentSizeCore(IPrerenderArea prerenderArea)
        {
            var currentColumn = 0;
            foreach(var sectionPart in _sectionParts)
            {
                var column = this.SectionProperties.Columns.ElementAt(currentColumn);
                var sectionPartPrerenderArea = prerenderArea.Restrict(column.Width);
                sectionPart.CalculateContentSize(sectionPartPrerenderArea);

                switch(sectionPart.SectionBreak)
                {
                    case SectionBreak.Column:
                        currentColumn = (currentColumn + 1) % this.SectionProperties.Columns.Count;
                        break;
                    case SectionBreak.Page:
                        currentColumn = 0;
                        break;
                }
            }

            return new XSize(0, 0);
        }

        protected override RenderResult RenderCore(IRenderArea renderArea)
        {
            this.RenderSectionBorder(renderArea);

            this.PrepareCurrentRenderRow();

            var renderedHeight = XUnit.Zero;

            for(var i = 0; i < _currentRenderRow.Length; i++)
            {
                var part = _currentRenderRow[i];
                if(part == null || part.RenderResult.Status == RenderingStatus.Done)
                {
                    continue;
                }

                var partRenderArea = renderArea
                    .PanLeft(this.SectionProperties.ColumnLeftMargin(i))
                    .Restrict(this.SectionProperties.ColumnWidth(i));

                part.Render(partRenderArea);

                renderedHeight = Math.Max(renderedHeight, part.RenderResult.RenderedHeight);
            }

            var status = this.GetCurrentRenderingStatus();

            return RenderResult.FromStatus(status, new XSize(renderArea.Width, renderedHeight));
        }

        private void PrepareCurrentRenderRow()
        {
            if(_currentRenderRow.Any(sp => sp.RenderResult.Status == RenderingStatus.ReachedEndOfArea))
            {
                return;
            }

            var currentColumn = 0;
            _currentRenderRow = new SectionPart[this.SectionProperties.Columns.Count];

            while (_sectionParts.Count > 0 && currentColumn < this.SectionProperties.Columns.Count)
            {
                var part = _sectionParts.Pop();
                _currentRenderRow[currentColumn] = part;
                if(part.SectionBreak == SectionBreak.Page)
                {
                    break;
                }
                currentColumn++;
            }
        }

        private RenderingStatus GetCurrentRenderingStatus()
        {
            if(_currentRenderRow.Any(p => p != null && p.RenderResult.Status != RenderingStatus.Done)
                || _sectionParts.Count > 0)
            {
                return RenderingStatus.ReachedEndOfArea;
            }

            return RenderingStatus.Done;
        }

        private void RenderSectionBorder(IRenderArea renderArea)
        {
            if (!renderArea.Options.SectionRegionBoundaries)
            {
                return;
            }

            var sectionRenderArea = renderArea
                .PanLeft(this.Margin.Left)
                .Restrict(renderArea.Width - this.Margin.HorizontalMargins);

            sectionRenderArea.DrawRectangle(XPens.Orange, XBrushes.Transparent, new XRect(sectionRenderArea.AreaRectangle.Size));
        }
    }
}
