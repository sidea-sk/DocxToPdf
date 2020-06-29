using System.Diagnostics;
using Sidea.DocxToPdf.Core;
using Sidea.DocxToPdf.Models.Common;

namespace Sidea.DocxToPdf.Models.Paragraphs
{
    [DebuggerDisplay("{GetType().Name}:{_content}")]
    internal abstract class TextElement : LineElement
    {
        private readonly string _hiddenContent;

        private readonly string _content;
        private readonly TextStyle _textStyle;

        protected TextElement(string content, string hiddenContent, TextStyle textStyle)
        {
            _content = content;
            _hiddenContent = hiddenContent;
            _textStyle = textStyle;

            var size = string.IsNullOrEmpty(content)
                ? textStyle.MeasureText(hiddenContent).SetWidth(0)
                : textStyle.MeasureText(content);

            this.Size = size;
        }

        //public override void SetLineBoundingBox(Rectangle rectangle, double baseLineOffset)
        //{
        //    _lineBoundingBox = rectangle;
        //    var y = baseLineOffset - this.GetBaseLineOffset();
        //    / / this.SetOffset(new Point(rectangle.X, y));
        //}

        public override sealed double GetBaseLineOffset()
        {
            return _textStyle.CellAscent;
        }

        public sealed override void Justify(DocumentPosition position, double baseLineOffset, double lineHeight)
        {
            var y = baseLineOffset - this.GetBaseLineOffset();
            this.SetPosition(position + new Point(0, y));
        }

        //public override sealed void Render()
        //{
        //    var t = this.Renderer.Options.HiddenChars && !string.IsNullOrEmpty(_hiddenContent)
        //        ? _hiddenContent
        //        : _content;

        //    if(_textStyle.Background != System.Drawing.Color.Empty)
        //    {
        //        this.Renderer.RenderRectangle(_lineBoundingBox, _textStyle.Background);
        //    }

        //    this.Renderer.RenderText(t, _textStyle, this.BoundingBox);
        //}
    }
}
