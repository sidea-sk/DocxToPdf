using DocumentFormat.OpenXml.Wordprocessing;
using PdfSharp.Drawing;
using Sidea.DocxToPdf.Renderers.Core;
using Sidea.DocxToPdf.Renderers.Core.RenderingAreas;

namespace Sidea.DocxToPdf.Renderers.Headers
{
    internal class NoHeaderRenderer : RendererBase, IHeaderRenderer
    {
        private readonly XUnit _topMargin;

        public NoHeaderRenderer(PageMargin pageMargin)
        {
            _topMargin = pageMargin.Top.ToXUnit();
        }

        protected override XSize CalculateContentSizeCore(IPrerenderArea prerenderArea)
        {
            return new XSize(prerenderArea.Width, _topMargin);
        }

        protected override RenderingState RenderCore(IRenderArea renderArea)
        {
            return RenderingState.Done(renderArea.Width, _topMargin);
        }
    }
}
