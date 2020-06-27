using System.Linq;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace Sidea.DocxToPdf
{
    internal static class DocumentSettingsXmlExtensions
    {
        public static bool EvenOddHeadersAndFooters(this DocumentSettingsPart documentSettingsPart)
        {
            var hasElement = documentSettingsPart.Settings?.ChildsOfType<EvenAndOddHeaders>().Any() ?? false;
            return hasElement;
        }
    }
}
