﻿using Sidea.DocxToPdf.Core;
using Sidea.DocxToPdf.Models.Common;

namespace Sidea.DocxToPdf.Models
{
    internal class Page : IPage
    {
        public static readonly Page None = new Page(
            PageNumber.None,
            new PageConfiguration(Size.Zero, PageOrientation.Portrait));

        public Page(
            PageNumber pageNumber,
            PageConfiguration configuration)
        {
            this.PageNumber = pageNumber;
            this.Configuration = configuration;
        }

        public PageNumber PageNumber { get; }
        public PageConfiguration Configuration { get; }

        public Margin Margin { get; set; } = Margin.None;

        public DocumentVariables DocumentVariables { get; set; } = new DocumentVariables(0);

        public Rectangle GetContentRegion()
        {
            return new Rectangle(
                this.Margin.Left,
                this.Margin.Top,
                this.Configuration.Width - this.Margin.HorizontalMargins,
                this.Configuration.Height - this.Margin.VerticalMargins);
        }

        public Rectangle GetPageRegion()
            => new Rectangle(0, 0, this.Configuration.Width, this.Configuration.Height);
    }
}
