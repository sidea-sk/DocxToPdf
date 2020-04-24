using PdfSharp.Drawing;
using Sidea.DocxToPdf.Renderers.Core;
using Sidea.DocxToPdf.Renderers.Core.RenderingAreas;

namespace Sidea.DocxToPdf.Renderers.Images
{
    internal class ImageRenderer : RendererBase
    {
        protected override XSize CalculateContentSizeCore(IPrerenderArea prerenderArea)
        {
            return new XSize(0, 0);
        }

        protected override RenderResult RenderCore(IRenderArea renderArea)
        {
            return RenderResult.DoneEmpty;
        }
    }
}
