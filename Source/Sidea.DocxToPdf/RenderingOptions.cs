namespace Sidea.DocxToPdf
{
    public class RenderingOptions   
    {
        public static readonly RenderingOptions Default = new RenderingOptions();

        public RenderingOptions(bool renderParagraphCharacter = false)
        {
            this.RenderHiddenChars = renderParagraphCharacter;
        }

        /// <summary>
        /// e.g. Paragraph, PageBreak, SectionBreak
        /// </summary>
        public bool RenderHiddenChars { get; }
    }
}
