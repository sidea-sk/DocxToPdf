using System;
using System.Drawing;

namespace Sidea.DocxToPdf
{
    public class RenderingOptions
    {
        public static readonly RenderingOptions Default = new RenderingOptions();

        public RenderingOptions(
            bool hiddenChars = false,
            Pen sectionBorders = null,
            Pen headerBorders = null,
            Pen footerBorders = null,
            Pen paragraphBorders = null,
            Pen lineBorders = null,
            Pen wordBorders = null,
            bool sectionRegionBoundaries = false)
        {
            this.HiddenChars = hiddenChars;
            this.SectionBorders = sectionBorders;
            this.HeaderBorders = headerBorders;
            this.FooterBorders = footerBorders;
            this.ParagraphBorders = paragraphBorders;
            this.LineBorders = lineBorders;
            this.WordBorders = wordBorders;

            this.SectionRegionBoundaries = sectionRegionBoundaries;
        }

        /// <summary>
        /// e.g. Paragraph, PageBreak, SectionBreak
        /// </summary>
        public bool HiddenChars { get; }

        public Pen SectionBorders { get; }
        public Pen HeaderBorders { get; }
        public Pen FooterBorders { get; }
        public Pen ParagraphBorders { get; }
        public Pen LineBorders { get; }
        public Pen WordBorders { get; }

        /// <summary>
        /// Bounding rectangle of a section
        /// </summary>
        [Obsolete]
        public bool SectionRegionBoundaries { get; }

        public static RenderingOptions WithDefaults(
            bool hiddenChars = true,
            bool section = false,
            bool header = false,
            bool footer = false,
            bool paragraph = false,
            bool line = false,
            bool word = false)
        {
            return new RenderingOptions(
                hiddenChars,
                sectionBorders: section ? SectionDefault : null,
                headerBorders: header ? HeaderDefault : null,
                footerBorders: footer ? FooterDefault : null,
                paragraphBorders: paragraph ? ParagraphDefault : null,
                lineBorders: line ? LineDefault : null,
                wordBorders: word ? WordDefault : null
                );
        }

        public static readonly Pen ParagraphDefault = new Pen(Color.Orange, 0.5f);
        public static readonly Pen LineDefault = new Pen(Color.Red, 0.5f);
        public static readonly Pen WordDefault = new Pen(Color.Green, 0.5f);
        public static readonly Pen HeaderDefault = new Pen(Color.LightBlue, 0.5f);
        public static readonly Pen FooterDefault = new Pen(Color.DarkBlue, 0.5f);
        public static readonly Pen SectionDefault = new Pen(Color.Olive, 0.5f);
    }
}
