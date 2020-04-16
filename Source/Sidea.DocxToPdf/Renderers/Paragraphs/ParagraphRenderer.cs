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
        // "¶"
        private readonly Paragraph _paragraph;
        private readonly RenderingOptions _renderingOptions;
        private XUnit _width = XUnit.Zero;

        private List<RLine> _remainingLines = null;

        private Queue<RLinesPage> _linesPages = new Queue<RLinesPage>();

        public XSize TotalArea { get; private set; }

        public ParagraphRenderer(Paragraph paragraph, RenderingOptions renderingOptions)
        {
            _paragraph = paragraph;
            _renderingOptions = renderingOptions;
        }

        public XSize CalculateContentSize(IPrerenderArea renderArea)
        {
            this.PrepareRemainingLines(renderArea);

            if(_remainingLines.Count == 0)
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

        public RenderingState Prepare(IPrerenderArea renderArea)
        {
            if(_remainingLines == null)
            {
                this.PrepareRemainingLines(renderArea);
            }

            var page = this.PrepareLinesPage(renderArea);
            var status = page.IsLastPage
                ? RenderingStatus.Done
                : RenderingStatus.ReachedEndOfArea;

            _linesPages.Enqueue(page);

            return RenderingState.FromStatus(status, 0, page.Height);
        }

        public RenderingState Render(IRenderArea renderArea)
        {
            if(_remainingLines.Count == 0)
            {
                return RenderingState.DoneEmpty;
            }

            var availableArea = renderArea;
            var aggregatedHeight = 0d;

            var lastLineEnd = XUnit.Zero;

            while (_remainingLines.Count > 0)
            {
                var line = _remainingLines[0];
                if (aggregatedHeight + line.Height > renderArea.Height)
                {
                    return RenderingState.EndOfRenderArea(renderArea.Width, aggregatedHeight);
                }

                _remainingLines.RemoveAt(0);
                var lineEnd = line.Render(availableArea.PanLeftDown(new XSize(0, aggregatedHeight)));
                lastLineEnd = lineEnd.X;
                aggregatedHeight += line.Height;
                // availableArea = availableArea.PanLeftDown(new XSize(0, line.Height));
            }

            if (_renderingOptions.RenderParagraphCharacter)
            {
                availableArea.DrawText("¶", renderArea.AreaFont, XBrushes.Black, new XPoint(lastLineEnd, aggregatedHeight));
            }

            return RenderingState.Done(_width, aggregatedHeight);

            //if(_linesPages.Count == 0)
            //{
            //    return RenderingState.DoneEmpty;
            //}

            //var page = _linesPages.Dequeue();
            //var availableArea = renderArea;
            //var endPoint = new XPoint(0, 0);

            //foreach(var line in page.Lines)
            //{
            //    var lineEnd = (XVector)line.Render(availableArea);
            //    availableArea = availableArea.PanLeftDown(new XSize(0, line.Height));
            //    endPoint = new XPoint(0, endPoint.Y + line.Height);
            //}

            //var status = _linesPages.Count == 0
            //    ? RenderingStatus.Done
            //    : RenderingStatus.ReachedEndOfArea;

            //return RenderingState.FromStatus(status, 0, page.Height);
        }

        private void PrepareRemainingLines(IPrerenderArea renderArea)
        {
            _remainingLines = _paragraph
                .ToRenderingLines(renderArea)
                .ToList();
        }

        private RLinesPage PrepareLinesPage(IPrerenderArea renderArea)
        {
            var linesInPage = new List<RLine>();

            var aggregatedHeight = 0d;
            while (_remainingLines.Count > 0)
            {
                var line = _remainingLines.First();
                if (aggregatedHeight + line.Height > renderArea.Height)
                {
                    break;
                }

                aggregatedHeight += line.Height;
                linesInPage.Add(line);
                _remainingLines.Remove(line);
            }

            return new RLinesPage(0, linesInPage, _remainingLines.Count == 0);
        }

        private class RLinesPage
        {
            private RLine[] _lines;

            public RLinesPage(
                int pageNumber,
                IEnumerable<RLine> lines,
                bool isLastPage)
            {
                this.PageNumber = pageNumber;
                _lines = lines.ToArray();

                this.IsLastPage = isLastPage;
                this.Height = _lines.Length == 0
                    ? 0d
                    : _lines.Sum(l => l.Height);
            }

            public int PageNumber { get; }
            public IEnumerable<RLine> Lines => _lines;
            public bool IsLastPage { get; }
            public XUnit Height { get; }
        }
    }
}
