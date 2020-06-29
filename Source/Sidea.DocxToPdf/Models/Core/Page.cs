using Sidea.DocxToPdf.Core;

namespace Sidea.DocxToPdf.Models
{
    internal class Page : IPage
    {
        public static readonly Page None = new Page(PageNumber.None, new PageConfiguration(Size.Zero, PageOrientation.Portrait));

        public Page(PageNumber pageNumber, PageConfiguration configuration)
        {
            this.PageNumber = pageNumber;
            this.Configuration = configuration;
        }

        public PageNumber PageNumber { get; }
        public PageConfiguration Configuration { get; }

        public double TopMargin { get; set; }

        public double BottomMargin { get; set; }
    }
}
