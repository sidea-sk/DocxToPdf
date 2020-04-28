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

        public RLine(
            IEnumerable<Box<RLineElement>> elements,
            bool isLastLineOfParagraph)
        {
            _elements = elements.ToArray();
            _isLastLineOfParagraph = isLastLineOfParagraph;
        }

        protected override XSize CalculateContentSizeCore(IPrerenderArea prerenderArea)
        {
            var maxHeight = _elements.Max(e => e.Element.PrecalulatedSize.Height);
            var lastElement = _elements.Last();
            var width = lastElement.Offset.X + lastElement.Element.PrecalulatedSize.Width;
            return new XSize(width, maxHeight);
        }

        protected override RenderResult RenderCore(IRenderArea renderArea)
        {
            var lineArea = renderArea
                .Restrict(renderArea.Width, this.PrecalulatedSize.Height);

            foreach(var box in _elements)
            {
                var elementArea = lineArea.PanLeft(box.Offset.X);
                box.Element.Render(elementArea);
            }

            if (_isLastLineOfParagraph && renderArea.Options.RenderParagraphCharacter)
            {
                var y = this.PrecalulatedSize.Height - renderArea.AreaFont.Height / 4d;
                lineArea
                    .PanLeft(this.PrecalulatedSize.Width)
                    .DrawText("¶", renderArea.AreaFont, XBrushes.Black, new XPoint(0, y));
            }

            return RenderResult.Done(this.PrecalulatedSize);
        }
    }
}
