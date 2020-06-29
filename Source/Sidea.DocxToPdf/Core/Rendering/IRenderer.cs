namespace Sidea.DocxToPdf.Core
{
    internal interface IRenderer
    {
        void CreatePage(PageNumber pageNumber, PageConfiguration configuration);

        IRendererPage Get(PageNumber pageNumber);
    }
}
