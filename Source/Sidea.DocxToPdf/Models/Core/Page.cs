using Sidea.DocxToPdf.Core;

namespace Sidea.DocxToPdf.Models
{
    internal class Page : IPage
    {
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
