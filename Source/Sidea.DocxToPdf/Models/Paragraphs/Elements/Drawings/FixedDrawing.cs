using Sidea.DocxToPdf.Core;
using Sidea.DocxToPdf.Models.Common;

namespace Sidea.DocxToPdf.Models.Paragraphs
{
    internal class FixedDrawing
    {
        private readonly string _id;
        private readonly Point _position;
        private readonly Size _size;

        public FixedDrawing(
            string id,
            Point position,
            Size size,
            Margin margin)
        {
            _id = id;
            _position = position;
            _size = size;

            var p = new Point(_position.X - margin.Left, _position.Y - margin.Top);
            this.BoundingBox = new Rectangle(p, size.Expand(margin.HorizontalMargins, margin.VerticalMargins));
        }

        public Rectangle BoundingBox { get; }
    }
}
