using System.Collections.Generic;
using System.Linq;
using PdfSharp.Drawing;
using Sidea.DocxToPdf.Renderers.Core;
using Sidea.DocxToPdf.Renderers.Core.RenderingAreas;

namespace Sidea.DocxToPdf.Renderers.Paragraphs.Models
{
    internal class RLine : RendererBase
    {
        private readonly LineAlignment _alignment;
        private readonly bool _isLastLineOfParagraph;
        private readonly RLineElement[] _elements;
        private XUnit[] _offsets;

        public RLine(
            IEnumerable<RLineElement> elements,
            LineAlignment alignment,
            bool isLastLineOfParagraph)
        {
            _elements = elements
                .SkipWhile(e => e.OmitableAtLineBegin)
                .Reverse()
                .SkipWhile(e => e.OmitableAtLineEnd)
                .Reverse()
                .ToArray();

            _offsets = Enumerable
                .Range(0, _elements.Length)
                .Select(_ => XUnit.Zero)
                .ToArray();

            _alignment = alignment;
            _isLastLineOfParagraph = isLastLineOfParagraph;
        }

        protected override XSize CalculateContentSizeCore(IPrerenderArea prerenderArea)
        {
            _offsets = this
                .CalculateOffsets(prerenderArea.Width)
                .ToArray();

            var maxHeight = _elements.Max(e => e.PrecalulatedSize.Height);
            var width = _offsets.Last()
                + _elements.Last().PrecalulatedSize.Width;

            return new XSize(width, maxHeight);
        }

        protected override RenderResult RenderCore(IRenderArea renderArea)
        {
            var lineArea = renderArea
                .Restrict(renderArea.Width, this.PrecalulatedSize.Height);

            for(var i = 0; i < _elements.Length; i++)
            {
                var elementArea = lineArea.PanLeft(_offsets[i]);
                _elements[i].Render(elementArea);
            }

            if(_isLastLineOfParagraph && renderArea.Options.RenderParagraphCharacter)
            {
                var y = this.PrecalulatedSize.Height - renderArea.AreaFont.Height / 4d;
                lineArea
                    .PanLeft(this.PrecalulatedSize.Width)
                    .DrawText("¶", renderArea.AreaFont, XBrushes.Black, new XPoint(0, y));
            }

            return RenderResult.Done(this.PrecalulatedSize);
        }

        private IEnumerable<XUnit> CalculateOffsets(XUnit totalWidth)
        {
            var defaultWidth = _elements
                .Sum(e => e.PrecalulatedSize.Width);

            switch (_alignment)
            {
                case LineAlignment.Left:
                    _offsets = this.CalculateDefaultElementOffsets(XUnit.Zero);
                    break;
                case LineAlignment.Center:
                    {
                        var firstElementOffset = (totalWidth - defaultWidth) / 2;
                        _offsets = this.CalculateDefaultElementOffsets(firstElementOffset);
                    }
                    break;
                case LineAlignment.Right:
                    {
                        var firstElementOffset = (totalWidth - defaultWidth);
                        _offsets = this.CalculateDefaultElementOffsets(firstElementOffset);
                    }
                    break;
                case LineAlignment.Justify:
                    _offsets = this.JustifyWords(defaultWidth, totalWidth);
                    break;
            }

            return _offsets;
        }

        private XUnit[] CalculateDefaultElementOffsets(XUnit firstElementOffset)
        {
            var leftOffset = firstElementOffset;
            return _elements
                .Select(e =>
                {
                    var t = leftOffset;
                    leftOffset += e.PrecalulatedSize.Width;
                    return t;
                })
                .ToArray();
        }

        private XUnit[] JustifyWords(XUnit defaultWidth, XUnit totalWidth)
        {
            if (defaultWidth < totalWidth - XUnit.FromCentimeter(2.5))
            {
                return this.CalculateDefaultElementOffsets(XUnit.Zero);
            }

            var spaceToJustify = totalWidth - defaultWidth;
            var wordsJustifiedSpacing = (totalWidth - defaultWidth) / _elements.Length;

            var x = XUnit.Zero;
            return _elements
                .Select(element =>
                {
                    var xCoordinate = x;
                    x += element.PrecalulatedSize.Width + wordsJustifiedSpacing;
                    return xCoordinate;
                })
                .ToArray();
        }
    }
}
