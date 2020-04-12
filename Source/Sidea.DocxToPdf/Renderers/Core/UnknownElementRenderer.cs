using PdfSharp.Drawing;

namespace Sidea.DocxToPdf.Renderers.Core
{
    internal class UnknownElementRenderer : IRenderer
    {
        public RenderingState Render(IRenderArea area) => new RenderingState(RenderingStatus.Error, new XPoint(0, 0));
    }
}
