namespace Sidea.DocxToPdf.Renderers.Core
{
    internal class UnknownElementRenderer : IRenderer
    {
        public RenderingStatus Render() => RenderingStatus.Error;
    }
}
