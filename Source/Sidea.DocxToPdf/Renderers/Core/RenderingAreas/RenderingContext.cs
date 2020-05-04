namespace Sidea.DocxToPdf.Renderers.Core.RenderingAreas
{
    internal class RenderingContext
    {
        public RenderingContext(int currentPage)
        {
            this.CurrentPage = currentPage;
        }

        public int CurrentPage { get; }
    }
}
