using System.Collections.Generic;
using System.Linq;
using DocumentFormat.OpenXml;
using Sidea.DocxToPdf.Models.Styles;
using Word = DocumentFormat.OpenXml.Wordprocessing;

namespace Sidea.DocxToPdf.Models
{
    internal static class Factory
    {
        public static IEnumerable<ContainerElement> CreateInitializeElements(this IEnumerable<OpenXmlCompositeElement> openXmlComposites, IStyleFactory styleFactory)
        {
            return openXmlComposites
                .Select(xml =>
                {
                    var e = xml.CreateElement(styleFactory);
                    return e;
                });
        }

        private static ContainerElement CreateElement(this OpenXmlCompositeElement openXmlComposite, IStyleFactory styleFactory)
        {
            return openXmlComposite switch
            {
                Word.Paragraph p => Paragraphs.Paragraph.Create(p, styleFactory),
                Word.Table t => Tables.Table.From(t, styleFactory),
                _ => throw new RendererException("Unhandled element")
            };
        }
    }
}
