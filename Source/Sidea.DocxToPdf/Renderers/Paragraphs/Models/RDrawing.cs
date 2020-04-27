using PdfSharp.Drawing;
using Sidea.DocxToPdf.Renderers.Core;
using Sidea.DocxToPdf.Renderers.Core.RenderingAreas;

namespace Sidea.DocxToPdf.Renderers.Paragraphs.Models
{
    internal class RDrawing : RLineElement
    {
        private readonly string _id;
        private readonly string _name;
        private readonly string _uri;

        // size defined in document
        private readonly XSize _size;

        public RDrawing(
            string id,
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

        protected override XSize CalculateContentSizeCore(IPrerenderArea prerenderArea)
        {
            return _size;
        }

        protected override RenderResult RenderCore(IRenderArea renderArea)
        {
            renderArea.DrawImage(_id, _size);
            return RenderResult.Done(_size.Width, _size.Height);
        }
    }
}
