using Sidea.DocxToPdf.Core;

namespace Sidea.DocxToPdf.Models.Common
{
    internal class DocumentPosition
    {
        public static readonly DocumentPosition None = new DocumentPosition(PageNumber.None, Point.Zero);

        public DocumentPosition(PageNumber pageNumber, Point offset)
        {
            this.PageNumber = pageNumber;
            this.Offset = offset;
        }

        public PageNumber PageNumber { get; }
        public Point Offset { get; }

        public static DocumentPosition operator+(DocumentPosition position, Point offset)
        {
            return new DocumentPosition(position.PageNumber, position.Offset + offset);
        }
    }
}
