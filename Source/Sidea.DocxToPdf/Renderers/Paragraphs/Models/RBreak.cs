using PdfSharp.Drawing;
using Sidea.DocxToPdf.Renderers.Core;
using Sidea.DocxToPdf.Renderers.Core.RenderingAreas;

namespace Sidea.DocxToPdf.Renderers.Paragraphs.Models
{
    internal class RBreak : RLineElement
    {
        private readonly string _text;
        private readonly XFont _font;

        public RBreak(string name, XFont font)
        {
            _text = $"------ {name} Break ------";
            _font = new XFont(font.Name, XUnit.FromPoint(6));
        }

        public override bool OmitableAtLineBegin => false;

        public override bool OmitableAtLineEnd => false;

        protected override XSize CalculateContentSizeCore(IPrerenderArea prerenderArea)
        {
            var size = new XSize(0, 0);
            if (prerenderArea.Options.RenderHiddenChars)
            {
                size = prerenderArea.MeasureText(_text, _font);
            }
            return size;
        }

        protected override RenderResult RenderCore(IRenderArea renderArea)
        {
            if (renderArea.Options.RenderHiddenChars)
            {
                var rect = new XRect(0, 0, this.PrecalulatedSize.Width, renderArea.Height);
                renderArea.DrawText(_text, _font, XBrushes.Black, rect, XStringFormats.CenterLeft);
            }

            return RenderResult.Done(this.PrecalulatedSize);
        }
    }
}
