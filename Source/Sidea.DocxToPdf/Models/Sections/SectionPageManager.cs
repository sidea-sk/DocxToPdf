using System;
using System.Collections.Generic;
using System.Linq;
using Sidea.DocxToPdf.Core;

namespace Sidea.DocxToPdf.Models.Sections
{
    internal class SectionPageManager : IPageManager
    {
        private readonly PageConfiguration _pageConfiguration;
        private readonly Action<Page> _onPageCreated;
        private List<Page> _pages = new List<Page>();
        private PageNumber _pagesFrom;

        public SectionPageManager(
            PageConfiguration pageConfiguration,
            Action<Page> onPageCreated)
        {
            _pageConfiguration = pageConfiguration;
            _onPageCreated = onPageCreated;
            _pagesFrom = PageNumber.None;
        }

        public IReadOnlyCollection<IPage> Pages => _pages;

        public void EnsurePage(PageNumber pageNumber)
        {
            if(_pages.Any(p => p.PageNumber == pageNumber)) 
            {
                return;
            }

            var beginOnPage = _pages.Count == 0
                ? 1 + _pagesFrom
                : _pages.Last().PageNumber + 1;

            for(var i = beginOnPage; i <= pageNumber; i++)
            {
                var page = new Page(new PageNumber(i), _pageConfiguration);
                _onPageCreated(page);
                _pages.Add(page);
            }
        }

        public IPage GetPage(PageNumber pageNumber)
        {
            return _pages.Single(p => p.PageNumber == pageNumber);
        }

        public void ShiftPages(PageNumber startFrom)
        {
            _pagesFrom = startFrom;
            _pages = Enumerable
                .Range(startFrom, _pages.Count)
                .Select(pageNumber => {
                    var page = new Page(new PageNumber(pageNumber), _pageConfiguration);
                    _onPageCreated(page);
                    return page;
                })
                .ToList();
            
        }
    }
}
