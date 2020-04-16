namespace Sidea.DocxToPdf
{
    public class RenderingOptions   
    {
        public static readonly RenderingOptions Default = new RenderingOptions();

        public RenderingOptions(bool renderParagraphCharacter = false)
        {
            this.RenderParagraphCharacter = renderParagraphCharacter;
        }

        public bool RenderParagraphCharacter { get; }
    }
}
