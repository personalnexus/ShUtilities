using System;

namespace ShUtilities.Threading
{
    public interface IActorOptions
    {
        void ScheduleAsyncExecution(Action action);

        void OperationStarted()
        {
        }

        void OperationCompleted()
        {
        }
    }
}