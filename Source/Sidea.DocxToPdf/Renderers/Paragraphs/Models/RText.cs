using System.Diagnostics;
using PdfSharp.Drawing;
using Sidea.DocxToPdf.Renderers.Core;
using Sidea.DocxToPdf.Renderers.Core.RenderingAreas;

namespace Sidea.DocxToPdf.Renderers.Paragraphs.Models
{
    [DebuggerDisplay("{_content}")]
    internal class RText : RLineElement
    {
        private readonly string _content;
        private readonly XFont _font;
        private readonly XBrush _brush;

        public RText(string content, XFont font, XBrush brush)
        {
            _content = content;
            _font = font;
            _brush = brush;
        }

        public override bool OmitableAtLineBegin => _content == " ";

        public override bool OmitableAtLineEnd => _content == " ";

        public int TextLength => _content.Length;

        public RText Substring(int fromIndex, int length)
        {
            return new RText(_content.Substring(fromIndex, length), _font, _brush);
        }

        public static RText Empty(XFont font) => new RText(string.Empty, font, XBrushes.Black);

        protected override XSize CalculateContentSizeCore(IPrerenderArea prerenderArea)
        {
            return prerenderArea.MeasureText(_content, _font);
        }

        protected override RenderResult RenderCore(IRenderArea renderArea)
        {
            var rect = new XRect(new XPoint(0, renderArea.Height - this.PrecalulatedSize.Height), this.PrecalulatedSize);
             renderArea.DrawText(_content, _font, _brush, rect, XStringFormats.TopLeft);
            return RenderResult.Done(this.PrecalulatedSize.Width, this.PrecalulatedSize.Height);
        }
    }
}
