using Sidea.DocxToPdf.Core;

namespace Sidea.DocxToPdf.Models.Tables.Borders
{
    internal class BorderCoordinates
    {
        public BorderCoordinates(PageNumber pageNumber, Point start, Point end)
        {
            this.PageNumber = pageNumber;
            this.Start = start;
            this.End = end;
        }

        public PageNumber PageNumber { get; }
        public Point Start { get; }
        public Point End { get; }
    }
}
