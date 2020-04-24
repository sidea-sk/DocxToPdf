using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sidea.DocxToPdf.Tests
{
    [TestClass]
    public class ImagesTests : TestBase
    {
        public ImagesTests() : base("Images")
        {
        }

        [TestMethod]
        public void Image()
        {
            this.Generate(nameof(Image));
        }
    }
}
