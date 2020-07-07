using Sidea.DocxToPdf.Core;
using Sidea.DocxToPdf.Models.Common;

namespace Sidea.DocxToPdf.Models.Paragraphs
{
    internal class InilineDrawing : LineElement
    {
        private readonly string _imageId;
        private readonly IImageAccessor _imageAccessor;

        // size defined in document
        public InilineDrawing(string imageId, Size size, IImageAccessor imageAccessor)
        {
            _imageId = imageId;
            _imageAccessor = imageAccessor;
            this.Size = size;
        }

        public override double GetBaseLineOffset()
            => this.Size.Height;

        public override void Justify(DocumentPosition position, double baseLineOffset, Size lineSpace)
        {
            this.SetPosition(position);
        }

        public override void Render(IRendererPage page)
        {
            var stream = _imageAccessor.GetImageStream(_imageId);
            page.RenderImage(stream, this.Position.Offset, this.Size);
        }
    }
}
