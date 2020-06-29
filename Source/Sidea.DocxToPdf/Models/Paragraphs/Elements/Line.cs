using System.Collections.Generic;
using System.Linq;
using Sidea.DocxToPdf.Core;
using Sidea.DocxToPdf.Models.Common;
using static Sidea.DocxToPdf.Models.FieldUpdateResult;

namespace Sidea.DocxToPdf.Models.Paragraphs
{
    internal class Line: ElementBase // : SinglePageElementBase, IFieldsElement
    {
        private readonly LineSegment[] _segments;

        public IEnumerable<LineElement> GetAllElements()
            => _segments.SelectMany(s => s.GetAllElements());

        public Line(IEnumerable<LineSegment> segments)
        {
            _segments = segments.ToArray();

            var boundingRectangle = Rectangle.Union(segments.Select(s => s.PageRegion));
            this.Size = boundingRectangle.Size;
        }

        public override void SetPosition(DocumentPosition position)
        {
            base.SetPosition(position);
            foreach(var segment in _segments)
            {
                segment.SetPosition(position);
            }
        }

        public override void Render(IRendererPage page)
        {
            foreach(var segment in _segments)
            {
                segment.Render(page);
            }
        }

        //public FieldUpdateResult Update(DocumentPosition documentPosition, DocumentVariables variables)
        //{
        //    foreach (var s in _segments)
        //    {
        //        var result = s.Update(documentPosition, variables);
        //        if(result == ReconstructionNecessary)
        //        {
        //            return result;
        //        }
        //    }

        //    return NoChange;
        //}

        //protected override PreparationState PrepareCore(IPageRegion renderer)
        //{
        //    if(this.BoundingBox.Height > renderer.Size.Height)
        //    {
        //        return OutOfRenderArea;
        //    }

        //    foreach(var segment in _segments)
        //    {
        //        segment.Prepare(renderer);
        //    }

        //    return Prepared;
        //}

        public FieldUpdateResult UpdateFields()
        {
            // var updateResult = _segments.UpdateFields();
            return NoChange;
        }

        //public override sealed void Render()
        //{
        //    _segments.Render();
        //}
    }
}
