using System.Threading;
using ShUtilities.Threading;

namespace ShUtilities.Diagnostics.Counters
{
    internal class ResetCounterValue: IIncrementableCounter
    {
        //
        // Writing from one thread
        //
        
        private Padded<long> _written;

        public void Increment() => _written.Value++;

        //
        // Reading from one other thread
        //

        public long Current { get; private set; }
        public long Today { get; private set; }
        public long Total { get; private set; }

        public void Update(bool isFirstOfTheDay)
        {
            long previousTotal = Total;
            long newTotal = Volatile.Read(ref _written.Value);
            Current = newTotal - previousTotal;
            Total = newTotal;
            Today = isFirstOfTheDay ? Current : Today + Current;
        }
    }
}