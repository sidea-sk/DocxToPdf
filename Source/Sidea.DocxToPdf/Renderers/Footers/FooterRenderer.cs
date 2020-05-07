using System;
using DocumentFormat.OpenXml.Wordprocessing;
using PdfSharp.Drawing;
using Sidea.DocxToPdf.Renderers.Core;
using Sidea.DocxToPdf.Renderers.Core.RenderingAreas;

namespace Sidea.DocxToPdf.Renderers.Footers
{
    internal class FooterRenderer : CompositeRenderer, IFooterRenderer
    {
        private readonly XUnit _bottomMargin;
        private readonly XUnit _toFooterMargin;

        public FooterRenderer(
            Footer footer,
            PageMargin pageMargin) : base(footer)
        {
            _bottomMargin = pageMargin.Bottom.ToXUnit();
            _toFooterMargin = pageMargin.Footer.ToXUnit();
        }

        protected override XSize CalculateContentSizeCore(IPrerenderArea prerenderArea)
        {
            var contentSize = base.CalculateContentSizeCore(prerenderArea);
            var h = Math.Max(contentSize.Height + _toFooterMargin, _bottomMargin);
            return contentSize.ExpandToMax(new XSize(prerenderArea.Width, h));
        }

        protected override RenderResult RenderCore(IRenderArea renderArea)
        {
            var footerArea = renderArea
                .PanDown(renderArea.Height - this.PrecalulatedSize.Height);

            // child rendering state ignored. The footer has no chance to extend its height during the rendering
            base.RenderCore(footerArea);
            return RenderResult.Done(renderArea.Width, this.PrecalulatedSize.Height);
        }
    }
}
