using Sidea.DocxToPdf.Renderers.Core.RenderingAreas;

namespace Sidea.DocxToPdf.Renderers.Core
{
    internal interface IRenderer
    {
        RenderingState Prepare(IPrerenderArea area);
        RenderingState Render(IRenderArea renderArea);
    }
}
