using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sidea.DocxToPdf.Tests
{
    [TestClass]
    public class FooterTests : TestBase
    {
        public FooterTests() : base("Footers")
        {
        }

        [TestMethod]
        public void HelloWorld()
        {
            this.Generate(nameof(HelloWorld));
        }

        [TestMethod]
        public void Default()
        {
            this.Generate(nameof(Default));
        }

        [TestMethod]
        public void EvenOdd()
        {
            this.Generate(nameof(EvenOdd));
        }

        [TestMethod]
        public void FirstEvenOdd()
        {
            this.Generate(nameof(FirstEvenOdd));
        }

        [TestMethod]
        public void FirstEvenOddXL()
        {
            this.Generate(nameof(FirstEvenOddXL));
        }
    }
}
