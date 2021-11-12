using BenchmarkDotNet.Attributes;
using ShUtilitiesTest.Mocks;

namespace ShUtilitiesTest.Benchmarks
{
    public class PropertyChangedBenchmark
    {
        public PropertyChangedBenchmark()
        {
            _mock = new();
            _mock.PropertyChanged += (sender, e) => { };
        }

        private PropertyChangedMock _mock;
        private long _counter;

        [Benchmark(Baseline = true)]
        public void Direct() => _mock.Direct = _counter++;

        [Benchmark]
        public void SetAndRaise() => _mock.SetAndRaise = _counter++;

        [Benchmark]
        public void SetAndRaiseWithReflection() => _mock.SetAndRaiseWithReflection = _counter++;
    }
}
