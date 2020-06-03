using DocumentFormat.OpenXml.Wordprocessing;

namespace Sidea.DocxToPdf.Renderers.Sections.Models
{
    internal class HeaderFooterRef
    {
        public HeaderFooterRef(string id, HeaderFooterValues type)
        {
            this.Id = id;
            this.Type = type;
        }

        public string Id { get; }
        public HeaderFooterValues Type { get; }
    }
}
