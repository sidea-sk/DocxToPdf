namespace Sidea.DocxToPdf.Renderers
{
    internal interface IRenderer
    {
        RenderingStatus Render(/*graphics area*/);
    }
}
