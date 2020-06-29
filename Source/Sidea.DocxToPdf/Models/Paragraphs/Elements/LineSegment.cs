using System;
using System.Collections.Generic;
using System.Linq;
using Sidea.DocxToPdf.Core;
using Sidea.DocxToPdf.Models.Common;
using Sidea.DocxToPdf.Models.Styles;

using static Sidea.DocxToPdf.Models.FieldUpdateResult;

namespace Sidea.DocxToPdf.Models.Paragraphs
{
    internal class LineSegment : ElementBase
    {
        private readonly LineElement[] _elements;
        private readonly LineElement[] _trimmedElements;
        private readonly LineAlignment _lineAlignment;
        private readonly HorizontalSpace _space;
        private double _baselineOffset = 0;
        private double _lineHeight = 0;

        public LineSegment(
            IEnumerable<LineElement> elements,
            LineAlignment lineAlignment,
            HorizontalSpace space,
            double defaultLineHeight)
        {
            _elements = elements.ToArray();
            _trimmedElements = _elements
                    .SkipWhile(e => e is SpaceElement) // skip leading spaces
                    .Reverse()
                    .SkipWhile(e => e is SpaceElement) // skip trailing spaces
                    .Reverse()
                    .ToArray();

            _lineAlignment = lineAlignment;
            _space = space;

            var _lineHeight = _trimmedElements.MaxOrDefault(e => e.Size.Height, defaultLineHeight);
            this.Size = new Size(_space.Width, _lineHeight);
        }

        public override void SetPosition(DocumentPosition position)
        {
            base.SetPosition(position + new Point(_space.X, 0));
            this.JustifyElementsToBaseLineAndLineHeight();
        }

        //public FieldUpdateResult Update(DocumentPosition documentPosition, DocumentVariables variables)
        //{
        //    var justifyIsNecessary = false;
        //    var pageVariables = new PageVariables(documentPosition.PageNumber, variables.TotalPages);
        //    foreach (var e in _elements.OfType<Field>())
        //    {
        //        justifyIsNecessary = justifyIsNecessary || e.Update(pageVariables) == BoundingBoxResized;
        //    }

        //    if (!justifyIsNecessary)
        //    {
        //        return NoChange;
        //    }

        //    this.JustifyElementsToBaseLineAndLineHeight();

        //    var result = _space.Width < CalculateTotalWidthOfElements()
        //        ? ReconstructionNecessary
        //        : NoChange;

        //    return result;
        //}

        public void JustifyElements(double baseLineOffset, double lineHeight)
        {
            _baselineOffset = baseLineOffset;
            _lineHeight = lineHeight;

            this.Size = new Size(_space.Width, _lineHeight);
            this.JustifyElementsToBaseLineAndLineHeight();
        }

        public double GetBaseLineOffset()
        {
            if(_elements.Length == 0)
            {
                return 0;
            }

            return _elements.Max(e => e.GetBaseLineOffset());
        }

        public override void Render(IRendererPage page)
        {
            foreach(var e in _trimmedElements)
            {
                e.Render(page);
            }
        }

        //public override void Render()
        //{
        //    _trimmedElements.Render();
        //    _renderer.RenderBorderIf(this.BoundingBox, _renderer.Options.LineRegionBoundaries);
        //}

        public FieldUpdateResult UpdateFields()
        {
            //var justifyIsNecessary = false;
            //foreach (var e in _elements.OfType<Field>())
            //{
            //    justifyIsNecessary = justifyIsNecessary || e.Update() == BoundingBoxResized;
            //}

            //if (!justifyIsNecessary)
            //{
            //    return NoChange;
            //}

            //this.JustifyElementsToBaseLineAndLineHeight();

            //var result = _space.Width < CalculateTotalWidthOfElements()
            //    ? ReconstructionNecessary
            //    : NoChange;

            //return result;

            return NoChange;
        }

        public void ReturnElementsToStack(Stack<LineElement> stack)
        {
            stack.Push(_elements);
        }

        public IEnumerable<LineElement> GetAllElements() => _elements;

        private void JustifyElementsToBaseLineAndLineHeight()
        {
            var totalWidth = _trimmedElements.Sum(e => e.Size.Width);
            switch (_lineAlignment)
            {
                case LineAlignment.Left:
                    this.AlignElements(0, _lineHeight, _baselineOffset);
                    break;
                case LineAlignment.Center:
                    this.AlignElements((_space.Width - totalWidth) / 2, _lineHeight, _baselineOffset);
                    break;
                case LineAlignment.Right:
                    this.AlignElements(_space.Width - totalWidth, _lineHeight, _baselineOffset);
                    break;
                case LineAlignment.Justify:
                    this.JustifyElements(_baselineOffset, _lineHeight, _space.Width - totalWidth);
                    break;
            }
        }

        private void AlignElements(
            double startX,
            double maxHeight,
            double baselineOffset)
        {
            var x = startX;
            foreach (var element in _trimmedElements)
            {
                element.Justify(this.Position + new Point(x, 0), baselineOffset, maxHeight);
                x += element.Size.Width;
            }
        }

        private void JustifyElements(
            double baselineOffset,
            double lineHeight,
            double freeSpaceWidth)
        {
            // TODO: improve justify algorithm from this naive to a better one.
            var sw = CalculateSpaceExpansion(_trimmedElements, freeSpaceWidth);
            var x = 0.0;

            foreach (var element in _trimmedElements)
            {
                var width = element is SpaceElement
                    ? sw + element.Size.Width
                    : element.Size.Width;

                element.Justify(this.Position + new Point(x, 0), baselineOffset, lineHeight);
                x += width;
            }
        }

        private static double CalculateSpaceExpansion(IReadOnlyCollection<LineElement> elements, double freeSpace)
        {
            var spaceElements = elements
                .OfType<SpaceElement>()
                .Count();

            if (spaceElements == 0)
            {
                return 0;
            }

            var sw = elements
                .OfType<SpaceElement>()
                .First()
                .Size.Width;

            var result = Math.Min(sw, freeSpace / spaceElements);
            return result;
        }
    }
}
