using System.Diagnostics;
using Sidea.DocxToPdf.Core;
using Sidea.DocxToPdf.Models.Common;

namespace Sidea.DocxToPdf.Models.Paragraphs
{
    [DebuggerDisplay("{GetType().Name}:{_content}")]
    internal abstract class Field : LineElement
    {
        private readonly TextStyle _textStyle;
        private Rectangle _lineBoundingBox = Rectangle.Empty;
        private string _content = string.Empty;

        protected Field(TextStyle textStyle)
        {
            _textStyle = textStyle;
        }

        public override double GetBaseLineOffset()
            => _textStyle.CellAscent;

        public override sealed void Justify(DocumentPosition position, double baseLineOffset, double lineHeight)
        {
            this.SetPosition(position);
        }

        //public override void SetLineBoundingBox(Rectangle rectangle, double baseLineOffset)
        //{
        //    _lineBoundingBox = rectangle;
        //    var y = baseLineOffset - _textStyle.CellAscent;
        //    this.SetOffset(new Point(rectangle.X, y));
        //}

        //public FieldUpdateResult Update(PageVariables variables)
        //{
        //    this.UpdateCore(variables);
        //    return this.Update();
        //}

        //public FieldUpdateResult Update()
        //{
        //    var currentSize = this.BoundingBox.Size;

        //    _content = this.GetContent();
        //    var size = _textStyle.MeasureText(_content);
        //    this.BoundingBox = new Rectangle(this.BoundingBox.TopLeft, size);

        //    var result = this.BoundingBox.Height > currentSize.Height || this.BoundingBox.Width > currentSize.Width
        //        ? FieldUpdateResult.BoundingBoxResized
        //        : FieldUpdateResult.NoChange;

        //    return result;
        //}

        //public override void Render()
        //{
        //    if (_textStyle.Background != System.Drawing.Color.Empty)
        //    {
        //        this.Renderer.RenderRectangle(_lineBoundingBox, _textStyle.Background);
        //    }

        //    this.Renderer.RenderText(_content, _textStyle, this.BoundingBox);
        //}

        protected abstract string GetContent();

        // protected abstract void UpdateCore(PageVariables variables);
    }
}
