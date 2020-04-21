using PdfSharp.Drawing;
using Sidea.DocxToPdf.Renderers.Core.RenderingAreas;

namespace Sidea.DocxToPdf.Renderers.Core
{
    internal class UnknownElementRenderer : IRenderer
    {
        public XSize PrecalulatedSize => new XSize(0, 0);

        public XSize RenderedSize => new XSize(0, 0);

        public RenderingState CurrentRenderingState => RenderingState.Unprepared;

        public void CalculateContentSize(IPrerenderArea prerenderArea)
        {
        }

        public void Render(IRenderArea area) { }
    }
}
