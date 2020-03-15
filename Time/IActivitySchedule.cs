using System;

namespace ShUtilities.Time
{
    public interface IActivitySchedule
    {
        bool IsActiveTime(DateTime time);
    }
}