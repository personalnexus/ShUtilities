using System.Collections.Generic;

namespace ShUtilities.Collections
{
    public static class QueueExtensions
    {
        /// <summary>
        /// Dequeues up to <paramref name="maxDequeueCount"/> items or the number of items in the queue, whichever is smaller, returning them as an <see cref="IEnumerable{T}"/>.
        /// </summary>
        public static IEnumerable<T> DequeueMany<T>(this Queue<T> queue, int maxDequeueCount)
        {
            for (int i = 0; i < maxDequeueCount && queue.Count > 0; i++)
            {
                yield return queue.Dequeue();
            }
        }
    }
}
