using Sidea.DocxToPdf.Models.Common;

namespace Sidea.DocxToPdf.Models.Sections
{
    internal class PageMargin : Margin
    {
        public static readonly PageMargin PageNone = new PageMargin(0, 0, 0, 0, 0, 0);

        public PageMargin(double top, double right, double bottom, double left, double header, double footer) : base(top, right, bottom, left)
        {
            this.Header = header;
            this.Footer = footer;
        }

        public double Header { get; }
        public double Footer { get; }
    }
}
