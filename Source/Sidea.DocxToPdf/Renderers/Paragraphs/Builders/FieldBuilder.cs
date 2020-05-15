using System.Collections.Generic;
using System.Linq;
using DocumentFormat.OpenXml.Wordprocessing;
using Sidea.DocxToPdf.Renderers.Paragraphs.Models.Fields;
using Sidea.DocxToPdf.Renderers.Styles;

namespace Sidea.DocxToPdf.Renderers.Paragraphs.Builders
{
    internal static class FieldBuilder
    {
        public static RField CreateField(this ICollection<Run> runs, IStyleAccessor styleAccessor)
        {
            var style = styleAccessor.EffectiveStyle(runs.First().RunProperties);

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

        private static RField CreateField(this string text, TextStyle style)
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
