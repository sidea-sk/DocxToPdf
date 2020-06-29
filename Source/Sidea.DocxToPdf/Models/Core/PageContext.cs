using Sidea.DocxToPdf.Core;

namespace Sidea.DocxToPdf.Models
{
    internal class PageContext
    {
        public PageContext(PageNumber pageNumber, Rectangle region)
        {
            this.PageNumber = pageNumber;
            this.Region = region;
        }

        public PageNumber PageNumber { get; }
        public Rectangle Region { get; }
    }
}
