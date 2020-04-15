using PdfSharp.Drawing;
using Sidea.DocxToPdf.Renderers.Core.RenderingAreas;

namespace Sidea.DocxToPdf.Renderers.Core
{
    internal class UnknownElementRenderer : IRenderer
    {
        public RenderingState Prepare(IPrerenderArea renderArea) => new RenderingState(RenderingStatus.Error, new XPoint(0, 0));

        public RenderingState Render(IRenderArea area) => new RenderingState(RenderingStatus.Error, new XPoint(0, 0));
    }
}
