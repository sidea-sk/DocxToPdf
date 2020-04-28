using System;
using System.Collections.Generic;
using System.Linq;
using PdfSharp.Drawing;
using Sidea.DocxToPdf.Renderers.Common;
using Sidea.DocxToPdf.Renderers.Core.RenderingAreas;
using Sidea.DocxToPdf.Renderers.Paragraphs.Models;

namespace Sidea.DocxToPdf.Renderers.Paragraphs.Builders
{
    internal static class LineBuilder
    {
        public static RLine CreateLine(
            Stack<RLineElement> fromElements,
            LineAlignment lineAlignment,
            XUnit verticalOffset,
            IEnumerable<RFixedDrawing> fixedDrawings,
            IPrerenderArea prerenderArea)
        {
            var lineSegments = prerenderArea
                .GetAvailableLineToSegments(fixedDrawings, verticalOffset);

            var boxes = lineSegments
                .SelectMany(l => l.GetAlignedElementsForLineSegment(fromElements, lineAlignment, lineSegments.Length == 1, prerenderArea))
                .ToList();

            if(boxes.Count == 0)
            {
                var emptyText = prerenderArea.CreateEmptyText();
                boxes.Add(new Box<RLineElement>(emptyText, new XPoint(lineSegments.First().LeftOffset, 0)));
            }

            return new RLine(boxes, fromElements.Count == 0);
        }

        private static IEnumerable<Box<RLineElement>> GetAlignedElementsForLineSegment(
            this LineSegment lineSegment,
            Stack<RLineElement> fromElements,
            LineAlignment lineAlignment,
            bool allowWordSplit,
            IPrerenderArea prerenderArea)
        {
            var segmentElements = lineSegment
                .GetElementsForLineSegment(fromElements, allowWordSplit, prerenderArea);

            var boxes = segmentElements
                .CreateBoxes(lineAlignment, lineSegment.LeftOffset, lineSegment.Width)
                .ToArray();

            return boxes;
        }

        private static IEnumerable<RLineElement> GetElementsForLineSegment(
            this LineSegment lineSegment,
            Stack<RLineElement> fromElements,
            bool allowWordSplit,
            IPrerenderArea prerenderArea)
        {
            var segmentElements = new List<RLineElement>();
            var left = new XUnit(0);

            var continueSearch = true;
            while(continueSearch && fromElements.Count > 0)
            {
                var element = fromElements.Pop();
                element.CalculateContentSize(prerenderArea);

                if(left + element.PrecalulatedSize.Width <= lineSegment.Width)
                { 
                    segmentElements.Add(element);
                    left += element.PrecalulatedSize.Width;
                }
                else if(allowWordSplit && segmentElements.Count == 0 && element is RText text)
                {
                    var (cut, tail) = text.CutTextOfMaxWidth(lineSegment.Width - left, prerenderArea);
                    segmentElements.Add(cut);
                    fromElements.Push(tail);
                    continueSearch = false;
                }
                else
                {
                    fromElements.Push(element);
                    continueSearch = false;
                }
            }

            return segmentElements
                .TrimSpaces();
        }

        private static LineSegment[] GetAvailableLineToSegments(
            this IPrerenderArea prerenderArea,
            IEnumerable<RFixedDrawing> fixedDrawings,
            XUnit lineVerticalOffset)
        {
            var reservedSpace = fixedDrawings.GetReservedSpaceInLine(lineVerticalOffset, prerenderArea.AreaFont.GetHeight());
            var lineSegments = new List<LineSegment>();
            var offset = XUnit.FromPoint(0);
            foreach(var rs in reservedSpace)
            {
                if(rs.LeftOffset > offset)
                {
                    lineSegments.Add(new LineSegment(offset, rs.LeftOffset));
                }

                offset = rs.LeftOffset + rs.Width;

                if(offset > prerenderArea.Width)
                {
                    break;
                }
            }

            if(offset < prerenderArea.Width)
            {
                lineSegments.Add(new LineSegment(offset, prerenderArea.Width - offset));
            }

            return lineSegments.ToArray();
        }

        private static LineSegment[] GetReservedSpaceInLine(
            this IEnumerable<RFixedDrawing> fixedDrawings,
            XUnit lineVerticalOffset,
            XUnit lineMinimalHeight)
        {
            var reservedSpace = fixedDrawings
                .Where(d => d.Position.Y <= lineVerticalOffset + lineMinimalHeight && d.Position.Y + d.OuterboxSize.Height >= lineVerticalOffset)
                .Select(d => new LineSegment(d.Position.X, d.OuterboxSize.Width))
                .OrderBy(r => r.LeftOffset)
                .ToArray();

            return reservedSpace;
        }

