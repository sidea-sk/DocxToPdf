using DocumentFormat.OpenXml.Wordprocessing;
using Sidea.DocxToPdf.Renderers.Paragraphs.Models.Spacing;

namespace Sidea.DocxToPdf.Renderers.Paragraphs.Builders
{
    internal static class Extensions
    {
        public static ParagraphSpacing Spacing(this Paragraph paragraph)
        {
            var spacingXml = paragraph.ParagraphProperties?.SpacingBetweenLines;

            if(spacingXml == null)
            {
                return ParagraphSpacing.Default;
            }

            var before = spacingXml.Before.ToXUnit();
            var after = spacingXml.After.ToXUnit();
            var line = spacingXml.GetLineSpacing();

            return new ParagraphSpacing(line, before, after);
        }

        private static LineSpacing GetLineSpacing(this SpacingBetweenLines spacingBetweenLines)
        {
            var rule = spacingBetweenLines.LineRule?.Value ?? LineSpacingRuleValues.Auto;

            LineSpacing lineSpacing = rule switch
            {
                LineSpacingRuleValues.Auto => new AutoLineSpacing(spacingBetweenLines.Line?.ToLong() ?? AutoLineSpacing.Default),
                LineSpacingRuleValues.Exact => new ExactLineSpacing(spacingBetweenLines.Line.ToXUnit()),
                LineSpacingRuleValues.AtLeast => new AtLeastLineSpacing(spacingBetweenLines.Line.ToXUnit()),
                _ => new AutoLineSpacing(),
            };

            return lineSpacing;
        }
    }
}
