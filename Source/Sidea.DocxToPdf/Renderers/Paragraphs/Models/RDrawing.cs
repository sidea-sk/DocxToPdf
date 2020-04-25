using PdfSharp.Drawing;

namespace Sidea.DocxToPdf.Renderers.Paragraphs.Models
{
    internal class RDrawing : RLineElement
    {
        private readonly uint _id;
        private readonly string _name;
        private readonly string _uri;

        // size defined in document
        private readonly XSize _size;

        public RDrawing(
            uint id,
            string name,
            string uri,
            XSize size)
        {
            _id = id;
            _name = name;
            _uri = uri;
            _size = size;
        }

        public override bool OmitableAtLineBegin => false;

        public override bool OmitableAtLineEnd => false;
    }
}
