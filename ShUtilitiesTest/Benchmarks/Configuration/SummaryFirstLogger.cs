using BenchmarkDotNet.Loggers;
using System;
using System.Text;

namespace ShUtilitiesTest.Benchmarks.Configuration
{
    public class SummaryFirstLogger : ILogger
    {
        private StringBuilder _buffer = new StringBuilder();
        private bool _useBuffer = true;

        public string Id => nameof(SummaryFirstLogger);

        public int Priority => 1;

        public void Flush() => Console.Write(_buffer.ToString());

        public void WriteLine(LogKind logKind, string text)
        {
            Write(logKind, text);
            WriteLine();
        }

        public void Write(LogKind logKind, string text) => WriteBuffered(text);

        public void WriteLine() => WriteBuffered(Environment.NewLine);

        private void WriteBuffered(string text)
        {
            // Buffer everything up to "summary" and write it in one go before cleanup
            if (text == "// * Summary *")
            {
                _useBuffer = false;
            }
            if (_useBuffer)
            {
                _buffer.Append(text);
            }
            else
            {
                Console.Write(text);
            }
        }
    }
}
