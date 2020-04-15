using System.Collections.Generic;

namespace ShUtilities.Collections
{
    public static class QueueExtensions
    {
        // .NET 4
        ///// <summary>
        ///// Tries to dequeue an item if the queue is not empty, in which case false is returned.
        ///// </summary>
        //public static bool TryDequeue<T>(Queue<T> queue, out T item)
        //{
        //    bool result = queue.Count > 0;
        //    item = result ? queue.Dequeue() : default;
        //    return result;
        //}

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
