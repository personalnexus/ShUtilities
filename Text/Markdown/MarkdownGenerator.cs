using System.Text;

namespace ShUtilities.Text.Markdown
{
    /// <summary>
    /// Generates text formatted with basic markdown
    /// </summary>
    public class MarkdownGenerator: ITextGenerator
    {
        private readonly StringBuilder _text = new StringBuilder();
        
        //
        // ITextGenerator
        //

        public ITextGenerator Heading1(string heading) => Heading(1, heading);

        public ITextGenerator Heading2(string heading) => Heading(2, heading);

        public ITextGenerator Heading3(string heading) => Heading(3, heading);

        public ITextGenerator Heading(int level, string heading) => Append(new string('#', level)).AppendLine(heading).AppendLine();

        public ITextGenerator Text(string text) => Append(text);

        public ITextGenerator Link(string displayText, string target) => Append("[").Append(displayText).Append("](").Append(target).Append(")");

        public ITextGenerator Bold(string text) => Append("**").Append(text).Append("**");

        public ITextGenerator Italic(string text) => Append("_").Append(text).Append("_");

        public ITextGenerator Code(string text) => Append("`").Append(text).Append("`");

        public ITextGenerator LineEnd() => AppendLine("  ");

        public ITextGenerator Paragraph(string text) => AppendLine(text).AppendLine();

        public ITextGenerator ParagraphEnd() => AppendLine().AppendLine();

        public ITextGenerator Numbered(params object[] items)
        {
            int number = 0;
            foreach (object item in items)
            {
                _text.Append(++number);
                _text.Append(". ");
                _text.Append(item);
                _text.AppendLine();
            }
            return AppendLine();
        }

        public ITextGenerator Bullet(params object[] items)
        {
            foreach (object item in items)
            {
                _text.Append("* ");
                _text.Append(item);
                _text.AppendLine();
            }
            return AppendLine();
        }

        public override string ToString() => _text.ToString();

        //
        // Internal helpers
        //

        private MarkdownGenerator Append(string text)
        {
            _text.Append(text);
            return this;
        }

        private MarkdownGenerator AppendLine(string text)
        {
            _text.AppendLine(text);
            return this;
        }

        private MarkdownGenerator AppendLine()
        {
            _text.AppendLine();
            return this;
        }
    }
}
