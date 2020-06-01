using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShUtilities.Text.Markdown;

namespace ShUtilitiesTest.Tests
{
    [TestClass]
    public class MarkdownGeneratorTest
    {
        [TestMethod]
        public void GenerateSimpleText()
        {
            var generator = new MarkdownGenerator();
            string text = generator
                          .Heading1("Heading 1")
                          .Text("This is a demo of text in ").Bold("bold").Text(", ").Italic("italic").LineEnd()
                          .Text("and ").Code("mono-spaced").ParagraphEnd()
                          .Heading2("Subheading: lists")
                          .Paragraph("First a numbered list:")
                          .Numbered("Some text", "other text", 42).Numbered("A new numbered list")
                          .Paragraph("Then a bullet list:")
                          .Bullet(1, "another bullet")
                          .ToString();
            TestDataUtility.AreEqual(text, ".md");
        }
    }
}
