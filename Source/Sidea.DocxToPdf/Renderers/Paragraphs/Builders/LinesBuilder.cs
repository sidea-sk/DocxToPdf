﻿using System;
using System.Collections.Generic;
using System.Linq;
using DocumentFormat.OpenXml.Drawing.Wordprocessing;
using DocumentFormat.OpenXml.Wordprocessing;
using PdfSharp.Drawing;
using Sidea.DocxToPdf.Renderers.Core.RenderingAreas;
using Sidea.DocxToPdf.Renderers.Paragraphs.Models;
using Sidea.DocxToPdf.Renderers.Styles;

namespace Sidea.DocxToPdf.Renderers.Paragraphs.Builders
{
    internal static class LinesBuilder
    {
        public static IEnumerable<RFixedDrawing> CreateFixedDrawings(this Paragraph paragraph)
        {
            var drawings = paragraph
                .Descendants<Drawing>()
                .Where(d => d.Anchor != null)
                .Select(d => d.Anchor.FromAnchor())
                .ToArray();

            return drawings;
        }

        public static IEnumerable<RLine> CreateRenderingLines(
            this Paragraph paragraph,
            IReadOnlyCollection<RFixedDrawing> fixedDrawings,
            LineSpacing lineSpacing,
            IPrerenderArea prerenderArea)
        {
            var lineAlignment = paragraph.GetLinesAlignment();

            var elements = paragraph
                .ToLineElements(prerenderArea)
                .ToStack();

            var lines = new List<RLine>();
            var vOffset = new XUnit(0);
            do
            {
                var line = LineBuilder.CreateLine(elements, lineAlignment, vOffset, fixedDrawings, prerenderArea);
                line.CalculateContentSize(prerenderArea);
                lines.Add(line);

                vOffset += line.PrecalulatedSize.Height
                    + lineSpacing.CalculateSpaceAfterLine(line);
            } while (elements.Count > 0);

            return lines;
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

        private static IEnumerable<RLineElement> ToLineElements(this Paragraph paragraph, IPrerenderArea prerenderArea)
        {
            var runs = paragraph
                .ChildElements
                .Where(child => child is Run || child is SdtRun)
                .SelectMany(child =>
                {
                    return child switch
                    {
                        Run r => new[] { r },
                        SdtRun sdtRun => sdtRun.SdtContentRun.ChildElements.Cast<Run>(),
                        _ => new Run[0]
                    };
                })
                .ToStack();

            var elements = new List<RLineElement>();
            while(runs.Count > 0)
            {
                var run = runs.Pop();
                if (run.IsFieldStart())
                {
                    var fieldRuns = new List<Run> { run };
                    do
                    {
                        run = runs.Pop();
                        fieldRuns.Add(run);
                    } while (!run.IsFieldEnd());

                    var field = fieldRuns.CreateField(prerenderArea.AreaFont);
                    elements.Add(field);
                }
                else
                {
                    var runElements = run.ToLineElements(prerenderArea.AreaFont);
                    elements.AddRange(runElements);
                }
            }

            return elements;
        }

        private static IEnumerable<RLineElement> ToLineElements(this Run run, XFont defaultFont)
        {
            var runStyle = run.Style(defaultFont);
            
            var xtexts = run
                .ChildElements
                .Where(c => c is Text || c is TabChar || c is Drawing || c is Break)
                .SelectMany(c =>
                {
                    return c switch
                    {
                        Text t => t.SplitToWords(runStyle.Font, runStyle.Brush).Cast<RLineElement>(),
                        TabChar t => new RLineElement[] { new RText("    ", runStyle.Font, runStyle.Brush) },
                        Drawing d => d.ToRInlineDrawing(),
                        Break b => b.ToRLineElements(defaultFont),
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

        private static RLineElement[] ToRLineElements(this Break @break, XFont font)
        {
            return new RLineElement[] { new RBreak(@break.Type.Value.ToString(), font) };
        }

        private static RInlineDrawing[] ToRInlineDrawing(this Drawing drawing)
        {
            if(drawing.Inline == null)
            {
                return new RInlineDrawing[0];
            }

            var rdrawing = drawing.Inline.FromInline();
            return new[] { rdrawing };
        }

        private static RInlineDrawing FromInline(this Inline inline)
        {
            var size = inline.Extent.Size();
            var blipElement = inline.Descendants<DocumentFormat.OpenXml.Drawing.Blip>().First();

            return new RInlineDrawing(blipElement.Embed.Value, size);
        }

        private static RFixedDrawing FromAnchor(this Anchor anchor)
        {
            var size = anchor.Extent.Size();
            var blipElement = anchor.Descendants<DocumentFormat.OpenXml.Drawing.Blip>().First();

            var position = anchor.SimplePos.Value
                ? new XPoint(anchor.SimplePosition.X.Value, anchor.SimplePosition.Y.Value)
                : new XPoint(anchor.HorizontalPosition.PositionOffset.ToXUnit(), anchor.VerticalPosition.PositionOffset.ToXUnit());

            return new RFixedDrawing(blipElement.Embed.Value, position, size);
        }
    }
}
