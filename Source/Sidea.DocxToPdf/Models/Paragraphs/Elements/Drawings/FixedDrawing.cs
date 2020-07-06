using Sidea.DocxToPdf.Core;
using Sidea.DocxToPdf.Models.Common;

namespace Sidea.DocxToPdf.Models.Paragraphs
{
    internal class FixedDrawing
    {
        private readonly string _imageId;
        private readonly Point _offsetFromParent;
        private readonly Size _size;
        private readonly IImageAccessor _imageAccessor;

        public FixedDrawing(
            string imageId,
            Point position,
            Size size,
            Margin margin,
            IImageAccessor imageAccessor)
        {
            _imageId = imageId;
            _offsetFromParent = position;
            _size = size;
            _imageAccessor = imageAccessor;
            var p = new Point(_offsetFromParent.X - margin.Left, _offsetFromParent.Y - margin.Top);
            this.BoundingBox = new Rectangle(p, size.Expand(margin.HorizontalMargins, margin.VerticalMargins));
        }

        public Rectangle BoundingBox { get; }

        public void Render(IRendererPage page)
        {
            var stream = _imageAccessor.GetImageStream(_imageId);
            page.RenderImage(stream, _offsetFromParent, _size);
        }
    }
}
