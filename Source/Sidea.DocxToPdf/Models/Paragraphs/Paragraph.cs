using System.Collections.Generic;
using System.Linq;
using Sidea.DocxToPdf.Core;
using Sidea.DocxToPdf.Models.Paragraphs.Builders;
using Sidea.DocxToPdf.Models.Styles;
using Word = DocumentFormat.OpenXml.Wordprocessing;

using static Sidea.DocxToPdf.Models.FieldUpdateResult;

namespace Sidea.DocxToPdf.Models.Paragraphs
{
    internal class Paragraph : ContainerElement
    {
        private readonly Word.Paragraph _paragraph;
        private readonly IStyleFactory _styleFactory;

        private List<Line> _lines = new List<Line>();
        private FixedDrawing[] _fixedDrawings = new FixedDrawing[0];
        private List<ParagraphElement> _paragraphElements = new List<ParagraphElement>();

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
                .ToList();
        }

        public override void Prepare(
            IPage page,
            Rectangle region,
            IPageManager pageManager)
        {

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
