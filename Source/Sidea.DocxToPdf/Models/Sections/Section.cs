using System.Collections.Generic;
using Sidea.DocxToPdf.Core;

namespace Sidea.DocxToPdf.Models.Sections
{
    internal class Section
    {
        private List<IPage> _pages = new List<IPage>();
        public IReadOnlyCollection<IPage> Pages { get; }

        public Section()
        {

        }

        public void Prepare(object previousPageInfo)
        {

        }

        public void Update(object previousPageInfo)
        {

        }
    }
}
