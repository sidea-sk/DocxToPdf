using System.Collections.Generic;
using System.Linq;
using PdfSharp.Drawing;
using Sidea.DocxToPdf.Renderers.Core.RenderingAreas;

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
            _words = words
                .SkipWhile(w => w.IsSpace)
                .Reverse()
                .SkipWhile(w => w.IsSpace)
                .Reverse()
                .ToArray();

            _alignment = alignment;

            _size = _words.CalculateBoundingAreaSize();
            if(_size.Height == 0)
            {
                _size = new XSize(0, defaultHeight);
            }
        }

        public XUnit Width => _size.Width;
        public XUnit Height => _size.Height;

        public XPoint Render(IRenderArea toArea)
        {
            var wordsArea = toArea;
            var position = new XPoint(0, this.Height);
            foreach (var word in this.AlignWords(toArea.Width))
            {
                word.Render(wordsArea, position);
            }

            return new XPoint(_size.Width, _size.Height);
        }

        private IEnumerable<OffsetWord> AlignWords(double totalWidth)
        {
            switch (_alignment)
            {
                case LineAlignment.Left:
                    return this.OffsetAlignWords(new XPoint(0, this.Height));
                case LineAlignment.Center:
                    {
                        var offset = (totalWidth - this.Width) / 2;
                        return this.OffsetAlignWords(new XPoint(offset, this.Height));
                    }
                case LineAlignment.Right:
                    {
                        var offset = totalWidth - this.Width;
                        return this.OffsetAlignWords(new XPoint(offset, this.Height));
                    }
                case LineAlignment.Justify:
                    return this.JustifyWords(new XPoint(0, this.Height), totalWidth);
                default:
                    throw new System.Exception("Unhandled alignment value");
            }
        }

        private IEnumerable<OffsetWord> OffsetAlignWords(XPoint lineBottomLeft)
        {
            var x = lineBottomLeft.X;
            var y = 0;

            return _words
                .Select(word =>
                {
                    var xCoordinate = x;
                    x += word.Width;
                    return new OffsetWord(word, new XVector(xCoordinate, y));
                })
                .ToArray();
        }

        private IEnumerable<OffsetWord> JustifyWords(XPoint lineBottomLeft, double totalWidth)
        {
            if (this.Width < totalWidth - XUnit.FromCentimeter(2.5))
            {
                return this.OffsetAlignWords(lineBottomLeft);
            }

            var spaceToJustify = totalWidth - this.Width;
            var wordsJustifiedSpacing = (totalWidth - this.Width) / _words.Length;

            var x = lineBottomLeft.X;

            return _words
                .Select(word =>
                {
                    var xCoordinate = x;
                    x += word.Width + wordsJustifiedSpacing;
                    return new OffsetWord(word, new XVector(xCoordinate, 0));
                })
                .ToArray();
        }

        private class OffsetWord
        {
            private readonly RWord _word;
            private readonly XVector _offset;

            public OffsetWord(RWord word, XVector offset)
            {
                _word = word;
                _offset = offset;
            }

            public void Render(IRenderArea renderArea, XPoint position) => _word.Render(renderArea, _offset + position);
        }

    }
}
