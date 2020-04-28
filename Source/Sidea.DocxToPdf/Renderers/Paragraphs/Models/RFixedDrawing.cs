using PdfSharp.Drawing;
using Sidea.DocxToPdf.Renderers.Core;
using Sidea.DocxToPdf.Renderers.Core.RenderingAreas;

namespace Sidea.DocxToPdf.Renderers.Paragraphs.Models
{
    internal class RFixedDrawing
    {
        private readonly string _id;
        private readonly XSize _size;
        private readonly XUnit _horizontalMargin;

        public RFixedDrawing(
            string id,
            XPoint position,
            XSize size)
        {
            _id = id;
            _size = size;
            
            _horizontalMargin = XUnit.FromMillimeter(2);

            this.Position = new XPoint(position.X - _horizontalMargin, position.Y);
            this.OuterboxSize = _size.ExpandWidth(2 * _horizontalMargin);
        }

        public XPoint Position { get; }
        public XSize OuterboxSize { get; }

        public void Render(IRenderArea renderArea)
        {
            renderArea.DrawImage(_id, this.Position + new XVector(_horizontalMargin, 0), _size);
        }
    }
}
