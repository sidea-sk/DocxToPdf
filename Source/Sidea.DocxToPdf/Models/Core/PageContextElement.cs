using System;
using System.Collections.Generic;
using System.Linq;
using Sidea.DocxToPdf.Core;
using Sidea.DocxToPdf.Models.Common;

namespace Sidea.DocxToPdf.Models
{
    internal abstract class PageContextElement : PageElement
    {
        public abstract void Prepare(
            PageContext pageContext,
            Func<PagePosition, PageContextElement, PageContext> nextPageContextFactory);
    }
}
