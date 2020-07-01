using System.Collections.Generic;
using System.Linq;
using Sidea.DocxToPdf.Core;
using Sidea.DocxToPdf.Models.Paragraphs.Builders;
using Sidea.DocxToPdf.Models.Styles;
using Word = DocumentFormat.OpenXml.Wordprocessing;

using static Sidea.DocxToPdf.Models.FieldUpdateResult;
using System;
using Sidea.DocxToPdf.Models.Common;

namespace Sidea.DocxToPdf.Models.Paragraphs
{
    internal class Paragraph : ContainerElement
    {
        private readonly IStyleFactory _styleFactory;

        private List<Line> _lines = new List<Line>();
        private FixedDrawing[] _fixedDrawings = new FixedDrawing[0];
        private Stack<LineElement> _unprocessedElements = new Stack<LineElement>();

        private Paragraph(IEnumerable<LineElement> elements, IEnumerable<FixedDrawing> fixedDrawings, IStyleFactory styleFactory)
        {
            _unprocessedElements = elements.ToStack();
            _fixedDrawings = fixedDrawings.ToArray();
            _styleFactory = styleFactory;
        }

        public static Paragraph Create(Word.Paragraph paragraph, IStyleFactory styleFactory)
        {
            var paragraphStyleFactory = styleFactory.ForParagraph(paragraph.ParagraphProperties);
            var fixedDrawings = paragraph
                .CreateFixedDrawingElements()
                .OrderBy(d => d.BoundingBox.Y)
                .ToArray();

            var elements = paragraph
                .CreateParagraphElements(paragraphStyleFactory);

            return new Paragraph(elements, fixedDrawings, paragraphStyleFactory);
        }

        private ParagraphStyle ParagraphStyle => _styleFactory.ParagraphStyle;

        public double SpaceBefore => this.ParagraphStyle.Spacing.Before;

        public double SpaceAfter => this.ParagraphStyle.Spacing.After;

        public override void Prepare(
            PageContext startOn,
            Func<PageNumber, ContainerElement, PageContext> pageFactory)
        {
            ExecuteResult execResult;
            int continueOnLineIndex = 0;

            var context = startOn;
            do
            {
                (execResult, continueOnLineIndex) = this.ExecutePrepare(context, continueOnLineIndex);
                if(execResult == ExecuteResult.RequestNextPage)
                {
                    this.SetPageRegion(new PageRegion(context.PageNumber, context.Region));
                    context = pageFactory(context.PageNumber.Next(), this);
                }
            } while (execResult != ExecuteResult.Done);

            var heightInLastRegion = _lines
                .Where(l => l.Position.PageNumber == context.PageNumber)
                .Sum(l => l.HeightWithSpacing);

            this.SetPageRegion(new PageRegion(context.PageNumber, new Rectangle(context.Region.TopLeft, context.Region.Width, heightInLastRegion)));
        }

        private (ExecuteResult, int) ExecutePrepare(PageContext context, int fromLineIndex)
        {
            var yOffset = 0.0;

            // update lines
            var i = fromLineIndex;
            while (i < _lines.Count)
            {
                var line = _lines[i];

                var updateResult = line.Update(context.PageVariables);
                if(updateResult == ReconstructionNecessary)
                {
                    ClearLinesFromIndex(i);
                    break;
                }

                if(context.Region.Y + yOffset + line.HeightWithSpacing > context.Region.BottomY)
                {
                    return (ExecuteResult.RequestNextPage, i);
                }

                line.SetPosition(context.TopLeft.MoveY(yOffset));
                yOffset += line.HeightWithSpacing;
                i++;
            }

            var defaultLineHeight = _styleFactory.TextStyle.Font.Height;
            var relativeYOffset = _lines.Sum(l => l.HeightWithSpacing);

            while (_unprocessedElements.Count > 0)
            {
                var line = _unprocessedElements.CreateLine(
                    this.ParagraphStyle.LineAlignment,
                    relativeYOffset,
                    _fixedDrawings,
                    context.Region.Width,
                    defaultLineHeight,
                    context.PageVariables,
                    this.ParagraphStyle.Spacing.Line);

                _lines.Add(line);
                if (context.Region.Y + yOffset + line.HeightWithSpacing > context.Region.BottomY)
                {
                    return (ExecuteResult.RequestNextPage, _lines.Count - 1);
                }

                line.SetPosition(context.TopLeft.MoveY(yOffset));
                yOffset += line.HeightWithSpacing;
            }

            return (ExecuteResult.Done, 0);
        }

        public override void Render(IRenderer renderer)
        {
            foreach(var line in _lines)
            {
                var page = renderer.Get(line.Position.PageNumber);
                line.Render(page);
            }

            this.RenderBordersIf(renderer, renderer.Options.ParagraphRegionBoundaries);
        }

        private void ClearLinesFromIndex(int fromIndex)
        {
            var elements = _lines
                .Skip(fromIndex)
                .Reverse()
                .SelectMany(l => l.GetAllElements())
                .ToArray();

            _lines = _lines
                .Take(fromIndex)
                .ToList();

            _unprocessedElements.Push(elements);
        }

        private enum ExecuteResult
        {
            RequestNextPage,
            Done
        }
    }
}
