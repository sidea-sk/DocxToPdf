using System.Collections.Generic;
using System.Linq;
using PdfSharp.Drawing;
using Sidea.DocxToPdf.Renderers.Core;

namespace Sidea.DocxToPdf.Renderers.Paragraphs.Models
{
    internal class RLine
    {
        private readonly RWord[] _words;
        private readonly LineAlignment _alignment;
        private readonly XSize _size;

        public RLine(
            IEnumerable<RWord> words,
            LineAlignment alignment,
            double defaultHeight)
        {
            _words = words.ToArray();
            _alignment = alignment;

            _size = _words.CalculateBoundingAreaSize();
            if(_size.Height == 0)
            {
                _size = new XSize(0, defaultHeight);
            }
        }

        public double Width => _size.Width;
        public double Height => _size.Height;

        public XPoint Render(IRenderArea toArea)
        {
            var wordsArea = toArea;
            var position = new XPoint(0, this.Height);
            foreach (var word in _words)
            {
                word.Render(wordsArea, position);
                wordsArea = wordsArea.PanLeft(word.Width);
            }

            return new XPoint(_size.Width, _size.Height);
        }
    }
}
