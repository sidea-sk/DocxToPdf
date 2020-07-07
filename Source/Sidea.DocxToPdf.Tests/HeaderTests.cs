using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sidea.DocxToPdf.Tests
{
    [TestClass]
    public class HeaderTests : TestBase
    {
        public HeaderTests() : base("Headers", useNextGeneration: true)
        {
            this.Options = RenderingOptions.WithDefaults(header: true);
        }

        [TestMethod]
        public void HelloWorld()
        {
            this.Generate(nameof(HelloWorld));
        }

        [TestMethod]
        public void XXL()
        {
            this.Generate(nameof(XXL));
        }

        [TestMethod]
        public void Default()
        {
            this.Generate(nameof(Default));
        }

        [TestMethod]
        public void OddEven()
        {
            this.Generate(nameof(OddEven));
        }

        [TestMethod]
        public void FirstEvenOddEvenOdd()
        {
            this.Generate(nameof(FirstEvenOddEvenOdd));
        }

        [TestMethod]
        public void FirstEvenOddXXL()
        {
            this.Generate(nameof(FirstEvenOddXXL));
        }
    }
}
