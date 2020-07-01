﻿using Sidea.DocxToPdf.Core;
using Sidea.DocxToPdf.Models.Common;

namespace Sidea.DocxToPdf.Models
{
    internal class PageContext
    {
        public PageContext(
            PageNumber pageNumber,
            Rectangle region,
            Variables variables)
        {
            this.PageNumber = pageNumber;
            this.Region = region;
            this.PageVariables = new PageVariables(this.PageNumber, variables.TotalPages);
            this.TopLeft = new DocumentPosition(this.PageNumber, this.Region.TopLeft);
        }

        public PageNumber PageNumber { get; }
        public Rectangle Region { get; }
        public PageVariables PageVariables { get; }

        public DocumentPosition TopLeft { get; }

        public PageContext Clip()
        {
            return this;
        }

        public PageContext Crop(Margin margin)
            => this.Crop(margin.Top, margin.Right, margin.Bottom, margin.Left);

        public PageContext Clip(Point point, double width)
        {
            var region = this.Region.Clip(point, width);
            return this.WithRegion(region);
        }

        public PageContext Crop(double top, double right, double bottom, double left)
        {
            var region = this.Region.Crop(top, right, bottom, left);
            return this.WithRegion(region);
        }

        private PageContext WithRegion(Rectangle region)
        {
            return new PageContext(this.PageNumber, region, this.PageVariables);
        }
    }
}
