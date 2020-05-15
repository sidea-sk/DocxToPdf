using System.Diagnostics;
using PdfSharp.Drawing;
using Sidea.DocxToPdf.Renderers.Core;
using Sidea.DocxToPdf.Renderers.Core.RenderingAreas;
using Sidea.DocxToPdf.Renderers.Styles;

namespace Sidea.DocxToPdf.Renderers.Paragraphs.Models
{
    [DebuggerDisplay("{_content}")]
    internal class RText : RLineElement
    {
        private readonly string _content;
        private readonly TextStyle _textSyle;

        public RText(string content, TextStyle textSyle)
        {
            _content = content;
            _textSyle = textSyle;
        }

        public override bool OmitableAtLineBegin => _content == " ";

        public override bool OmitableAtLineEnd => _content == " ";

        public int TextLength => _content.Length;

        public RText Substring(int fromIndex, int length)
        {
            return new RText(_content.Substring(fromIndex, length), _textSyle);
        }

        public static RText Empty(TextStyle textStyle) => new RText(string.Empty, textStyle);

        protected override XSize CalculateContentSizeCore(IPrerenderArea prerenderArea)
        {
            return prerenderArea.MeasureText(_content, _textSyle.Font);
        }

        protected override RenderResult RenderCore(IRenderArea renderArea)
        {
            var rect = new XRect(new XPoint(0, renderArea.Height - this.PrecalulatedSize.Height), this.PrecalulatedSize);
             renderArea.DrawText(_content, _textSyle.Font, _textSyle.Brush, rect, XStringFormats.TopLeft);
            return RenderResult.Done(this.PrecalulatedSize.Width, this.PrecalulatedSize.Height);
        }
    }
}
