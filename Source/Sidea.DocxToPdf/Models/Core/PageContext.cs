using Sidea.DocxToPdf.Core;
using Sidea.DocxToPdf.Models.Common;

namespace Sidea.DocxToPdf.Models
{
    internal class PageContext
    {
        public PageContext(
            PageNumber pageNumber,
            Rectangle region,
            Variables variables)
        {
            this.PageNumber = pageNumber;
            this.Region = region;
            this.PageVariables = new PageVariables(this.PageNumber, variables.TotalPages);
        }

        public PageNumber PageNumber { get; }
        public Rectangle Region { get; }
        public PageVariables PageVariables { get; }
    }
}
