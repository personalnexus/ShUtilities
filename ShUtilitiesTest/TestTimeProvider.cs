using ShUtilities.Time;
using System;

namespace ShUtilitiesTest
{
    /// <summary>
    /// An implementation of ITimeProvider whose time can be set to simulate progression of time in tests
    /// </summary>
    public class TestTimeProvider : ITimeProvider, IDisposable
    {
        public DateTime Now { get; set; }

        public long Ticks => Now.Ticks;

        private ITimeProvider _previousTimeProvider;

        public static TestTimeProvider SetDefault(DateTime now)
        {
            var result = new TestTimeProvider { Now = now };
            result._previousTimeProvider = TimeProvider.Default;
            TimeProvider.Default = result;
            return result;
        }

        public void Dispose()
        {
            if (_previousTimeProvider != null)
            {
                TimeProvider.Default = _previousTimeProvider;
                _previousTimeProvider = null;
            }
        }
    }
}
