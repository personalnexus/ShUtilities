using System.Runtime.CompilerServices;

namespace ShUtilities.Threading.Awaiters
{
    /// <summary>
    /// Not technically necessary, but helpful to remember what to implement on an awaiter, see https://weblogs.asp.net/dixin/understanding-c-sharp-async-await-2-awaitable-awaiter-pattern
    /// </summary>
    public interface IAwaiter : INotifyCompletion
    {
        bool IsCompleted { get; }
        void GetResult();
    }
}