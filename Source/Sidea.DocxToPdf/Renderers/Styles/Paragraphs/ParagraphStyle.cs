﻿using System.Collections.Generic;
using System.Linq;
using DocumentFormat.OpenXml.Wordprocessing;

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

        public ParagraphStyle Override(
            ParagraphProperties paragraphProperties,
            IReadOnlyCollection<StyleParagraphProperties> styleParagraphs)
        {
            if(paragraphProperties == null && styleParagraphs.Count == 0)
            {
                return this;
            }

            var lineAlignment = paragraphProperties == null
                ? this.LineAlignment
                : paragraphProperties.Justification.GetLinesAlignment(this.LineAlignment);

            var spacing = styleParagraphs
                .Select(sp => sp.SpacingBetweenLines)
                .Where(sp => sp != null)
                .ToEffectiveParagraphSpacing(this.Spacing);

            return new ParagraphStyle(lineAlignment, spacing);
        }

        public static ParagraphStyle From(ParagraphPropertiesBaseStyle style)
        {
            var spacing = style.SpacingBetweenLines.ToParagraphSpacing(ParagraphSpacing.Default);
            var lineAlignment = style.Justification.GetLinesAlignment(LineAlignment.Left);

            return new ParagraphStyle(lineAlignment, spacing);
        }
    }
}
