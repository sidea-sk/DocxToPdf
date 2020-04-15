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
    internal class ParagraphRenderer : IRenderer
    {
        private readonly Paragraph _paragraph;
        private List<RLine> _lines = null;

        public ParagraphRenderer(Paragraph paragraph)
        {
            _paragraph = paragraph;
        }

        public RenderingState Prepare(IPrerenderArea renderArea)
        {
            throw new System.NotImplementedException();
        }

        public RenderingState Render(IRenderArea renderArea)
        {
            if (_lines == null)
            {
                _lines = this.PrepareLinesQueue(renderArea);
            }

            var availableArea = renderArea;
            var endPoint = new XPoint(0, 0);

            while(_lines.Count > 0)
            {
                var line = _lines[0];
                if (availableArea.Height < line.Height)
                {
                    return new RenderingState(RenderingStatus.ReachedEndOfArea, endPoint);
                }

                _lines.RemoveAt(0);

                var lineEnd = (XVector)line.Render(availableArea);
                availableArea = availableArea.PanLeftDown(new XSize(0, lineEnd.Y));
                endPoint = new XPoint(0, endPoint.Y + lineEnd.Y);
            }

            return new RenderingState(RenderingStatus.Done, endPoint);
        }

        private List<RLine> PrepareLinesQueue(IRenderArea renderArea)
        {
            var lines = _paragraph
                .ToRenderingLines(renderArea)
                .ToArray();

            return new List<RLine>(lines);
        }
    }
}
