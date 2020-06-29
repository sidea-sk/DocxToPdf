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
        private readonly Word.Paragraph _paragraph;
        private readonly IStyleFactory _styleFactory;

        private List<Line> _lines = new List<Line>();
        private FixedDrawing[] _fixedDrawings = new FixedDrawing[0];
        private Stack<LineElement> _paragraphElements = new Stack<LineElement>();

        public Paragraph(Word.Paragraph paragraph, IStyleFactory styleFactory)
        {
            _paragraph = paragraph;
            _styleFactory = styleFactory.ForParagraph(paragraph.ParagraphProperties);
        }

        private ParagraphStyle ParagraphStyle => _styleFactory.ParagraphStyle;

        public double SpaceBefore => this.ParagraphStyle.Spacing.Before;

        public double SpaceAfter => this.ParagraphStyle.Spacing.After;

        public override sealed void Initialize()
        {
            _fixedDrawings = _paragraph
                .CreateFixedDrawingElements()
                .OrderBy(d => d.BoundingBox.Y)
                .ToArray();

            _paragraphElements = _paragraph
                .CreateParagraphElements(_styleFactory)
                .ToStack();
        }

        public override void Prepare(
            PageContext startOn,
            Func<PageNumber, PageContext> pageFactory)
        {
            this.ReconstructLines(startOn.Region.Width, startOn.PageVariables);

            var pageNumber = startOn.PageNumber;
            var availableRegion = startOn.Region;
            var paragraphPosition = new DocumentPosition(pageNumber, availableRegion.TopLeft);
            var maxY = availableRegion.BottomY;
            var yOffset = 0.0;

            foreach(var line in _lines)
            {
                var lineHeightAndSpace = line.Size.Height + this.ParagraphStyle.Spacing.Line.CalculateSpaceAfterLine(line.Size.Height);

                if (availableRegion.Y + yOffset + lineHeightAndSpace > maxY)
                {
                    this.SetPageRegion(new PageRegion(pageNumber, availableRegion));

                    pageNumber = pageNumber.Next();
                    var nextPage = pageFactory(pageNumber);

                    availableRegion = nextPage.Region;
                    paragraphPosition = new DocumentPosition(pageNumber, availableRegion.TopLeft);
                    maxY = availableRegion.BottomY;
                    yOffset = 0.0;
                }

                line.SetPosition(paragraphPosition.MoveY(yOffset));
                yOffset += lineHeightAndSpace;
            }

            this.SetPageRegion(new PageRegion(pageNumber, new Rectangle(availableRegion.TopLeft, availableRegion.Width, yOffset)));
        }

        public void Update(
            PageContext startOn,
            Func<PageNumber, PageContext> pageFactory)
        {
            foreach(var line in _lines)
            {

            }
        }

        private void ReconstructLines(double maxWidth, PageVariables variables)
        {
            if(_paragraphElements.Count == 0)
            {
                return;
            }

            var defaultLineHeight = _styleFactory.TextStyle.Font.Height;
            var relativeYOffset = 0.0;

            while (_paragraphElements.Count > 0)
            {
                var line = _paragraphElements.CreateLine(this.ParagraphStyle.LineAlignment, relativeYOffset, _fixedDrawings, maxWidth, defaultLineHeight, variables);
                _lines.Add(line);
            }
        }

        public override void Render(IRenderer renderer)
        {
            foreach(var line in _lines)
            {
                var page = renderer.Get(line.Position.PageNumber);
                line.Render(page);
            }
        }

        // nochange, resized
        //public void Update(PageRegion startRegion, DocumentVariables variables)
        //{
        //    _pageRegions.Clear();

        //    var pageVariables = new PageVariables(startRegion.PageNumber, variables.TotalPages);
        //    var index = 0;

        //    do
        //    {
        //        var line = _lines[index];
        //        var updateResult = line.Update(startRegion.DocumentPosition, pageVariables);

        //        if (updateResult == ReconstructionNecessary)
        //        {
        //            var elements = _lines
        //                .Skip(index)
        //                .SelectMany(l => l.GetAllElements())
        //                .ToStack();

        //            var relY = line.BoundingBox.TopLeft.Y;

        //            _lines = _lines
        //                .Take(index)
        //                .ToList();

        //            var updatedLines = this.CreateLinesFromElements(elements, relY);
        //            _lines.AddRange(updatedLines);
        //        }
        //        else
        //        {
        //            index++;
        //        }
        //    } while (index < _lines.Count);
        //}

        //private void ReconstructLines(PageRegion startRegion, PageVariables pageVariables)
        //{
        //    var elements = _paragraph
        //           .CreateParagraphElements(pageVariables, _styleFactory)
        //           .ToStack();

        //    var defaultLineHeight = _styleFactory.TextStyle.Font.Height;
        //    var relativeYOffset = 0.0;

        //    while (elements.Count > 0)
        //    {
        //        var line = elements.CreateLine(relativeYOffset, this.ParagraphStyle.LineAlignment, _maxAvailableRegion.Width, _fixedDrawings.Select(f => f.BoundingBox), defaultLineHeight);
        //        line.SetOffset(new Point(0, relativeYOffset));
        //        _lines.Add(line);
        //        relativeYOffset += line.BoundingBox.Height + this.ParagraphStyle.Spacing.Line.CalculateSpaceAfterLine(line.BoundingBox.Height);
        //    }
        //}

        //private IEnumerable<Line> CreateLinesFromElements(
        //    Stack<ParagraphElement> elements,
        //    double lineRelativeOffset)
        //{
        //}
    }
}
