using Sidea.DocxToPdf.Core;
using Sidea.DocxToPdf.Models.Common;

namespace Sidea.DocxToPdf.Models.Paragraphs
{
    internal class InilineDrawing : LineElement
    {
        private readonly string _imageId;

        // size defined in document
        public InilineDrawing(string imageId, Size size)
        {
            _imageId = imageId;
            this.Size = size;
        }

        public override double GetBaseLineOffset()
            => 0;

        public override void Justify(DocumentPosition position, double baseLineOffset, double lineHeight)
        {
            this.SetPosition(position);
        }

        //public override void Render()
        //{
        //    this.Renderer.RenderImage(_imageId, this.BoundingBox.TopLeft, this.BoundingBox.Size);
        //}
    }
}
