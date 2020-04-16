using PdfSharp.Drawing;
using Sidea.DocxToPdf.Renderers.Core.RenderingAreas;

namespace Sidea.DocxToPdf.Renderers.Core
{
    internal class UnknownElementRenderer : IRenderer
    {
        public XSize TotalArea => new XSize(0, 0);

        public XSize CalculateContentSize(IPrerenderArea prerenderArea)
        {
            return new XSize(0, 0);
        }

        public RenderingState Prepare(IPrerenderArea renderArea) => RenderingState.Error;

        public RenderingState Render(IRenderArea area) => RenderingState.Error;
    }
}
