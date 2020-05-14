namespace Sidea.DocxToPdf.Renderers.Styles
{
    internal class ParagraphStyle
    {
        public static readonly ParagraphStyle Default = new ParagraphStyle(LineAlignment.Left, ParagraphSpacing.Default);

        public ParagraphStyle(
            LineAlignment lineAlignment,
            ParagraphSpacing spacing)
        {
            this.LineAlignment = lineAlignment;
            this.Spacing = spacing;
        }

        public LineAlignment LineAlignment { get; }
        public ParagraphSpacing Spacing { get; }
    }
}
