using System;
using System.Collections.Generic;

namespace ShUtilities.Threading.PriorityScheduling
{
    public class PrioritySchedulerOptionsChangedEventArgs: EventArgs
    {
        public IReadOnlyList<int> OldThreadCountsByPriority { get; set; }
        public IReadOnlyList<int> NewThreadCountsByPriority { get; set; }
    }
}