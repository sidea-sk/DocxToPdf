using System;
using PdfSharp.Drawing;

namespace Sidea.DocxToPdf.Renderers.Paragraphs.Models
{
    internal class RText : RLineElement
    {
        private readonly string _content;
        private readonly XFont _font;
        private readonly XBrush _brush;

        public RText(string content, XFont font, XBrush brush)
        {
            _content = content;
            _font = font;
            _brush = brush;
        }

        public override bool OmitableAtLineBegin => throw new NotImplementedException();

        public override bool OmitableAtLineEnd => throw new NotImplementedException();
    }
}
