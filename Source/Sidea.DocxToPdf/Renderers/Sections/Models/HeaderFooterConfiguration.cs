using System.Collections.Generic;
using System.Linq;
using DocumentFormat.OpenXml.Wordprocessing;

namespace Sidea.DocxToPdf.Renderers.Sections.Models
{
    internal class HeaderFooterConfiguration
    {
        private readonly HeaderFooterRef[] _headers;
        private readonly HeaderFooterRef[] _footers;
        private readonly bool _hasTitlePage;
        private readonly bool _useEvenOddHeadersAndFooters;

        public HeaderFooterConfiguration(
            bool hasTitlePage,
            bool useEvenOddHeadersAndFooters,
            IEnumerable<HeaderFooterRef> headers,
            IEnumerable<HeaderFooterRef> footers /*, int pageNumberStart*/)
        {
            _hasTitlePage = hasTitlePage;
            _useEvenOddHeadersAndFooters = useEvenOddHeadersAndFooters;
            _headers = headers.ToArray();
            _footers = footers.ToArray();
        }

        public string GetHeaderReferenceId(int pageNumber)
        {
            return this.GetReferenceId(_headers, pageNumber);
        }

        public string GetFooterReferenceId(int pageNumber)
        {
            return this.GetReferenceId(_footers, pageNumber);
        }

        private string GetReferenceId(IEnumerable<HeaderFooterRef> references, int pageNumber)
        {
            if(_hasTitlePage && pageNumber == 1)
            {
                return references.FirstOrDefault(r => r.Type == HeaderFooterValues.First)?.Id
                    ?? references.FirstOrDefault(r => r.Type == HeaderFooterValues.Default)?.Id;
            }

            if(!_useEvenOddHeadersAndFooters || pageNumber % 2 == 1)
            {
                return references.FirstOrDefault(r => r.Type == HeaderFooterValues.Default)?.Id;
            }

            return references.FirstOrDefault(r => r.Type == HeaderFooterValues.Even)?.Id
                ?? references.FirstOrDefault(r => r.Type == HeaderFooterValues.Default)?.Id;
        }
    }
}
