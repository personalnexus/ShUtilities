using System;

namespace ShUtilities.Time
{
    public class AlwaysActiveSchedule : IActivitySchedule
    {
        internal static readonly AlwaysActiveSchedule Default = new AlwaysActiveSchedule();

        public bool IsActiveTime(DateTime time) => true;
    }
}
