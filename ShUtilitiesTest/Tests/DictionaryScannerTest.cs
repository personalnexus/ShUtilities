using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShUtilities.Collections;
using ShUtilities.Text;
using System.Collections.Generic;

namespace ShUtilitiesTest.Tests
{
    [TestClass]
    public class DictionaryScannerTest
    {
        [TestMethod]
        public void ParseEmptyString()
        {
            CheckParsedDictionary("", new Dictionary<string, string>());
        }

        [TestMethod]
        public void ParseLeadingLineBreaks()
        {
            CheckParsedDictionary("\n\r\nKey1=Value1", new Dictionary<string, string> { ["Key1"] = "Value1" });
        }

        [TestMethod]
        public void ParseTrailingLineBreaks()
        {
            CheckParsedDictionary("Key1=Value1\n\r\n", new Dictionary<string, string> { ["Key1"] = "Value1" });
        }

        [TestMethod]
        public void ParseOnlyLineBreaks()
        {
            CheckParsedDictionary("\n\r\n", new Dictionary<string, string>());
        }

        [TestMethod]
        public void ParseSingleKey()
        {
            CheckParsedDictionary("Key", new Dictionary<string, string> { ["Key"] = "" });
        }

        [TestMethod]
        public void ParseSingleValue()
        {
            CheckParsedDictionary("=Value", new Dictionary<string, string> { [""] = "Value" });
        }

        [TestMethod]
        public void ParseVariousCrLfVariations()
        {
            CheckParsedDictionary("Key1=Value1\r\nKey2=\nKey3=Value3\rKey4=Value4\n\n\n\n",
                new Dictionary<string, string>
                {
                    ["Key1"] = "Value1",
                    ["Key2"] = "",
                    ["Key3"] = "Value3",
                    ["Key4"] = "Value4",
                });
        }

        [TestMethod]
        public void ParseRelevantKeysVariousCrLfVariations()
        {
            CheckParsedDictionary("Key1=Value1\r\nKey2=\nKey3=Value3\rKey4=Value4\nKey=Value\n\n\n",
                new Dictionary<string, string>
                {
                    ["Key1"] = "Value1",
                    ["Key2"] = "",
                    ["Key3"] = "Value3",
                },
                "Key1", "Key2", "Key3");
        }

        private static void CheckParsedDictionary(string input, Dictionary<string, string> expectedResult, params string[] relevantKeys)
        {
            var actualResult = new Dictionary<string, string>();

            if (relevantKeys.Length == 0)
            {
                DictionaryScanner.Parse(input, actualResult);
            }
            else
            {
                var dictionaryRelevantKeys = new DictionaryRelevantKeys(relevantKeys);
                dictionaryRelevantKeys.Clear();
                DictionaryScanner.Parse(input, dictionaryRelevantKeys, actualResult);
            }
            
            DictionaryAssert.AreEqual(expectedResult, actualResult);
        }
    }
}
