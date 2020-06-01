namespace ShUtilities.Text
{
    public interface ITextGenerator
    {
        ITextGenerator Bold(string text);
        ITextGenerator Code(string text);
        ITextGenerator Heading(int level, string heading);
        ITextGenerator Heading1(string heading);
        ITextGenerator Heading2(string heading);
        ITextGenerator Heading3(string heading);
        ITextGenerator Italic(string text);
        ITextGenerator LineEnd();
        ITextGenerator Link(string displayText, string target);
        ITextGenerator Paragraph(string text);
        ITextGenerator ParagraphEnd();
        ITextGenerator Text(string text);
        ITextGenerator Numbered(params object[] items);
        ITextGenerator Bullet(params object[] items);
        string ToString();
    }
}
