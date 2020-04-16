using System;
using System.Collections.Generic;
using System.Linq;
using DocumentFormat.OpenXml.Wordprocessing;
using PdfSharp.Drawing;
using Sidea.DocxToPdf.Renderers.Core;
using Sidea.DocxToPdf.Renderers.Core.RenderingAreas;
using Sidea.DocxToPdf.Renderers.Core.Serices;
using Sidea.DocxToPdf.Renderers.Paragraphs.Models;

namespace Sidea.DocxToPdf.Renderers.Paragraphs.Builders
{
    internal static class LinesBuilder
    {
        public static IEnumerable<RLine> ToRenderingLines(this Paragraph paragraph, IRenderingAreaBase renderArea)
        {
            var lineAlignment = paragraph.GetLinesAlignment();

            var xtexts = paragraph.ChildElements
                .OfType<Run>()
                .SelectMany(r => r.ToWords(renderArea.AreaFont, renderArea))
                .ToArray();

            var lines = xtexts.ChunkWordsToLines(lineAlignment, renderArea.Width, renderArea.AreaFont)
                .ToArray();

            return lines.Length == 0
                ? new[] { new RLine(new RWord[0], lineAlignment, renderArea.AreaFont.GetHeight()) }
                : lines;
        }

        private static IEnumerable<RWord> ToWords(this Run run, XFont defaultFont, ITextMeasuringService textMeasuringService)
        {
            XFont font = run.RunProperties.CreateRunFont(defaultFont);
            XBrush brush = run.RunProperties.Color.ToXBrush();

            var xtexts = run
                .ChildElements
                .Where(c => c is Text || c is TabChar)
                .SelectMany(c =>
                {
                    return c switch
                    {
                        Text t => t.SplitToWords(font, brush, textMeasuringService),
                        TabChar t => new[] { new RWord("    ", font, brush, textMeasuringService) },
                        _ => throw new Exception("unprocessed child")
                    };
                })
                .ToArray();

            return xtexts;
        }

        private static IEnumerable<RWord> SplitToWords(this Text text, XFont font, XBrush brush, ITextMeasuringService textMeasuringService)
        {
            var xtexts = new List<RWord>();
            var xText = text
                .InnerText
                .SplitToLinesWordsWhitespaces()
                .Select(t => new RWord(t, font, brush, textMeasuringService))
                .ToArray();

            return xText;
        }

        private static IEnumerable<RLine> ChunkWordsToLines(this IEnumerable<RWord> words, LineAlignment alignment, double maxWidth, XFont defaultFont)
        {
            double defaultFontHeight = defaultFont.GetHeight();

            var lines = new List<RLine>();
            var stack = words.ToStack();

            var currentLine = new List<RWord>();
            var currentLineWidth = 0d;

            while (stack.Count > 0)
            {
                var word = stack.Pop();

                if (currentLineWidth + word.Width < maxWidth)
                {
                    currentLine.Add(word);
                    currentLineWidth += word.Width;
                    continue;
                }

                if (currentLine.Count > 0)
                {
                    lines.Add(new RLine(currentLine, alignment, defaultFontHeight));
                    stack.Push(word);

                    currentLine.Clear();
                    currentLineWidth = 0;
                }
                else
                {
                    var splits = word.SplitToMaxWidth(maxWidth);
                    foreach (var split in splits.Reverse())
                    {
                        stack.Push(split);
                    }
                }
            }

            if (currentLine.Count > 0)
            {
                lines.Add(new RLine(currentLine, alignment, defaultFontHeight));
            }

            return lines;
        }

        private static IEnumerable<RWord> SplitToMaxWidth(this RWord word, double maxWidth)
        {
            var words = new List<RWord>();

            var substring = string.Empty;
            var wholeWord = (string)word;
            for (var i = 0; i < wholeWord.Length; i++)
            {
                var temp = substring + wholeWord[i];
                var tempSize = word.MeasureWithSameFormatting(temp);
                if (tempSize.Width > maxWidth)
                {
                    words.Add(word.CreateWithSameFormatting(substring));
                    substring = wholeWord[i].ToString();
                }
                else
                {
                    substring = temp;
                }
            }

            return words;
        }

        private static Stack<RWord> ToStack(this IEnumerable<RWord> words)
        {
            var stack = new Stack<RWord>(words.Reverse());
            return stack;
        }

        private static IEnumerable<string> SplitToLinesWordsWhitespaces(this string text)
        {
            var lines = text.SplitByNewLines();
            var result = lines
                 .SelectMany(l => {
                     return l == "\r\n"
                        ? new[] { l }
                        : l.SplitToWordsAndWhitechars();
                 })
                 .ToArray();

            return result;
        }

        private static IEnumerable<string> SplitByNewLines(this string text)
        {
            var lines = text.Split("\r\n");
            var result = lines
                .Take(lines.Length - 1)
                .SelectMany(l => new string[] { l, "\r\n" })
                .Concat(new string[] { lines.Last() });

            return result;
        }

        private static IEnumerable<string> SplitToWordsAndWhitechars(this string text)
        {
            var words = new List<string>();

            var index = 0;
            var word = string.Empty;
            do
            {
                word += text[index].ToString();
                index++;
                if (string.IsNullOrEmpty(word))
                {
                    words.Add(word);
                    word = string.Empty;
                }
                else
                {
                    break;
                }
            }
            while (index < text.Length);

            for (var i = index; i < text.Length; i++)
            {
                if (char.IsWhiteSpace(text[i]))
                {
                    words.Add(word);
                    words.Add(text[i].ToString());
                    word = string.Empty;
                }
                else
                {
                    word += text[i];
                }
            }

            if (word != string.Empty)
            {
                words.Add(word);
            }

            return words;
        }

        private static LineAlignment GetLinesAlignment(this Paragraph paragraph)
        {
            var properties = paragraph.ParagraphProperties;
            if (properties?.Justification == null)
            {
                return LineAlignment.Left;
            }

            return properties.Justification.Val.Value switch
            {
                JustificationValues.Right => LineAlignment.Right,
                JustificationValues.Center => LineAlignment.Center,
                JustificationValues.Both => LineAlignment.Justify,
                _ => LineAlignment.Left,
            };
        }
    }
}
