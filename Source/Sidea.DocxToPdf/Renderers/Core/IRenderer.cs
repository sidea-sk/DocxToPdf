namespace Sidea.DocxToPdf.Renderers.Core
{
    internal interface IRenderer
    {
        RenderingState Prepare(IRenderArea renderArea);
        RenderingState Render(IRenderArea renderArea);
    }
}
