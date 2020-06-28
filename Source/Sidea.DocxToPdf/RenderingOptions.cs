namespace Sidea.DocxToPdf
{
    public class RenderingOptions
    {
        public static readonly RenderingOptions Default = new RenderingOptions();

        public RenderingOptions(
            bool hiddenChars = false,
            bool sectionRegionBoundaries = false,
            bool paragraphRegionBoundaries = false,
            bool lineRegionBoundaries = false,
            bool wordRegionBoundaries = false)
        {
            this.HiddenChars = hiddenChars;
            this.SectionRegionBoundaries = sectionRegionBoundaries;
            this.ParagraphRegionBoundaries = paragraphRegionBoundaries;
            this.LineRegionBoundaries = lineRegionBoundaries;
            this.WordRegionBoundaries = wordRegionBoundaries;
        }

        /// <summary>
        /// e.g. Paragraph, PageBreak, SectionBreak
        /// </summary>
        public bool HiddenChars { get; }

        /// <summary>
        /// Bounding rectangle of a section
        /// </summary>
        public bool SectionRegionBoundaries { get; }

        /// <summary>
        /// Bounding rectangle of a section
        /// </summary>
        public bool ParagraphRegionBoundaries { get; }

        /// <summary>
        /// Bounding rectangle of a section
        /// </summary>
        public bool LineRegionBoundaries { get; }

        /// <summary>
        /// Bounding rectangle of a section
        /// </summary>
        public bool WordRegionBoundaries { get; }
    }
}
