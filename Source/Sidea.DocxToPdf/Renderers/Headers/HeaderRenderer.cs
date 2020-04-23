using System;
using System.Collections.Generic;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using PdfSharp.Drawing;
using Sidea.DocxToPdf.Renderers.Core;
using Sidea.DocxToPdf.Renderers.Core.RenderingAreas;

namespace Sidea.DocxToPdf.Renderers.Headers
{
    internal class HeaderRenderer : CompositeRenderer, IHeaderRenderer
    {
        private readonly XUnit _minimumMargin = XUnit.FromCentimeter(2.5);
        private readonly int _pageNumber;

        public HeaderRenderer(Header header, int pageNumber, RenderingOptions renderingOptions) : base(header, renderingOptions)
        {
            _pageNumber = pageNumber;
        }

        protected override XSize CalculateContentSizeCore(IPrerenderArea prerenderArea)
        {
            var contentSize = base.CalculateContentSizeCore(prerenderArea);
            return contentSize.ExpandToMax(new XSize(prerenderArea.Width, _minimumMargin));
        }

        protected override RenderingState RenderCore(IRenderArea renderArea)
        {
            var state = base.RenderCore(renderArea);
            var size = state.RenderedSize.ExpandToMax(new XSize(renderArea.Width, _minimumMargin));
            return RenderingState.FromStatus(state.Status, size);
        }
    }
}
