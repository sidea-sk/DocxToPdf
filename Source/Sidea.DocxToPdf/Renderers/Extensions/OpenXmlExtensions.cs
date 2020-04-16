using System.Collections.Generic;
using DocumentFormat.OpenXml;

namespace Sidea.DocxToPdf.Renderers
{
    internal static class OpenXmlExtensions
    {
        public static IEnumerable<T> ChildsOfType<T>(this OpenXmlElement xmlElement)
            where T : OpenXmlElement
        {
            return xmlElement.ChildElements.OfType<T>();
        }
    }
}
