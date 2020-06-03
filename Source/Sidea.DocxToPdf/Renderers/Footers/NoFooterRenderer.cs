using PdfSharp.Drawing;
using Sidea.DocxToPdf.Renderers.Common;
using Sidea.DocxToPdf.Renderers.Core;
using Sidea.DocxToPdf.Renderers.Core.RenderingAreas;

namespace Sidea.DocxToPdf.Renderers.Footers
{
    internal class NoFooterRenderer : RendererBase, IFooterRenderer
    {
        private readonly XUnit _bottomMargin;

        public NoFooterRenderer(PageConfiguration pageConfiguration)
        {
            _bottomMargin = pageConfiguration.Margin.Bottom;
        }

        protected override XSize CalculateContentSizeCore(IPrerenderArea prerenderArea)
        {
            return new XSize(prerenderArea.Width, _bottomMargin);
        }

        protected override RenderResult RenderCore(IRenderArea renderArea)
        {
            return RenderResult.Done(renderArea.Width, _bottomMargin);
        }
    }
}
