using System;
using System.Collections.Generic;
using System.Text;
using Sidea.DocxToPdf.Core;

using Word = DocumentFormat.OpenXml.Wordprocessing;

namespace Sidea.DocxToPdf.Models.Tables
{
    internal class Table : ContainerElement
    {
        private readonly Word.Table _wordTable;

        public Table(Word.Table wordTable)
        {
            _wordTable = wordTable;
        }

        public override void Initialize()
        {

        }

        public override void Prepare(PageContext pageContext, Func<PageNumber, ContainerElement, PageContext> pageFactory)
        {
        }

        public override void Render(IRenderer renderer)
        {
        }
    }
}
