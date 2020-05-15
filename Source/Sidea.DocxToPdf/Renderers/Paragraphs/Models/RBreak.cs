using PdfSharp.Drawing;
using Sidea.DocxToPdf.Renderers.Core;
using Sidea.DocxToPdf.Renderers.Core.RenderingAreas;
using Sidea.DocxToPdf.Renderers.Styles;

namespace Sidea.DocxToPdf.Renderers.Paragraphs.Models
{
    internal class RBreak : RLineElement
    {
        private readonly string _text;
        private readonly TextStyle _textStyle;

        public RBreak(string name, TextStyle textStyle)
        {
            _text = $"------ {name} Break ------";
            // _font = new XFont(font.Name, XUnit.FromPoint(6));
            _textStyle = textStyle;
        }

        public override bool OmitableAtLineBegin => false;

        public override bool OmitableAtLineEnd => false;

        protected override XSize CalculateContentSizeCore(IPrerenderArea prerenderArea)
        {
            var size = new XSize(0, 0);
            if (prerenderArea.Options.RenderHiddenChars)
            {
                size = prerenderArea.MeasureText(_text, _textStyle.Font);
            }
            return size;
        }

        protected override RenderResult RenderCore(IRenderArea renderArea)
        {
            if (renderArea.Options.RenderHiddenChars)
            {
                var rect = new XRect(0, 0, this.PrecalulatedSize.Width, renderArea.Height);
                renderArea.DrawText(_text, _textStyle.Font, _textStyle.Brush, rect, XStringFormats.CenterLeft);
            }

            return RenderResult.Done(this.PrecalulatedSize);
        }
    }
}
