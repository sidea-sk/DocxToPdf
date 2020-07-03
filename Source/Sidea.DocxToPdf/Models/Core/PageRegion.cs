using Sidea.DocxToPdf.Core;
using Sidea.DocxToPdf.Models.Common;

namespace Sidea.DocxToPdf.Models
{
    internal class PageRegion
    {
        public static readonly PageRegion None = new PageRegion(PagePosition.None, Rectangle.Empty);

        public PageRegion(PagePosition pagePosition, Rectangle region)
        {
            this.PagePosition = pagePosition;
            this.Region = region;
        }

        public PagePosition PagePosition { get; }
        // public PageNumber PageNumber { get; }
        public Rectangle Region { get; }
    }
}
