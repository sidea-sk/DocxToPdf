namespace Sidea.DocxToPdf.Renderers.Sections.Models
{
    internal class HeaderFooterConfiguration
    {
        public HeaderFooterConfiguration(bool hasTitlePage/*, int pageNumberStart*/)
        {
            this.HasTitlePage = hasTitlePage;
        }

        public bool HasTitlePage { get; }
    }
}
