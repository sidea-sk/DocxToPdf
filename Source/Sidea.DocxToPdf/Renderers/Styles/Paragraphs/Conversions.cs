using System.Collections.Generic;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;

namespace Sidea.DocxToPdf.Renderers.Styles
{
    internal static class Conversions
    {
        public static ParagraphSpacing Spacing(
            this Paragraph paragraph,
            ParagraphSpacing defaultParagraphSpacing)
        {
            var spacingXml = paragraph.ParagraphProperties?.SpacingBetweenLines;
            return spacingXml.ToParagraphSpacing(defaultParagraphSpacing);
        }

        public static ParagraphSpacing ToEffectiveParagraphSpacing(
            this IEnumerable<SpacingBetweenLines> prioritized,
            ParagraphSpacing defaultSpacing)
        {
            StringValue before = null;
            StringValue after = null;
            StringValue line = null;
            EnumValue<LineSpacingRuleValues> lineRule = null;

            foreach(var spacing in prioritized)
            {
                before = before ?? spacing.Before;
                after = after ?? spacing.After;
                line = line ?? spacing.Line;
                lineRule = lineRule ?? spacing.LineRule;

                if(before != null && after != null && line != null)
                {
                    break;
                }
            }

            var bf = before?.ToXUnit() ?? defaultSpacing.Before;
            var af = after?.ToXUnit() ?? defaultSpacing.After;
            var ls = line?.GetLineSpacing(lineRule) ?? defaultSpacing.Line;

            return new ParagraphSpacing(ls, bf, af);
        }

        public static ParagraphSpacing ToParagraphSpacing(
            this SpacingBetweenLines spacingXml,
            ParagraphSpacing ifNull)
        {
            if (spacingXml == null)
            {
                return ifNull;
            }

            var before = spacingXml.Before.ToXUnit();
            var after = spacingXml.After.ToXUnit();
            var line = spacingXml.GetLineSpacing();

            return new ParagraphSpacing(line, before, after);
        }

        public static LineAlignment GetLinesAlignment(
            this Justification justification,
            LineAlignment ifNull)
        {
            if (justification == null)
            {
                return ifNull;
            }

            return justification.Val.Value switch
            {
                JustificationValues.Right => LineAlignment.Right,
                JustificationValues.Center => LineAlignment.Center,
                JustificationValues.Both => LineAlignment.Justify,
                _ => LineAlignment.Left,
            };
        }

        private static LineSpacing GetLineSpacing(this SpacingBetweenLines spacingBetweenLines)
        {
            var rule = spacingBetweenLines.LineRule?.Value ?? LineSpacingRuleValues.Auto;

            LineSpacing lineSpacing = rule switch
            {
                LineSpacingRuleValues.Auto => new AutoLineSpacing(spacingBetweenLines.Line?.ToLong() ?? AutoLineSpacing.Default),
                LineSpacingRuleValues.Exact => new ExactLineSpacing(spacingBetweenLines.Line.ToXUnit()),
                LineSpacingRuleValues.AtLeast => new AtLeastLineSpacing(spacingBetweenLines.Line.ToXUnit()),
                _ => new AutoLineSpacing()
            };

            return lineSpacing;
        }

        private static LineSpacing GetLineSpacing(this StringValue line, EnumValue<LineSpacingRuleValues> lineRule)
        {
            var rule = lineRule?.Value ?? LineSpacingRuleValues.Auto;

            LineSpacing lineSpacing = rule switch
            {
                LineSpacingRuleValues.Auto => new AutoLineSpacing(line?.ToLong() ?? AutoLineSpacing.Default),
                LineSpacingRuleValues.Exact => new ExactLineSpacing(line.ToXUnit()),
                LineSpacingRuleValues.AtLeast => new AtLeastLineSpacing(line.ToXUnit()),
                _ => new AutoLineSpacing()
            };

            return lineSpacing;
        }
    }
}
