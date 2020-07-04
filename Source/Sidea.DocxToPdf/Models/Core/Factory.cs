using System.Collections.Generic;
using System.Linq;
using DocumentFormat.OpenXml;
using Sidea.DocxToPdf.Models.Styles;
using Word = DocumentFormat.OpenXml.Wordprocessing;

namespace Sidea.DocxToPdf.Models
{
    internal static class Factory
    {
        public static IEnumerable<PageContextElement> CreateInitializeElements(this IEnumerable<OpenXmlCompositeElement> openXmlComposites, IStyleFactory styleFactory)
        {
            return openXmlComposites
                .Select(xml =>
                {
                    var e = xml.CreateElement(styleFactory);
                    return e;
                });
        }

        private static PageContextElement CreateElement(this OpenXmlCompositeElement openXmlComposite, IStyleFactory styleFactory)
        {
            return openXmlComposite switch
            {
                Word.Paragraph p => Paragraphs.Builders.ParagraphFactory.Create(p, styleFactory),
                Word.Table t => Tables.Builders.TableFactory.Create(t, styleFactory),
                _ => throw new RendererException("Unhandled element")
            };
        }
    }
}
