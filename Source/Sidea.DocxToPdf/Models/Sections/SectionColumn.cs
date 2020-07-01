using System;
using System.Collections.Generic;
using System.Linq;
using Sidea.DocxToPdf.Core;
using Sidea.DocxToPdf.Models.Styles;
using OpenXml = DocumentFormat.OpenXml;

namespace Sidea.DocxToPdf.Models.Sections
{
    internal class SectionColumn : ContainerElement
    {
        private readonly OpenXml.OpenXmlCompositeElement[] _openXmlElements;
        private readonly IStyleFactory _styleFactory;

        private ContainerElement[] _childs = new ContainerElement[0];

        public SectionColumn(
            SectionBreak sectionBreak,
            IEnumerable<OpenXml.OpenXmlCompositeElement> openXmlElements,
            IStyleFactory styleFactory)
        {
            this.SectionBreak = sectionBreak;

            _openXmlElements = openXmlElements.ToArray();
            _styleFactory = styleFactory;
        }

        public SectionBreak SectionBreak { get; }

        public override void Initialize()
        {
            _childs = _openXmlElements
                .CreateInitializeElements(_styleFactory)
                .ToArray();
        }

        public override void Prepare(PageContext pageContext, Func<PageNumber, ContainerElement, PageContext> pageFactory)
        {
            var currentPageContext = pageContext;

            Func<PageNumber, ContainerElement, PageContext> onNewPage = (pageNumber, childElement) => 
            {
                var c = pageFactory(pageNumber, this);
                currentPageContext = c;
                return c;
            };

            Rectangle availableRegion = pageContext.Region;
            foreach (var child in _childs)
            {
                child.Prepare(new PageContext(currentPageContext.PageNumber, availableRegion, currentPageContext.PageVariables), onNewPage);
                var lastPage = child.LastPageRegion.Region;

                availableRegion = currentPageContext.Region.Clip(lastPage.BottomLeft);
            }

            this.UpdatePageRegionsFromChildren();
        }

        public override void Render(IRenderer renderer)
        {
            _childs.Render(renderer);
        }

        private void UpdatePageRegionsFromChildren()
        {
            this.ClearPageRegions();
            var pageRegions = _childs.UnionPageRegions();
            foreach(var pageRegion in pageRegions)
            {
                this.SetPageRegion(pageRegion);
            }
        }
    }
}
