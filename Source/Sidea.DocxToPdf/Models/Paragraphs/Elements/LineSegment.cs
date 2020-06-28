using System;
using System.Collections.Generic;
using System.Linq;
using Sidea.DocxToPdf.Core;
using Sidea.DocxToPdf.Models.Common;
using Sidea.DocxToPdf.Models.Styles;

using static Sidea.DocxToPdf.Models.FieldUpdateResult;

namespace Sidea.DocxToPdf.Models.Paragraphs
{
    internal class LineSegment
    {
        private readonly ParagraphElement[] _elements;
        private readonly ParagraphElement[] _trimmedElements;
        private readonly LineAlignment _lineAlignment;
        private readonly HorizontalSpace _space;
        private double _baselineOffset = 0;
        private double _lineHeight = 0;

        public LineSegment(
            IEnumerable<ParagraphElement> elements,
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

            var _lineHeight = _trimmedElements.MaxOrDefault(e => e.BoundingBox.Height, defaultLineHeight);
            this.BoundingBox = new Rectangle(0, 0, _space.Width, _lineHeight);
        }

        public Rectangle BoundingBox { get; set; } = Rectangle.Empty;

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

            this.BoundingBox = new Rectangle(0, 0, _space.Width, _lineHeight);
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

        public void ReturnElementsToStack(Stack<ParagraphElement> stack)
        {
            stack.Push(_elements);
        }

        public IEnumerable<ParagraphElement> GetAllElements() => _elements;

        //protected override PreparationState PrepareCore(IPageRegion renderer)
        //{
        //    _renderer = renderer
        //        .ClipFromLeftWidth(_space.X, _space.Width);

        //    foreach(var element in _trimmedElements)
        //    {
        //        element.Prepare(_renderer);
        //    }

        //    return PreparationState.Prepared;
        //}

        private void JustifyElementsToBaseLineAndLineHeight()
        {
            var totalWidth = _trimmedElements.Sum(e => e.BoundingBox.Width);
            switch (_lineAlignment)
            {
                case LineAlignment.Left:
                    AlignElements(_trimmedElements, 0, _lineHeight, _baselineOffset);
                    break;
                case LineAlignment.Center:
                    AlignElements(_trimmedElements, (_space.Width - totalWidth) / 2, _lineHeight, _baselineOffset);
                    break;
                case LineAlignment.Right:
                    AlignElements(_trimmedElements, _space.Width - totalWidth, _lineHeight, _baselineOffset);
                    break;
                case LineAlignment.Justify:
                    JustifyElements(_trimmedElements, _lineHeight, _baselineOffset, _space.Width - totalWidth);
                    break;
            }

            // this.UpdateBoundingBox();
        }

        private double CalculateTotalWidthOfElements()
        {
            return _trimmedElements.Sum(e => e.BoundingBox.Width);
        }

        private static void AlignElements(
            IEnumerable<ParagraphElement> elements,
            double startX,
            double maxHeight,
            double baselineOffset)
        {
            var x = startX;
            foreach (var element in elements)
            {
                var boundingBox = new Rectangle(x, 0, element.BoundingBox.Width, maxHeight);
                element.SetLineBoundingBox(boundingBox, baselineOffset);
                x += element.BoundingBox.Width;
            }
        }

        private static void JustifyElements(
            IReadOnlyCollection<ParagraphElement> elements,
            double lineHeight,
            double baselineOffset,
            double freeSpaceWidth)
        {
            // TODO: improve justify algorithm from this naive to a better one.
            var sw = CalculateSpaceExpansion(elements, freeSpaceWidth);
            var x = 0.0;

            foreach (var element in elements)
            {
                var width = element is SpaceElement
                    ? sw + element.BoundingBox.Width
                    : element.BoundingBox.Width;

                var boundingBox = new Rectangle(x, 0, width, lineHeight);
                element.SetLineBoundingBox(boundingBox, baselineOffset);
                x += width;
            }
        }

        private static double CalculateSpaceExpansion(IReadOnlyCollection<ParagraphElement> elements, double freeSpace)
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
                .BoundingBox.Width;

            var result = Math.Min(sw, freeSpace / spaceElements);
            return result;
        }
    }
}
