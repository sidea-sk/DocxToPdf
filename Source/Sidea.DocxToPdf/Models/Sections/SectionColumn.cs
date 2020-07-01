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
        private readonly ContainerElement[] _childs;

        //public SectionColumn(
        //    SectionBreak sectionBreak,
        //    IEnumerable<OpenXml.OpenXmlCompositeElement> openXmlElements,
        //    IStyleFactory styleFactory)
        //{
        //    this.SectionBreak = sectionBreak;

        //    _openXmlElements = openXmlElements.ToArray();
        //    _styleFactory = styleFactory;
        //}

        private SectionColumn(IEnumerable<ContainerElement> childs, SectionBreak sectionBreak)
        {
            _childs = childs.ToArray();
            this.SectionBreak = sectionBreak;
        }

        public static SectionColumn Create(
            SectionBreak sectionBreak,
            IEnumerable<OpenXml.OpenXmlCompositeElement> openXmlElements,
            IStyleFactory styleFactory)
        {
            var childs = openXmlElements
                .CreateInitializeElements(styleFactory)
                .ToArray();

            return new SectionColumn(childs, sectionBreak);
        }

        public SectionBreak SectionBreak { get; }

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

            this.ResetPageRegionsFrom(_childs);
        }

        public override void Render(IRenderer renderer)
        {
            _childs.Render(renderer);
        }
    }
}
