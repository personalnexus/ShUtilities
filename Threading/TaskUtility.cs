using System;
using System.Threading;
using System.Threading.Tasks;

namespace ShUtilities.Threading
{
    public static class TaskUtility
    {
        /// <summary>
        /// Use to start tasks that should cancel when a newer iteration of the same task is starting, e.g. a long-running operation that you do 
        /// not want the output of as soon as there is a newer input to start the operation again.
        /// </summary>
        /// <param name="action"></param>
        /// <param name="cancellationTokenSource"></param>
        /// <returns></returns>
        public static Task RunCancellable(Action<CancellationToken> action, ref CancellationTokenSource cancellationTokenSource)
        {
            if (cancellationTokenSource != null)
            {
                cancellationTokenSource.Cancel();
                cancellationTokenSource.Dispose();
            }
            cancellationTokenSource = new CancellationTokenSource();
            CancellationToken token = cancellationTokenSource.Token;
            Task result = Task.Run(() => action(token), token);
            return result;
        }
    }
}
