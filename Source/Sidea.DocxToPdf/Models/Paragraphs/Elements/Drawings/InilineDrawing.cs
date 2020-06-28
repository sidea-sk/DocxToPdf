using Sidea.DocxToPdf.Core;

namespace Sidea.DocxToPdf.Models.Paragraphs
{
    internal class InilineDrawing : ParagraphElement
    {
        private readonly string _imageId;

        // size defined in document
        public InilineDrawing(string imageId, Size size)
        {
            _imageId = imageId;
            this.BoundingBox = new Rectangle(size);
        }

        public override double GetBaseLineOffset()
            => 0;

        //public override void Render()
        //{
        //    this.Renderer.RenderImage(_imageId, this.BoundingBox.TopLeft, this.BoundingBox.Size);
        //}

        public override void SetLineBoundingBox(Rectangle rectangle, double baseLineOffset)
        {
            // this.SetOffset(new Point(rectangle.X, 0));
        }
    }
}
