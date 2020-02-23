using System.Threading;

namespace ShUtilities.Diagnostics.Counters
{
    internal class ResetCounterValue: IIncrementableCounter
    {
        private long _written; //TODO: add padding so this field is in its own cache line

        public long Current { get; private set; }
        public long Today { get; private set; }
        public long Total { get; private set; }

        public void Increment() => _written++;

        public void Update(bool isFirstOfTheDay)
        {
            long previousTotal = Total;
            long newTotal = Volatile.Read(ref _written);
            Current = newTotal - previousTotal;
            Total = newTotal;
            Today = isFirstOfTheDay ? Current : Today + Current;
        }
    }
}