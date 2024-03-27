namespace ShUtilities.Threading.DelayedActions
{
    public interface ICancelable
    {
        bool TryCancel();
    }
}