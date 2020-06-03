using PdfSharp.Drawing;
using Sidea.DocxToPdf.Renderers.Common;
using Sidea.DocxToPdf.Renderers.Core;
using Sidea.DocxToPdf.Renderers.Core.RenderingAreas;

namespace Sidea.DocxToPdf.Renderers.Headers
{
    internal class NoHeaderRenderer : RendererBase, IHeaderRenderer
    {
        private readonly XUnit _topMargin;

        public NoHeaderRenderer(PageConfiguration pageConfiguration)
        {
            _topMargin = pageConfiguration.Margin.Top;
        }

        protected override XSize CalculateContentSizeCore(IPrerenderArea prerenderArea)
        {
            return new XSize(prerenderArea.Width, _topMargin);
        }

        protected override RenderResult RenderCore(IRenderArea renderArea)
        {
            return RenderResult.Done(renderArea.Width, _topMargin);
        }
    }
}
