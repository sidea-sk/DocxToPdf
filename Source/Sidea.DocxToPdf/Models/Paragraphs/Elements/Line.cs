using System.Collections.Generic;
using System.Linq;
using Sidea.DocxToPdf.Core;

using static Sidea.DocxToPdf.Models.FieldUpdateResult;

namespace Sidea.DocxToPdf.Models.Paragraphs
{
    internal class Line // : SinglePageElementBase, IFieldsElement
    {
        private readonly LineSegment[] _segments;

        public Rectangle BoundingBox { get; set; } = Rectangle.Empty;

        public IEnumerable<ParagraphElement> GetAllElements()
            => _segments.SelectMany(s => s.GetAllElements());

        public Line(IEnumerable<LineSegment> segments)
        {
            _segments = segments.ToArray();
            this.BoundingBox = Rectangle.Union(_segments.Select(s => s.BoundingBox));
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
