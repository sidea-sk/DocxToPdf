using System.Collections.Generic;
using System.Linq;
using Sidea.DocxToPdf.Models.Common;
using Sidea.DocxToPdf.Models.Styles;

namespace Sidea.DocxToPdf.Models.Paragraphs.Builders
{
    internal static class LineSegmentBuilder
    {
        public static LineSegment CreateLineSegment(
            this Stack<LineElement> fromElements,
            HorizontalSpace space,
            LineAlignment lineAlignment,
            double defaultLineHeight)
        {
            var overflow = lineAlignment == LineAlignment.Justify ? 2 : 0;
            var elements = fromElements
                .GetElementsToFitMaxWidth(space.Width + overflow)
                .ToArray();

            return new LineSegment(elements, lineAlignment, space, defaultLineHeight);
        }

        private static IEnumerable<LineElement> GetElementsToFitMaxWidth(
            this Stack<LineElement> fromElements,
            double maxWidth)
        {
            var aggregatedWidth = 0.0;
            var elements = new List<LineElement>();
            var spaces = new List<SpaceElement>();

            while (fromElements.Count > 0 && aggregatedWidth <= maxWidth)
            {
                var element = fromElements.Pop();

                if(element is SpaceElement space)
                {
                    spaces.Add(space);
                    continue;
                }

                aggregatedWidth += spaces.TotalWidth() + element.Size.Width;
                if (aggregatedWidth < maxWidth)
                {
                    elements.AddRange(spaces);
                    elements.Add(element);
                    spaces.Clear();
                }
                else
                {
                    fromElements.Push(element);
                }

                if (element is NewLineElement)
                {
                    break;
                }
            }

            return elements.Union(spaces);
        }

        private static double TotalWidth(this IEnumerable<LineElement> elements)
        {
            var w = elements.Sum(e => e.Size.Width);
            return w;
        }
    }
}