        private static IEnumerable<RLineElement> TrimSpaces(this IEnumerable<RLineElement> elements)
        {
            var result = elements
                .SkipWhile(e => e.OmitableAtLineBegin)
                .Reverse()
                .SkipWhile(e => e.OmitableAtLineEnd)
                .Reverse()
                .ToArray();

            return result;
        }

        private static (RText cut, RText tail) CutTextOfMaxWidth(this RText text, XUnit maxWidth, IPrerenderArea prerenderArea)
        {
            RText previous = text.Substring(0, 0);
            previous.CalculateContentSize(prerenderArea);

            RText current;
            for (var i = 1; i < text.TextLength; i++)
            {
                current = text.Substring(0, i);
                current.CalculateContentSize(prerenderArea);

                if (current.PrecalulatedSize.Width > maxWidth)
                {
                    return (previous, text.Substring(i, text.TextLength - i));
                }

                previous = current;
            }

            var empty = prerenderArea.CreateEmptyText();
            return (empty, text);
        }

        private static IEnumerable<Box<RLineElement>> CreateBoxes(
            this IEnumerable<RLineElement> elements,
            LineAlignment lineAlignment,
            XUnit defaultOffset,
            XUnit totalWidth)
        {
            var offsets = elements
                .Select(e => (XUnit)e.PrecalulatedSize.Width)
                .ToArray()
                .CalculateAlignedOffsets(lineAlignment, defaultOffset, totalWidth)
                .ToArray();

            var boxes = elements
                .Select((e, i) =>
                {
                    var offset = offsets[i];
                    return new Box<RLineElement>(e, new XPoint(offset, 0));
                })
                .ToArray();

            return boxes;
        }

        private static IEnumerable<XUnit> CalculateAlignedOffsets(
            this IReadOnlyCollection<XUnit> widths,
            LineAlignment lineAlignment,
            XUnit defaultOffset,
            XUnit toTotalWidth)
        {
            var sumWidth = widths.Sum();

            switch (lineAlignment)
            {
                case LineAlignment.Left:
                    return widths.CalculateDefaultElementOffsets(defaultOffset);
                case LineAlignment.Center:
                    return widths.CalculateDefaultElementOffsets(defaultOffset + (toTotalWidth - sumWidth) / 2);
                case LineAlignment.Right:
                    return widths.CalculateDefaultElementOffsets(defaultOffset + (toTotalWidth - sumWidth));
                case LineAlignment.Justify:
                    return widths.JustifyOffsets(sumWidth, defaultOffset, toTotalWidth);
                default:
                    throw new Exception("Unexpected line alignment value");
            }
        }

        private static IEnumerable<XUnit> JustifyOffsets(
            this IReadOnlyCollection<XUnit> widths,
            XUnit sumWidth,
            XUnit defaultOffset,
            XUnit toTotalWidth)
        {
            if (sumWidth < toTotalWidth - XUnit.FromCentimeter(2.5))
            {
                return widths.CalculateDefaultElementOffsets(defaultOffset);
            }

            var spaceToJustify = toTotalWidth - sumWidth;
            var wordsJustifiedSpacing = spaceToJustify / widths.Count;

            var x = defaultOffset;
            var offsets = widths
                .Select(w =>
                {
                    var xCoordinate = x;
                    x += w + wordsJustifiedSpacing;
                    return xCoordinate;
                })
                .ToArray();

            return offsets;
        }

        private static IEnumerable<XUnit> CalculateDefaultElementOffsets(this IEnumerable<XUnit> widths, XUnit firstElementOffset)
        {
            var leftOffset = firstElementOffset;
            return widths
                .Select(w =>
                {
                    var t = leftOffset;
                    leftOffset += w;
                    return t;
                })
                .ToArray();
        }

        private static RText CreateEmptyText(this IPrerenderArea prerenderArea)
        {
            var empty = RText.Empty(prerenderArea.AreaFont);
            empty.CalculateContentSize(prerenderArea);
            return empty;
        }
    }

    internal struct LineSegment
    {
        public XUnit LeftOffset { get; }
        public XUnit Width { get; }

        public LineSegment(XUnit leftOffset, XUnit width)
        {
            this.LeftOffset = leftOffset;
            this.Width = width;
        }
    }
}
