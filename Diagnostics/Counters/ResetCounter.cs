using ShUtilities.Collections;
using System;
using System.Threading;

namespace ShUtilities.Diagnostics.Counters
{
    /// <summary>
    /// A counter that can be updated and resets in regular intervals, e.g. to track number of operations per minute. 
    /// Each thread operates on their own internal counter field, reducing shared state to a minimum.
    /// </summary>
    public class ResetCounter: IIncrementableCounter
    {
        private ResetCounterValue[] _values;
        private DateTime _lastUpdate;
        
        /// <summary>
        /// Initialize for a certain number of threads. Currently this value is 128, but that is not contractual.
        /// </summary>
        public ResetCounter(): this(128)
        {
        }

        /// <summary>
        /// Initialize with the number of threads you expect to use the counter. If there end up being more, 
        /// they will be made room for, but this can be a bit expensive, so better be generous up front.
        /// </summary>
        public ResetCounter(int capacity)
        {
            _values = new ResetCounterValue[capacity];
            _values.FillNew<ResetCounterValue>();
        }

        //
        // Writing from any thread
        //

        public void Increment() => GetOrAdd(Thread.CurrentThread.ManagedThreadId).Increment();

        internal IIncrementableCounter GetOrAdd(int index)
        {
            if (index >= _values.Length)
            {
                ResetCounterValue[] oldValues, newValues;
                do
                {
                    oldValues = Volatile.Read(ref _values);
                    newValues = new ResetCounterValue[index + 1];
                    oldValues.CopyTo(newValues, 0);
                    newValues.FillNew<ResetCounterValue>(oldValues.Length);
                }
                while (oldValues != Interlocked.CompareExchange(ref _values, newValues, oldValues));
            }
            return _values[index];
        }

        //
        // Reading fom one thread at a time
        //

        /// <summary>
        /// After <see cref="Update"/> was called, contains the number of operations counted since the previous update, i.e. the sum of Current values from all threads
        /// </summary>
        public long Current { get; private set; }
        /// <summary>
        /// After <see cref="Update"/> was called, contains the number of operations counted since the first update of the day, i.e. the sum of Today values from all threads
        /// </summary>
        public long Today { get; private set; }
        /// <summary>
        /// After <see cref="Update"/> was called, contains the number of all operations ever counted, i.e. the sum of Total values from all threads
        /// </summary>
        public long Total { get; private set; }

        /// <summary>
        /// Collects values from each thread and aggregates them. Call only from one thread at a time, as this method does no locking on its own.
        /// </summary>
        public void Update()
        {
            DateTime newUpdate = Time.TimeProvider.Default.Now;
            bool isFirstOfTheDay = _lastUpdate.Date != newUpdate.Date;
            _lastUpdate = newUpdate;

            Current = Today = Total = 0;

            foreach (ResetCounterValue value in _values)
            {
                value.Update(isFirstOfTheDay);
                Current += value.Current;
                Today += value.Today;
                Total += value.Total;
            }
        }
    }
}
