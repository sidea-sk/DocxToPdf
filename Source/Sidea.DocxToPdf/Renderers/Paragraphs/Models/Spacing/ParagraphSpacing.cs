using PdfSharp.Drawing;

namespace Sidea.DocxToPdf.Renderers.Paragraphs.Models.Spacing
{
    internal class ParagraphSpacing
    {
        public static readonly ParagraphSpacing Default = new ParagraphSpacing(new AutoLineSpacing(), new XUnit(0), new XUnit(10));

        public ParagraphSpacing(LineSpacing line, XUnit before, XUnit after)
        {
            this.Line = line;
            this.Before = before;
            this.After = after;
        }

        public LineSpacing Line { get; }
        public XUnit Before { get; }
        public XUnit After { get; }
    }
}
