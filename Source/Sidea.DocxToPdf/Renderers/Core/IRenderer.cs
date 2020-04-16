using PdfSharp.Drawing;
using Sidea.DocxToPdf.Renderers.Core.RenderingAreas;

namespace Sidea.DocxToPdf.Renderers.Core
{
    internal interface IRenderer
    {
        XSize TotalArea { get; }
        XSize CalculateContentSize(IPrerenderArea prerenderArea);
        RenderingState CurrentRenderingState { get; }
        RenderingState Render(IRenderArea renderArea);
    }
}
