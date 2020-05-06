using System.Collections.Generic;
using System.Linq;
using DocumentFormat.OpenXml.Wordprocessing;
using PdfSharp.Drawing;
using Sidea.DocxToPdf.Renderers.Paragraphs.Models;
using Sidea.DocxToPdf.Renderers.Paragraphs.Models.Fields;

namespace Sidea.DocxToPdf.Renderers.Paragraphs.Builders
{
    internal static class FieldBuilder
    {
        public static RField CreateField(this ICollection<Run> runs, XFont defaultFont)
        {
            var style = runs.First().Style(defaultFont);

            var run = runs
                .Skip(1)
                .First();

            var fieldCode = run
                .ChildsOfType<FieldCode>()
                .Single();

            var text = fieldCode.Text;
            var field = text.CreateField(style);
            return field;
        }

        public static bool IsFieldStart(this Run run)
        {
            return run
                .Descendants<FieldChar>()
                .Where(fc => fc.FieldCharType == FieldCharValues.Begin)
                .Any();
        }

        public static bool IsFieldEnd(this Run run)
        {
            return run
                .Descendants<FieldChar>()
                .Where(fc => fc.FieldCharType == FieldCharValues.End)
                .Any();
        }

        private static RField CreateField(this string text, RStyle style)
        {
            var items = text.Split("\\");
            switch (items[0].Trim())
            {
                case "PAGE":
                    return new RPageNumberField(style);
                default:
                    return new REmptyField();
            }
        }
    }
}
