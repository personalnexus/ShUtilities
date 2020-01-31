using System;
using System.Collections.Generic;

namespace ShUtilities.Threading.ActionScheduling
{
    public class PriorityActionSchedulerOptions
    {
        public PriorityActionSchedulerOptions(string name, params int[] threadCountsByPriority)
        {
            Name = name;
            ThreadCountsByPriority = threadCountsByPriority;
            MaximumWaitDuration = TimeSpan.FromMilliseconds(512);
        }

        public string Name { get; }
        public IReadOnlyList<int> ThreadCountsByPriority { get; }
        public TimeSpan MaximumWaitDuration { get; set; }
    }
}
