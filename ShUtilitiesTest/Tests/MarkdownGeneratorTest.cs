using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShUtilities.Text.Markdown;
using System.IO;

namespace ShUtilitiesTest.Tests
{
    [TestClass]
    public class MarkdownGeneratorTest : AssertionScopedTestBase
    {
        [TestClass]
        public class ToString
        {

            [TestMethod]
            public void GenerateSimpleText()
            {
                // Arrange
                var generator = new MarkdownGenerator()
                    .Heading1("Heading 1")
                          .Text("This is a demo of text in ").Bold("bold").Text(", ").Italic("italic").LineEnd()
                    .Text("and ").Code("mono-spaced").ParagraphEnd()
                    .Heading2("Subheading: lists")
                    .Paragraph("First a numbered list:")
                    .Numbered("Some text", "other text", 42).Numbered("A new numbered list")
                    .Paragraph("Then a bullet list:")
                    .Bullet(1, "another bullet")
                    .Code("SingleLine").ParagraphEnd()
                    .Code("LineOne", "LineTwo");

                // Act
                string text = generator.ToString();

                // Assert
                text.Should().Be(File.ReadAllText(@"TestData\GenerateSimpleText.md"));
            }
        }
    }
}
