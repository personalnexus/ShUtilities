using System;
using System.Collections.Generic;

namespace ShUtilities.Threading.PriorityScheduling
{
    /// <summary>
    /// Options for creating <see cref="PriorityScheduler"/>.
    /// </summary>
    public class PrioritySchedulerOptions
    {
        public PrioritySchedulerOptions(string name, params int[] threadCountsByPriority)
        {
            Name = name;
            ThreadCountsByPriority = threadCountsByPriority;
            MaximumSpinDuration = TimeSpan.FromSeconds(1);
            MaximumWaitDuration = TimeSpan.FromSeconds(5);
        }

        public string Name { get; }
        public IReadOnlyList<int> ThreadCountsByPriority { get; private set;  }
        public TimeSpan MaximumSpinDuration { get; set; }
        public TimeSpan MaximumWaitDuration { get; set; }

        public void ChangeThreadCounts(params int[] newThreadCountsByPriority)
        {
            var oldThreadCountsByPriority = ThreadCountsByPriority;
            ThreadCountsByPriority = newThreadCountsByPriority;
            var eventArgs = new PrioritySchedulerOptionsChangedEventArgs
            {
                OldThreadCountsByPriority = oldThreadCountsByPriority,
                NewThreadCountsByPriority = newThreadCountsByPriority
            };
            OnChanged(eventArgs);
        }

        public event EventHandler<PrioritySchedulerOptionsChangedEventArgs> Changed;

        protected void OnChanged(PrioritySchedulerOptionsChangedEventArgs eventArgs)
        {
            Changed?.Invoke(this, eventArgs);
        }
    }
}
