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
        private readonly RenderingOptions _renderingOptions;
        private XUnit _width = XUnit.Zero;

        private List<RLine> _remainingLines = null;

        public ParagraphRenderer(Paragraph paragraph, RenderingOptions renderingOptions)
        {
            _paragraph = paragraph;
            _renderingOptions = renderingOptions;
        }

        protected override sealed RenderResult RenderCore(IRenderArea renderArea)
        {
            if(_remainingLines.Count == 0)
            {
                return RenderResult.DoneEmpty;
            }

            var availableArea = renderArea;
            var aggregatedHeight = 0d;

            var lastLineEnd = XUnit.Zero;

            while (_remainingLines.Count > 0)
            {
                var line = _remainingLines[0];
                if (aggregatedHeight + line.Height > renderArea.Height)
                {
                    return RenderResult.EndOfRenderArea(renderArea.Width, aggregatedHeight);
                }

                _remainingLines.RemoveAt(0);
                var lineEnd = line.Render(availableArea.PanLeftDown(new XSize(0, aggregatedHeight)));
                lastLineEnd = lineEnd.X;
                aggregatedHeight += line.Height;
            }

            if (_renderingOptions.RenderParagraphCharacter && _remainingLines.Count == 0)
            {
                // TODO: use some property of Font to position paragraph corectly
                var y = aggregatedHeight - renderArea.AreaFont.Height / 4d;
                availableArea.DrawText("¶", renderArea.AreaFont, XBrushes.Black, new XPoint(lastLineEnd, y));
            }

            return RenderResult.Done(_width, aggregatedHeight);
        }

        protected override sealed XSize CalculateContentSizeCore(IPrerenderArea renderArea)
        {
            this.PrepareRemainingLines(renderArea);

            if (_remainingLines.Count == 0)
            {
                return new XSize(0, 0);
            }

            _width = _remainingLines
                .Select(l => (double)l.Width)
                .Max();

            var height = _remainingLines
                .Select(l => l.Height)
                .Sum();

            var width = _remainingLines
                .Select(l => (double)l.Width)
                .Max();

            return new XSize(width, height);
        }

        private void PrepareRemainingLines(IPrerenderArea renderArea)
        {
            _remainingLines = _paragraph
                .ToRenderingLines(renderArea)
                .ToList();
        }
    }
}
