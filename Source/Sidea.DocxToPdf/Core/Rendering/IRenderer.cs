namespace Sidea.DocxToPdf.Core
{
    internal interface IRenderer
    {
        void CreatePages(/*page configurations*/);

        IRendererPage Get(PageNumber pageNumber);
    }
}
