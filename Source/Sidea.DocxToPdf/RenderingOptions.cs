namespace Sidea.DocxToPdf
{
    public class RenderingOptions   
    {
        public static readonly RenderingOptions Default = new RenderingOptions();

        public RenderingOptions(
            bool renderParagraphCharacter = false,
            bool renderSectionRegionBoundaries = false)
        {
            this.RenderHiddenChars = renderParagraphCharacter;
            this.RenderSectionRegionBoundaries = renderSectionRegionBoundaries;
        }

        /// <summary>
        /// e.g. Paragraph, PageBreak, SectionBreak
        /// </summary>
        public bool RenderHiddenChars { get; }

        /// <summary>
        /// Bounding rectangle of a section
        /// </summary>
        public bool RenderSectionRegionBoundaries { get; }
    }
}
