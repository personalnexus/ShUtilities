namespace ShUtilities.Threading.ActionScheduling
{
    public interface IPriorityAction
    {
        object SetSchedulerQueue(object newQueue);
        void Execute();
    }
}
