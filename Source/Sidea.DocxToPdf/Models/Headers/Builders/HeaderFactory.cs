using Sidea.DocxToPdf.Core;
using Sidea.DocxToPdf.Models.Common;
using Sidea.DocxToPdf.Models.Styles;
using Word = DocumentFormat.OpenXml.Wordprocessing;

namespace Sidea.DocxToPdf.Models.Headers.Builders
{
    internal static class HeaderFactory
    {
        public static HeaderBase CreateHeader(
            this Word.Header wordHeader,
            PageMargin pageMargin,
            IImageAccessor imageAccessor,
            IStyleFactory styleFactory)
        {
            if(wordHeader == null)
            {
                return new NoHeader(pageMargin);
            }

            var childElements = wordHeader.RenderableChildren().CreatePageElements(imageAccessor, styleFactory);
            return new Header(childElements, pageMargin);
        }
    }
}
