using System.Collections.Generic;
using System.Linq;
using DocumentFormat.OpenXml.Wordprocessing;

namespace Sidea.DocxToPdf.Renderers.Sections.Models
{
    internal class HeaderFooterConfiguration
    {
        public static readonly HeaderFooterConfiguration Empty = new HeaderFooterConfiguration(false, false, new HeaderFooterRef[0], new HeaderFooterRef[0]);

        private readonly HeaderFooterRef[] _headers;
        private readonly HeaderFooterRef[] _footers;
        private readonly bool _hasTitlePage;
        private readonly bool _useEvenOddHeadersAndFooters;

        private HeaderFooterConfiguration(
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

        public HeaderFooterConfiguration Inherited(
            bool hasTitlePage,
            bool useEvenOddHeadersAndFooters,
            IEnumerable<HeaderFooterRef> headers,
            IEnumerable<HeaderFooterRef> footers)
        {
            var h = headers.ToArray();
            if(h.Length == 0)
            {
                h = _headers.ToArray();
            }

            var f = footers.ToArray();
            if (f.Length == 0)
            {
                f = _footers.ToArray();
            }

            return new HeaderFooterConfiguration(hasTitlePage, useEvenOddHeadersAndFooters, h, f);
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
