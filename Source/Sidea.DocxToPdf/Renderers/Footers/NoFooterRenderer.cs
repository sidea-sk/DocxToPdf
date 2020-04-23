using DocumentFormat.OpenXml.Wordprocessing;
using PdfSharp.Drawing;
using Sidea.DocxToPdf.Renderers.Core;
using Sidea.DocxToPdf.Renderers.Core.RenderingAreas;

namespace Sidea.DocxToPdf.Renderers.Footers
{
    internal class NoFooterRenderer : RendererBase, IFooterRenderer
    {
        private readonly XUnit _bottomMargin;

        public NoFooterRenderer(PageMargin pageMargin)
        {
            _bottomMargin = pageMargin.Bottom.ToXUnit();
        }

        protected override XSize CalculateContentSizeCore(IPrerenderArea prerenderArea)
        {
            return new XSize(prerenderArea.Width, _bottomMargin);
        }

        protected override RenderingState RenderCore(IRenderArea renderArea)
        {
            return RenderingState.Done(renderArea.Width, _bottomMargin);
        }
    }
}
