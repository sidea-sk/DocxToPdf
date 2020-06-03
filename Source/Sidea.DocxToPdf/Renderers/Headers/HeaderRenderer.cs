using System;
using PdfSharp.Drawing;
using Sidea.DocxToPdf.Renderers.Common;
using Sidea.DocxToPdf.Renderers.Core;
using Sidea.DocxToPdf.Renderers.Core.RenderingAreas;
using Sidea.DocxToPdf.Renderers.Styles;
using Word = DocumentFormat.OpenXml.Wordprocessing;

namespace Sidea.DocxToPdf.Renderers.Headers
{
    internal class HeaderRenderer : CompositeRenderer, IHeaderRenderer
    {
        private readonly XUnit _leftMargin;
        private readonly XUnit _renderableWidth;
        private readonly XUnit _topMargin;
        private readonly XUnit _toHeaderMargin;

        public HeaderRenderer(
            Word.Header header,
            PageConfiguration pageConfiguration,
            IStyleAccessor styleAccessor) : base(header, styleAccessor)
        {
            _leftMargin = pageConfiguration.Margin.Left;
            _renderableWidth = pageConfiguration.Size.Width
                - pageConfiguration.Margin.Left
                - pageConfiguration.Margin.Right;

            _toHeaderMargin = pageConfiguration.Margin.Header;
            _topMargin = pageConfiguration.Margin.Top;
        }

        protected override XSize CalculateContentSizeCore(IPrerenderArea prerenderArea)
        {
            var contentSize = base.CalculateContentSizeCore(prerenderArea.Restrict(_renderableWidth));
            return contentSize.ExpandToMax(new XSize(prerenderArea.Width, _topMargin));
        }

        protected override RenderResult RenderCore(IRenderArea renderArea)
        {
            var headerContentArea = renderArea
                .PanLeft(_leftMargin)
                .Restrict(_renderableWidth)
                .PanDown(_toHeaderMargin);

            var state = base.RenderCore(headerContentArea);

            var t = Math.Max(state.RenderedHeight + _toHeaderMargin, _topMargin);

            var size = state.RenderedSize
                .ExpandToMax(new XSize(renderArea.Width, t));

            return RenderResult.FromStatus(state.Status, size);
        }
    }
}
