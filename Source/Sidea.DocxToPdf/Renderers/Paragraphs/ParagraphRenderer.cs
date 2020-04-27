using System.Collections.Generic;
using System.Linq;
using DocumentFormat.OpenXml.Wordprocessing;
using PdfSharp.Drawing;
using Sidea.DocxToPdf.Renderers.Core;
using Sidea.DocxToPdf.Renderers.Core.RenderingAreas;
using Sidea.DocxToPdf.Renderers.Paragraphs.Builders;
using Sidea.DocxToPdf.Renderers.Paragraphs.Models;

namespace Sidea.DocxToPdf.Renderers.Paragraphs
{
    internal class ParagraphRenderer : RendererBase
    {
        private readonly Paragraph _paragraph;
        private List<RLine> _lines = null;

        public ParagraphRenderer(Paragraph paragraph)
        {
            _paragraph = paragraph;
        }

        protected override sealed XSize CalculateContentSizeCore(IPrerenderArea prerenderArea)
        {
            _lines = _paragraph
                .CreateRenderingLines(prerenderArea)
                .ToList();

            var width = _lines
                .Select(l => (double)l.PrecalulatedSize.Width)
                .Max();

            var height = _lines
                .Select(l => l.PrecalulatedSize.Height)
                .Sum();

            return new XSize(width, height);
        }

        protected override sealed RenderResult RenderCore(IRenderArea renderArea)
        {
            var renderedSize = new XSize(0, 0);

            while (_lines.Count > 0)
            {
                if(_lines[0].PrecalulatedSize.Height + renderedSize.Height > renderArea.Height)
                {
                    return RenderResult.EndOfRenderArea(renderedSize);
                }

                var line = _lines[0];
                _lines.RemoveAt(0);

                line.Render(renderArea.PanDown(renderedSize.Height));

                renderedSize = renderedSize
                    .ExpandWidthIfBigger(line.RenderResult.RenderedWidth)
                    .ExpandHeight(line.RenderResult.RenderedHeight);
            }

            return RenderResult.Done(renderedSize);
        }
    }
}
