using System;
using System.Collections.Generic;
using System.Linq;
using DocumentFormat.OpenXml.Wordprocessing;
using PdfSharp.Drawing;
using Sidea.DocxToPdf.Renderers.Core;
using Sidea.DocxToPdf.Renderers.Core.RenderingAreas;
using Sidea.DocxToPdf.Renderers.Paragraphs.Builders;
using Sidea.DocxToPdf.Renderers.Paragraphs.Models;
using Sidea.DocxToPdf.Renderers.Styles;

namespace Sidea.DocxToPdf.Renderers.Paragraphs
{
    internal class ParagraphRenderer : RendererBase
    {
        private readonly Paragraph _paragraph;
        private readonly IStyleAccessor _styleAccessor;
        private List<RFixedDrawing> _fixedDrawings = null;
        private List<RLine> _lines = null;
        private ParagraphStyle _paragraphStyle;

        public ParagraphRenderer(Paragraph paragraph, IStyleAccessor styleAccessor)
        {
            _paragraph = paragraph;
            _styleAccessor = styleAccessor;
        }

        protected override sealed XSize CalculateContentSizeCore(IPrerenderArea prerenderArea)
        {
            _paragraphStyle = _styleAccessor.EffectiveStyle(_paragraph.ParagraphProperties);

            _fixedDrawings = _paragraph
                .CreateFixedDrawings()
                .OrderBy(d => d.Position.Y)
                .ToList();

            _lines = _paragraph
                .CreateRenderingLines(_fixedDrawings, _paragraphStyle.Spacing.Line, prerenderArea)
                .ToList();

            var width = _lines
                .Select(l => (double)l.PrecalulatedSize.Width)
                .Max();

            var height = _lines
                .Select(l => l.PrecalulatedSize.Height + _paragraphStyle.Spacing.Line.CalculateSpaceAfterLine(l))
                .Sum()
                + _paragraphStyle.Spacing.After;

            return new XSize(width, height);
        }

        protected override sealed RenderResult RenderCore(IRenderArea renderArea)
        {
            var renderedSize = new XSize(0, 0);

            while (_fixedDrawings.Count > 0)
            {
                var drawing = _fixedDrawings[0];
                if (drawing.Position.Y + drawing.OuterboxSize.Height < renderArea.Height)
                {
                    drawing.Render(renderArea);
                    _fixedDrawings.RemoveAt(0);
                }
                else
                {
                    break;
                }
            }

            while (_lines.Count > 0)
            {
                if (_lines[0].PrecalulatedSize.Height + renderedSize.Height > renderArea.Height)
                {
                    return RenderResult.EndOfRenderArea(renderedSize);
                }

                var line = _lines[0];
                _lines.RemoveAt(0);

                line.Render(renderArea.PanDown(renderedSize.Height));

                var spaceAfterLine = _paragraphStyle.Spacing.Line.CalculateSpaceAfterLine(line);

                renderedSize = renderedSize
                    .ExpandWidthIfBigger(line.RenderResult.RenderedWidth)
                    .ExpandHeight(line.RenderResult.RenderedHeight);

                renderedSize = renderedSize
                    .ExpandHeight(Math.Min(spaceAfterLine, renderArea.Height - renderedSize.Height));
            }

            renderedSize = renderedSize
                .ExpandHeight(Math.Min(_paragraphStyle.Spacing.After, renderArea.Height - renderedSize.Height));

            return RenderResult.Done(renderedSize);
        }
    }
}
