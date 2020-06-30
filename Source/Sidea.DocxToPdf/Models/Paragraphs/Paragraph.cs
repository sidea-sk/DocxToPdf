﻿using System.Collections.Generic;
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
        private Stack<LineElement> _unprocessedElements = new Stack<LineElement>();

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

            _unprocessedElements = _paragraph
                .CreateParagraphElements(_styleFactory)
                .ToStack();
        }

        public override void Prepare(
            PageContext startOn,
            Func<PageNumber, PageContext> pageFactory)
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
                    context = pageFactory(context.PageNumber.Next());
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
                    ClearLines(i);
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
        }

        private void ClearLines(int fromIndex)
        {
            var elements = _lines.Skip(fromIndex).Reverse().SelectMany(l => l.GetAllElements());
            _unprocessedElements.Push(elements);
        }

        private enum ExecuteResult
        {
            RequestNextPage,
            Done
        }
    }
}