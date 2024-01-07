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

        /// <summary>
        /// Sets a new <see cref="TestTimeProvider"/> as the default time provider using the current time as a starting point for Now.
        /// Use this overload when the actual time is not as important as the change in time.
        /// </summary>
        /// <returns></returns>
        public static TestTimeProvider SetDefault() => SetDefault(DateTime.Now);

        /// <summary>
        /// Sets a new <see cref="TestTimeProvider"/> as the default time provider using the given time as a starting point for Now.
        /// </summary>
        public static TestTimeProvider SetDefault(DateTime now)
        {
            var result = new TestTimeProvider { Now = now };
            result._previousTimeProvider = ShUtilities.Time.TimeProvider.Default;
            ShUtilities.Time.TimeProvider.Default = result;
            return result;
        }

        public void Dispose()
        {
            if (_previousTimeProvider != null)
            {
                ShUtilities.Time.TimeProvider.Default = _previousTimeProvider;
                _previousTimeProvider = null;
            }
        }
    }
}
