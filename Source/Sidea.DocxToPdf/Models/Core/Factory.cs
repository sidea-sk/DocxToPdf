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
                .Select(e =>
                {
                    var p = Factory.CreateElement(e, styleFactory);
                    p.Initialize();
                    return p;
                });
        }

        private static ContainerElement CreateElement(OpenXmlCompositeElement openXmlComposite, IStyleFactory styleFactory)
        {
            return openXmlComposite switch
            {
                Word.Paragraph p => new Paragraphs.Paragraph(p, styleFactory),
                Word.Table t => new Tables.Table(t, styleFactory),
                _ => throw new RendererException("Unhandled element")
            };
        }
    }
}
