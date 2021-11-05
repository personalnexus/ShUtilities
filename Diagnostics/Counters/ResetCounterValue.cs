using System.Threading;

namespace ShUtilities.Diagnostics.Counters
{
    internal class ResetCounterValue: IIncrementableCounter
    {
        //
        // Writing from one thread with padding to avoid false sharing https://mechanical-sympathy.blogspot.com/2011/07/false-sharing.html
        //

        #pragma warning disable CS0169
        private long _padding1, _padding2, _padding3, _padding4, _padding5, _padding6, _padding7;
        #pragma warning restore CS0169

        private long _written;
        
        #pragma warning disable CS0169
        private long _padding8, _padding9, _padding10, _padding11, _padding12, _padding13, _padding14;
        #pragma warning restore CS0169

        public void Increment() => _written++;

        //
        // Reading from one other thread
        //

        public long Current { get; private set; }
        public long Today { get; private set; }
        public long Total { get; private set; }

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