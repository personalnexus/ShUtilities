using System;
using System.Collections.Generic;

namespace ShUtilities.Threading.ActionScheduling
{
    /// <summary>
    /// Options for creating <see cref="PriorityActionScheduler"/>.
    /// </summary>
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
