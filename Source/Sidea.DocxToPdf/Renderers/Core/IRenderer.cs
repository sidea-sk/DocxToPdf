namespace Sidea.DocxToPdf.Renderers.Core
{
    internal interface IRenderer
    {
        RenderingState Render(IRenderArea renderArea);
    }
}
