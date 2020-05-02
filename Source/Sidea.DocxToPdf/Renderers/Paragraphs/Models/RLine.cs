using System;
using System.Collections.Generic;
using System.Linq;
using PdfSharp.Drawing;
using Sidea.DocxToPdf.Renderers.Common;
using Sidea.DocxToPdf.Renderers.Core;
using Sidea.DocxToPdf.Renderers.Core.RenderingAreas;

namespace Sidea.DocxToPdf.Renderers.Paragraphs.Models
{
    internal class RLine : RendererBase
    {
        private readonly Box<RLineElement>[] _elements;
        private readonly bool _isLastLineOfParagraph;
        private readonly RParagraph _paragraph = new RParagraph();

        public RLine(
            IEnumerable<Box<RLineElement>> elements,
            bool isLastLineOfParagraph)
        {
            _elements = elements.ToArray();
            _isLastLineOfParagraph = isLastLineOfParagraph;
        }

        protected override XSize CalculateContentSizeCore(IPrerenderArea prerenderArea)
        {
            _paragraph.CalculateContentSize(prerenderArea);

            var maxHeight = _elements
                .Max(e => e.Element.PrecalulatedSize.Height);

            var lastElement = _elements.Last();
            var width = lastElement.Offset.X + lastElement.Element.PrecalulatedSize.Width;
            return new XSize(width, Math.Max(maxHeight, prerenderArea.AreaFont.Height));
        }

        protected override RenderResult RenderCore(IRenderArea renderArea)
        {
            if(this.RenderResult.Status == RenderingStatus.ReachedEndOfArea)
            {
                return RenderResult.DoneEmpty;
            }

            var lineArea = renderArea
                .Restrict(renderArea.Width, this.PrecalulatedSize.Height);

            foreach(var box in _elements)
            {
                var elementArea = lineArea.PanLeft(box.Offset.X);
                box.Element.Render(elementArea);
            }

            if (_isLastLineOfParagraph)
            {
                var pan = _elements.Length > 0
                    ? new XUnit(_elements.Last().Offset.X + _elements.Last().Element.PrecalulatedSize.Width)
                    : new XUnit(0);

                _paragraph.Render(lineArea.PanLeft(pan));
            }

            return _elements.LastOrDefault()?.Element is RPageBreak
                ? RenderResult.EndOfRenderArea(renderArea.AreaRectangle)
                : RenderResult.Done(this.PrecalulatedSize);
        }
    }
}
