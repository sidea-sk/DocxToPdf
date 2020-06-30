namespace Sidea.DocxToPdf.Core
{
    internal interface IRenderer
    {
        RenderingOptions Options { get; }

        void CreatePage(PageNumber pageNumber, PageConfiguration configuration);

        IRendererPage Get(PageNumber pageNumber);
    }
}
