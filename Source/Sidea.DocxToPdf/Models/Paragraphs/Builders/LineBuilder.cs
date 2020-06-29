using System;
using System.Collections.Generic;
using System.Linq;
using Sidea.DocxToPdf.Core;
using Sidea.DocxToPdf.Models.Styles;

namespace Sidea.DocxToPdf.Models.Paragraphs.Builders
{
    internal static class LineBuilder
    {
        public static Line CreateLine(
            this Stack<LineElement> fromElements,
            LineAlignment lineAlignment,
            double relativeYOffset,
            IEnumerable<FixedDrawing> fixedDrawings,
            double availableWidth,
            double defaultLineHeight)
        {
            var (lineSegments, lineHeight) = fromElements
                .CreateLineSegments(lineAlignment, relativeYOffset, fixedDrawings.Select(d => d.BoundingBox), availableWidth, defaultLineHeight);

            var baseLineOffset = lineSegments.Max(ls => ls.GetBaseLineOffset());
            foreach (var ls in lineSegments)
            {
                ls.JustifyElements(baseLineOffset, lineHeight);
            }

            return new Line(lineSegments);
        }

        private static (LineSegment[], double) CreateLineSegments(
            this Stack<LineElement> fromElements,
            LineAlignment lineAlignment,
            double relativeYOffset,
            IEnumerable<Rectangle> fixedDrawings,
            double availableWidth,
            double defaultLineHeight)
        {
            var reserveSpaceHelper = new LineReservedSpaceHelper(fixedDrawings, relativeYOffset, availableWidth);

            var expectedLineHeight = 0.0;
            var finished = false;

            do
            {
                var segmentSpaces = reserveSpaceHelper
                    .GetLineSegments()
                    .ToArray();

                var lineSegments = segmentSpaces
                    .Select((space, i) => fromElements.CreateLineSegment(space, lineAlignment, defaultLineHeight))
                    .ToArray();

                var maxHeight = lineSegments.Max(l => l.Size.Height);
                expectedLineHeight = Math.Max(maxHeight, expectedLineHeight);
                var hasChanged = reserveSpaceHelper.UpdateLineHeight(expectedLineHeight);

                if (!hasChanged)
                {
                    return (lineSegments, expectedLineHeight);
                }
                else
                {
                    foreach (var ls in lineSegments.Reverse())
                    {
                        ls.ReturnElementsToStack(fromElements);
                    }
                }
            } while (!finished);

            return (new LineSegment[0], expectedLineHeight);
        }
    }
}
