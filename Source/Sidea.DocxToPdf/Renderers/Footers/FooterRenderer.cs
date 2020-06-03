using System;
using PdfSharp.Drawing;
using Sidea.DocxToPdf.Renderers.Common;
using Sidea.DocxToPdf.Renderers.Core;
using Sidea.DocxToPdf.Renderers.Core.RenderingAreas;
using Sidea.DocxToPdf.Renderers.Styles;
using Word = DocumentFormat.OpenXml.Wordprocessing;

namespace Sidea.DocxToPdf.Renderers.Footers
{
    internal class FooterRenderer : CompositeRenderer, IFooterRenderer
    {
        private readonly XUnit _leftMargin;
        private readonly XUnit _renderableWidth;
        private readonly XUnit _bottomMargin;
        private readonly XUnit _toFooterMargin;

        public FooterRenderer(
            Word.Footer footer,
            PageConfiguration pageConfiguration,
            IStyleAccessor styleAccessor) : base(footer, styleAccessor)
        {
            _leftMargin = pageConfiguration.Margin.Left;
            _renderableWidth = pageConfiguration.Size.Width
                - pageConfiguration.Margin.Left
                - pageConfiguration.Margin.Right;

            _bottomMargin = pageConfiguration.Margin.Bottom;
            _toFooterMargin = pageConfiguration.Margin.Footer;
        }

        protected override XSize CalculateContentSizeCore(IPrerenderArea prerenderArea)
        {
            var contentSize = base.CalculateContentSizeCore(prerenderArea.Restrict(_renderableWidth));
            var h = Math.Max(contentSize.Height + _toFooterMargin, _bottomMargin);
            return contentSize.ExpandToMax(new XSize(prerenderArea.Width, h));
        }

        protected override RenderResult RenderCore(IRenderArea renderArea)
        {
            var footerArea = renderArea
                .PanLeftDown(_leftMargin, renderArea.Height - this.PrecalulatedSize.Height)
                .Restrict(_renderableWidth);

            // child rendering state ignored. The footer has no chance to extend its height during the rendering
            base.RenderCore(footerArea);
            return RenderResult.Done(renderArea.Width, this.PrecalulatedSize.Height);
        }
    }
}
