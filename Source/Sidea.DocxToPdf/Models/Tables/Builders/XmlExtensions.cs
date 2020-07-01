using Sidea.DocxToPdf.Models.Tables.Elements;
using Word = DocumentFormat.OpenXml.Wordprocessing;

namespace Sidea.DocxToPdf.Models.Tables.Builders
{
    internal static class XmlExtensions
    {
        public static BorderStyle GetBorderStyle(this Word.TableCell cell)
            => cell.TableCellProperties.GetBorderStyle();

        private static BorderStyle GetBorderStyle(this Word.TableCellProperties properties)
        {
            return properties?.TableCellBorders.ToCellBorderStyle();
        }

        private static BorderStyle ToCellBorderStyle(this Word.TableCellBorders borders)
        {
            if (borders == null)
            {
                return null;
            }

            var top = borders.TopBorder.ToPen();
            var right = borders.RightBorder.ToPen();
            var bottom = borders.BottomBorder.ToPen();
            var left = borders.LeftBorder.ToPen();

            return new BorderStyle(top, right, bottom, left);
        }
    }
}
