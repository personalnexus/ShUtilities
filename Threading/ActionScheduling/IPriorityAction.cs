namespace ShUtilities.Threading.ActionScheduling
{
    public interface IPriorityAction
    {
        void SetSchedulerQueue(object newQueue);
        bool TryExtractSchedulerQueue(object expectedQueue);
        void Execute();
    }
}
