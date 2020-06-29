using System.Collections.Generic;
using System.Linq;
using Sidea.DocxToPdf.Core;
using Sidea.DocxToPdf.Models.Common;
using Sidea.DocxToPdf.Models.Paragraphs.Elements.Fields;
using Sidea.DocxToPdf.Models.Styles;
using Word = DocumentFormat.OpenXml.Wordprocessing;

namespace Sidea.DocxToPdf.Models.Paragraphs.Builders
{
    internal static class FieldBuilder
    {
        public static LineElement CreateField(
            this ICollection<Word.Run> runs,
            IStyleFactory styleFactory)
        {
            var style = styleFactory.EffectiveTextStyle(runs.First().RunProperties);

            var run = runs
                .Skip(1)
                .First();

            var fieldCode = run
                .ChildsOfType<Word.FieldCode>()
                .Single();

            var text = fieldCode.Text;
            var field = text.CreateField(style);
            field.Update(PageVariables.Empty);
            return field;
        }

        private static Field CreateField(
            this string text,
            TextStyle style)
        {
            var items = text.Split("\\");
            switch (items[0].Trim())
            {
                case "PAGE":
                    return new PageNumberField(style);
                case "NUMPAGES":
                    return new TotalPagesField(style);
                default:
                    return new EmptyField(style);
            }
        }
    }
}
