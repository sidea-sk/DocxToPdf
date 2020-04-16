using PdfSharp.Drawing;
using Sidea.DocxToPdf.Renderers.Core.RenderingAreas;

namespace Sidea.DocxToPdf.Renderers.Core
{
    internal interface IRenderer
    {
        XSize TotalArea { get; }
        XSize CalculateContentSize(IPrerenderArea prerenderArea);
        RenderingState Prepare(IPrerenderArea area);
        RenderingState Render(IRenderArea renderArea);
    }
}
