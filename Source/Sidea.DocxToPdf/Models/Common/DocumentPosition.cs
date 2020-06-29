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

        public DocumentPosition Move(Point offset)
            => this + offset;

        public DocumentPosition MoveX(double xOffset)
            => this + new Point(xOffset, 0);

        public DocumentPosition MoveY(double yOffset)
            => this + new Point(0, yOffset);

        public static DocumentPosition operator+(DocumentPosition position, Point offset)
        {
            return new DocumentPosition(position.PageNumber, position.Offset + offset);
        }
    }
}
