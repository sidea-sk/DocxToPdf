using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PdfSharp.Drawing;

namespace Sidea.DocxToPdf.Renderers.Paragraphs.Models
{
    internal static class Extensions
    {
        public static XSize CalculateBoundingAreaSize(this IEnumerable<RWord> words)
        {
            var size = words.Aggregate(
                new XSize(0, 0),
                (acc, word) =>
                {
                    var height = Math.Max(acc.Height, word.Size.Height);
                    return new XSize(acc.Width + word.Size.Width, height);
                });

            return size;
        }

        public static XSize CalculateBoundingAreaSize(this IEnumerable<RLine> lines)
        {
            var size = lines.Aggregate(
                new XSize(0, 0),
                (acc, line) =>
                {
                    var height = Math.Max(acc.Height, line.Height);
                    return new XSize(acc.Width + line.Width, height);
                });

            return size;
        }
    }
}
