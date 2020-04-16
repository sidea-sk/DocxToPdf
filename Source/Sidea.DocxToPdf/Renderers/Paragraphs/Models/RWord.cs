using System.Diagnostics;
using PdfSharp.Drawing;
using Sidea.DocxToPdf.Renderers.Core.RenderingAreas;
using Sidea.DocxToPdf.Renderers.Core.Serices;

namespace Sidea.DocxToPdf.Renderers.Paragraphs.Models
{
    [DebuggerDisplay("{_content}")]
    internal class RWord
    {
        private readonly string _content;
        private readonly XFont _font;
        private readonly XBrush _brush;
        private readonly ITextMeasuringService _textMeasuringService;

        public RWord(string content, XFont font, XBrush brush, ITextMeasuringService textMeasuringService)
        {
            _content = content;
            _font = font;
            _brush = brush;
            _textMeasuringService = textMeasuringService;

            this.Size = _textMeasuringService.MeasureText(_content, _font);
        }

        public bool IsSpace => _content == " ";

        public XSize Size { get; }

        public double Width => this.Size.Width;

        public double Height => this.Size.Height;

        public XPoint Render(IRenderArea toArea, XPoint offset)
        {
            toArea.DrawText(_content, _font, _brush, offset);
            return offset + this.Size;
        }

        public XSize MeasureWithSameFormatting(string text)
        {
            return _textMeasuringService.MeasureText(text, _font);
        }

        public RWord CreateWithSameFormatting(string content)
        {
            return new RWord(content, _font, _brush, _textMeasuringService);
        }

        public static implicit operator string(RWord word) => word._content;
    }
}
