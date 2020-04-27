using System;
using System.Collections.Generic;
using System.Linq;
using DocumentFormat.OpenXml.Drawing.Wordprocessing;
using DocumentFormat.OpenXml.Wordprocessing;
using PdfSharp.Drawing;
using Sidea.DocxToPdf.Renderers.Core.RenderingAreas;
using Sidea.DocxToPdf.Renderers.Paragraphs.Models;

namespace Sidea.DocxToPdf.Renderers.Paragraphs.Builders
{
    internal static class LinesBuilder
    {
        public static IEnumerable<RLine> CreateRenderingLines(
            this Paragraph paragraph,
            IPrerenderArea prerenderArea)
        {
            var lineAlignment = paragraph.GetLinesAlignment();
            var elements = paragraph
                .ChildsOfType<Run>()
                .SelectMany(run => run.ToLineElements(prerenderArea.AreaFont))
                .ToStack();

            var lines = new List<RLine>();
            do
            {
                var line = CreateLine(elements, lineAlignment, prerenderArea);
                line.CalculateContentSize(prerenderArea);
                lines.Add(line);
            } while (elements.Count > 0);

            return lines;
        }

        private static RLine CreateLine(Stack<RLineElement> fromElements, LineAlignment lineAlignment, IPrerenderArea prerenderArea)
        {
            var aggregatedWidth = XUnit.Zero;
            var lineElements = new List<RLineElement>();

            while(fromElements.Count > 0)
            {
                var e = fromElements.Pop();
                e.CalculateContentSize(prerenderArea);

                if (e.PrecalulatedSize.Width + aggregatedWidth <= prerenderArea.Width)
                {
                    lineElements.Add(e);
                    aggregatedWidth += e.PrecalulatedSize.Width;
                    continue;
                }

                if (lineElements.Count == 0)
                {
                    switch (e)
                    {
                        case RDrawing d:
                            lineElements.Add(d);
                            break;

                        case RText t:
                            var (head, tail) = t.CutTextOfMaxWidth(prerenderArea.Width, prerenderArea);
                            lineElements.Add(head);
                            fromElements.Push(tail);
                            break;
                    }
                }
                break;
            }

            var elements = lineElements
                .TrimSpaces()
                .EnsureAtLeastOne(prerenderArea);

            var line = new RLine(elements, lineAlignment, fromElements.Count == 0);
            return line;
        }

        public static IEnumerable<RLineElement> ToLineElements(this Paragraph paragraph, XFont defaultFont)
        {
            var le = paragraph
                .ChildsOfType<Run>()
                .SelectMany(r => r.ToLineElements(defaultFont))
                .ToArray();

            return le.Length == 0
                ? new RLineElement[] { RText.Empty(defaultFont) }
                : le;
        }

        private static IEnumerable<RLineElement> TrimSpaces(this IEnumerable<RLineElement> elements)
        {
            var result = elements
                .SkipWhile(e => e.OmitableAtLineBegin)
                .Reverse()
                .SkipWhile(e => e.OmitableAtLineEnd)
                .Reverse()
                .ToArray();

            return result;
        }

        private static IEnumerable<RLineElement> EnsureAtLeastOne(this IEnumerable<RLineElement> elements, IPrerenderArea prerenderArea)
        {
            var e = elements.ToList();
            if(e.Count > 0)
            {
                return e;
            }

            var empty = RText.Empty(prerenderArea.AreaFont);
            empty.CalculateContentSize(prerenderArea);
            return new List<RLineElement> { empty };
        }

        public static LineAlignment GetLinesAlignment(this Paragraph paragraph)
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

        private static IEnumerable<RLineElement> ToLineElements(this Run run, XFont defaultFont)
        {
            XFont font = run.RunProperties.CreateRunFont(defaultFont);
            XBrush brush = run.RunProperties?.Color.ToXBrush() ?? XBrushes.Black;

            var xtexts = run
                .ChildElements
                .Where(c => c is Text || c is TabChar || c is Drawing)
                .SelectMany(c =>
                {
                    return c switch
                    {
                        Text t => t.SplitToWords(font, brush).Cast<RLineElement>(),
                        TabChar t => new RLineElement[] { new RText("    ", font, brush) },
                        Drawing d => new RLineElement[] { d.ToRDrawing() },
                        _ => throw new Exception("unprocessed child")
                    };
                })
                .ToArray();

            return xtexts;
        }

        private static IEnumerable<RText> SplitToWords(this Text text, XFont font, XBrush brush)
        {
            var xText = text
                .InnerText
                .SplitToLinesOrWordsOrWhitespaces()
                .Select(t => new RText(t, font, brush))
                .ToArray();

            return xText;
        }

        private static IEnumerable<string> SplitToLinesOrWordsOrWhitespaces(this string text)
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
                if (string.IsNullOrWhiteSpace(word))
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

        private static (RText cut, RText tail) CutTextOfMaxWidth(this RText text, XUnit maxWidth, IPrerenderArea prerenderArea)
        {
            RText previous = text.Substring(0, 0);
            previous.CalculateContentSize(prerenderArea);

            RText current;
            for(var i = 1; i < text.TextLength; i++)
            {
                current = text.Substring(0, i);
                current.CalculateContentSize(prerenderArea);

                if(current.PrecalulatedSize.Width > maxWidth)
                {
                    return (previous, text.Substring(i, text.TextLength - i));
                }

                previous = current;
            }

            throw new Exception("Could not found appropriate substring");
        }

        private static RDrawing ToRDrawing(this Drawing drawing)
        {
            var rdraw = drawing.Inline?.FromInline() ?? drawing.Anchor.FromAnchor();
            return rdraw;
        }

        private static RDrawing FromInline(this Inline inline)
        {
            var size = inline.Extent.Size();
            var blipElement = inline.Descendants<DocumentFormat.OpenXml.Drawing.Blip>().First();

            return new RDrawing(
                blipElement.Embed.Value,
                inline.DocProperties.Name,
                inline.Graphic.GraphicData.Uri,
                size);
        }

        private static RDrawing FromAnchor(this Anchor anchor)
        {
            var size = anchor.Extent.Size();
            var blipElement = anchor.Descendants<DocumentFormat.OpenXml.Drawing.Blip>().First();

            var docProperties = anchor.ChildsOfType<DocProperties>().Single();
            var graphic = anchor.ChildsOfType<DocumentFormat.OpenXml.Drawing.Graphic>().Single();

            return new RDrawing(
                blipElement.Embed.Value,
                docProperties.Name,
                graphic.GraphicData.Uri, size);
        }
    }
}
