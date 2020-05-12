using System;
using PdfSharp.Drawing;
using Sidea.DocxToPdf.Renderers.Common;
using Sidea.DocxToPdf.Renderers.Core;
using Sidea.DocxToPdf.Renderers.Core.RenderingAreas;
using Word = DocumentFormat.OpenXml.Wordprocessing;

namespace Sidea.DocxToPdf.Renderers.Headers
{
    internal class HeaderRenderer : CompositeRenderer, IHeaderRenderer
    {
        private readonly XUnit _topMargin;
        private readonly XUnit _toHeaderMargin;

        public HeaderRenderer(
            Word.Header header,
            PageMargin pageMargin) : base(header)
        {
            _toHeaderMargin = pageMargin.Header;
            _topMargin = pageMargin.Top;
        }

        protected override XSize CalculateContentSizeCore(IPrerenderArea prerenderArea)
        {
            var contentSize = base.CalculateContentSizeCore(prerenderArea);
            return contentSize.ExpandToMax(new XSize(prerenderArea.Width, _topMargin));
        }

        protected override RenderResult RenderCore(IRenderArea renderArea)
        {
            var headerContentArea = renderArea
                .PanDown(_toHeaderMargin);

            var state = base.RenderCore(headerContentArea);

            var t = Math.Max(state.RenderedHeight + _toHeaderMargin, _topMargin);

            var size = state.RenderedSize
                .ExpandToMax(new XSize(renderArea.Width, t));

            return RenderResult.FromStatus(state.Status, size);
        }
    }
}
