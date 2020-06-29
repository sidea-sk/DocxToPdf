using Sidea.DocxToPdf.Core;

namespace Sidea.DocxToPdf.Models
{
    internal class PageRegion
    {
        public static readonly PageRegion None = new PageRegion(PageNumber.None, Rectangle.Empty);

        public PageRegion(PageNumber pageNumber, Rectangle region)
        {
            this.PageNumber = pageNumber;
            this.Region = region;
        }

        public PageNumber PageNumber { get; }
        public Rectangle Region { get; }
    }
}
