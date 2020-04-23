using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sidea.DocxToPdf.Tests
{
    [TestClass]
    public class HeaderTests : TestBase
    {
        public HeaderTests() : base("Headers")
        {
        }

        [TestMethod]
        public void Header()
        {
            this.Generate(nameof(Header));
        }
    }
}
