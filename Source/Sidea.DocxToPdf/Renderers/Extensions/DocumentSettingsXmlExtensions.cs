using System.Linq;
using DocumentFormat.OpenXml.Wordprocessing;

namespace Sidea.DocxToPdf.Renderers
{
    internal static class DocumentSettingsXmlExtensions
    {
        public static bool UseEvenOdHeaders(this Settings settings)
        {
            var evenOdd = settings
                .ChildElements
                .OfType<EvenAndOddHeaders>()
                .SingleOrDefault();

            return evenOdd.IsOn(true);
        }
    }
}
